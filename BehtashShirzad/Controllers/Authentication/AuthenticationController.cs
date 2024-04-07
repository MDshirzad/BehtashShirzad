using BehtashShirzad.Models.ApiModels;
using ElliotStore.Model.ApiModels;
using ElliotStore.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;
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
            if (    userRegister.Password.Trim().Length<6 || userRegister.Password.Trim().Length > 12) {

                return BadRequest("طول کلمه عبور نامتعارف است");
            }

            if (userRegister.PhoneNumber.Trim().Length != 11)
            {
                return BadRequest("طول شماره موبایل نامتعارف است");
            }
            /// Error Regex not good
            /// 
            //var IranianPatternNumber = "^09(1[0-9] |2[0-2] |3[0-9] |9[0-9]) [0-9]{7}$";

            //Regex r = new Regex(IranianPatternNumber);
            //var isvalid = r.IsMatch(userRegister.PhoneNumber.Trim());
            //if (!isvalid)
            //{
            //    return BadRequest("شماره موبایل نامتعارف است");
            //}
            ///
            var result = await UserDAL.CreateUser(userRegister);
            if (result)
            {
                return Ok("ثبت نام انجام شد");
            }
            return BadRequest("ثبت نام انجام نشد");
        }
    }
}
