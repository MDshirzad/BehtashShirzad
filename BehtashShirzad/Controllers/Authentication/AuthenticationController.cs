﻿using ApiCommunicator;
using BehtashShirzad.Models.ApiModels;
using ElliotStore.Model;
using ElliotStore.Model.ApiModels;
using ElliotStore.Model.Context.DAL;
using ElliotStore.Tools;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Services;
using SharedObjects.DTO;
using System.Text.RegularExpressions;

namespace BehtashShirzad.Controllers.Authentication
{
    public class AuthenticationController : Controller
    {
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login([FromForm]UserLoginDto loginUser)
        {
            var user =await UserDAL.GetUser(loginUser);
            if (user is not null)
            {
                if (user.isVerified)
                {

                user.Password = "";
                    return Ok(user);
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
        public async Task<IActionResult> VerifyNumber([FromForm] UserLoginDto user)
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

            var userDb =await UserDAL.GetUser(user);
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
