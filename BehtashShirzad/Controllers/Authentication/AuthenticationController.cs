using ApiCommunicator;
using BehtashShirzad.Models.ApiModels;
using BehtashShirzad.Model;
using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Model.Context.DAL;
using BehtashShirzad.Tools;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Services;
using SharedObjects.DTO;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Newtonsoft.Json;
using Logger;
using static SharedObjects.Constants;
using System.Reflection;


namespace BehtashShirzad.Controllers.Authentication
{
    public class AuthenticationController : Controller
    {

  
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login([FromForm]UserLoginDto loginUser)
        {
            var user = await UserDAL.GetUser(loginUser);
            Log.CreateLog(new() { LogType = LogType.Info,  Description = LogonRequestText, Extra=loginUser.Credential.ToString() });

            if (string.IsNullOrEmpty(loginUser.Credential)|| string.IsNullOrEmpty(loginUser.Password))
                return NotFound();

            
            if (user is not null)
            {
                if (user.isVerified)
                {

                user.Password = "";

                    var claims = new List<Claim>
            {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("isAdmin", user.isAdmin.ToString()) ,
              new Claim("isVerified", user.isVerified.ToString()) ,
              new Claim(ClaimTypes.Role,user.Role.ToString())
           
            // Add additional claims as needed
        };


                    var token = Infrastructure.GenerateToken(claims);
                    if (!string.IsNullOrEmpty(token))
                    {

                        Response.Cookies.Append("Token", token);
                        
                        Log.CreateLog(new() { LogType = LogType.Success,  Description = OkResponse });
                        return RedirectToAction("Index","User");

                    }
                    return Problem("InnerError");

                   
                }
                else
                {
                    return Unauthorized("UserNotVerified");
                }
                
           
            }

            return NotFound();
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> Register([FromForm] UserRegistrationDto userRegister)
        {
            Log.CreateLog(new() { LogType = LogType.Info,  Description = RegisterRequestText, Extra = userRegister.ToString() });


            if (   Infrastructure.IsPasswordLengthValid(userRegister.Password)==SharedObjects.Constants.Status.NotCorrect  ) {

                return BadRequest("طول کلمه عبور نامتعارف است");
            }

            if (Infrastructure.IsPhoneNumberLengthValid( userRegister.PhoneNumber)==SharedObjects.Constants.Status.NotCorrect)
            {
                 return BadRequest("طول شماره موبایل نامتعارف است");
            }
          
           
            if (Infrastructure.IsPhoneNumberFormatValid(userRegister.PhoneNumber)==SharedObjects.Constants.Status.NotCorrect)
            {
                return BadRequest("شماره موبایل نامتعارف است");
            }

            var result = await UserDAL.CreateUser(userRegister);
            switch (result)
            {

                case SharedObjects.Constants.Status.Fail:
                    return BadRequest("ثبت نام انجام نشد");

                case SharedObjects.Constants.Status.UserExists:
                    return BadRequest("نام کاربری و یا شماره موبایل وجود دارد");

                case SharedObjects.Constants.Status.Registered:
                    var otpRes = await CallOtp(new() { To = userRegister.PhoneNumber });
                    if (otpRes == SharedObjects.Constants.Status.SENT)
                    {
                        return Ok("OtpSent");
                    }
                    else if (otpRes == SharedObjects.Constants.Status.NotSent)
                    {
                        return Ok("OtpNotSent");
                    }

                    return Ok("InnerError");
            }

         

            
            return BadRequest("خطا داخلی");
            
           
        }

 
        private async Task<SharedObjects.Constants.Status> CallOtp([FromForm] OtpDto otpDto)
        {
            var service = Provider.GetInstance(SharedObjects.Constants.ApiType.SMS_OTP);
             return await  service.Call(otpDto);
         


        }

        [HttpPost]
        public async Task<IActionResult> VerifyByOtp([FromForm] VerifyByOtpDto otpP )
        {
            using(var redis = new RedisCommunicator()) {
                var userOtpredis = await redis.GetValueAsync(otpP.phoneNumber);
                if (string.IsNullOrEmpty(userOtpredis))
                {
                    return BadRequest("OtpNotExist");

                }
                if (userOtpredis == otpP.otp)
                {
                    var updateRes = await UserDAL.VerifyUser(otpP.phoneNumber);
                    if (updateRes)
                    {

                    return Ok("Verified");
                    }
                    return Problem("NotVerified");
                }
                return BadRequest("NotVerified");
                    }

        }

        [HttpPost]
        public async Task<IActionResult> VerifyNumber([FromForm] VerifyNumberDto user)
        {
            if (string.IsNullOrEmpty(user.PhoneNumber))
                return BadRequest("PhoneNumberEmpty");
            if (string.IsNullOrEmpty(user.UserName))
                return BadRequest("UsernameEmpty");
            if (Infrastructure.IsPhoneNumberLengthValid(user.PhoneNumber) == SharedObjects.Constants.Status.NotCorrect)
            {
                return BadRequest("طول شماره موبایل نامتعارف است");
            }
            if (Infrastructure.IsPhoneNumberFormatValid(user.PhoneNumber) == SharedObjects.Constants.Status.NotCorrect)
            {
                return BadRequest("شماره موبایل نامتعارف است");
            }
            
            using(var redis = new RedisCommunicator())
            {
                var value = await redis.GetValueAsync(user.PhoneNumber);
                if (!string.IsNullOrEmpty(value))
                    return BadRequest("کد ارسال شده است");
            }

            var userDb =await UserDAL.GetUser(new(){ Credential=user.UserName });
              if (userDb == null)
            {
                return BadRequest("UserNotExistOrPasswordIncorrect");
            }

            var updateNumber =  await UserDAL.UpdateUserNumber(new() {Username=user.UserName,PhoneNumber=user.PhoneNumber });
            if (updateNumber == SharedObjects.Constants.Status.Success)
            {
    
            var otpRes = await CallOtp(new() { To=user.PhoneNumber });
                    switch (otpRes)
                    {
                        case SharedObjects.Constants.Status.SENT:
                            return Ok("OtpSent");
                        case SharedObjects.Constants.Status.NotSent:
                            return Ok("OtpNotSent");
                        
                        case SharedObjects.Constants.Status.Fail:
                            return Problem("InnerError");
                       
                          
                     
                    }
                    return BadRequest();
                
            }else if (updateNumber == SharedObjects.Constants.Status.UserVerified)
            {
                return BadRequest("UserVerified");
            }
            return Problem("InnerError");
        }

        
    
    }
}
