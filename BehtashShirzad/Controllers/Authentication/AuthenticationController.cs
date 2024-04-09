﻿using ApiCommunicator;
using BehtashShirzad.Models.ApiModels;
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
        public async Task<IActionResult> Login([FromBody]UserLoginDto loginUser)
        {
            var user =await UserDAL.GetUser(loginUser);
            if (user is not null)
            {
                user.Password = "";
                
            return Ok(user);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserRegistrationDto userRegister)
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
                    return Created();
               
            }
            return BadRequest("خطا داخلی");
            
           
        }


        [HttpPost]
        public async Task<IActionResult> CallOtp([FromBody] OtpDto otpDto)
        {
            var service = Provider.GetInstance(SharedObjects.Constants.ApiType.SMS_OTP);
         var result = await  service.Call(otpDto);
            if (result == SharedObjects.Constants.Status.SENT)
            {
                return Ok("Sent");
            }
            return BadRequest("NotSent");


        }

        [HttpPost]
        public async Task<IActionResult> VerifyByOtp([FromBody]VerifyByOtpDto otp )
        {
            using(var redis = new RedisCommunicator()) {
                var userOtpredis = await redis.GetValueAsync(otp.phoneNumber);
                if (string.IsNullOrEmpty(userOtpredis))
                {
                    return BadRequest("OtpNotExist");

                }
                if (userOtpredis == otp.otp)
                {
                    var updateRes = await UserDAL.VerifyUser(otp.phoneNumber);
                    if (updateRes)
                    {

                    return Ok("Verified");
                    }
                    return Problem("NotVerified");
                }
                return BadRequest("NotVerified");
                    }

        }
    
    
    }
}
