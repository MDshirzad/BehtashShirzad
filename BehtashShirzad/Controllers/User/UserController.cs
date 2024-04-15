
using BehtashShirzad.Controllers.Attrubutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.Controllers.User
{
    public class UserController : Controller
    {
        [JwtAuthorization("user")]
        public IActionResult Index()
        {
           
             
            return View();
        }


        [JwtAuthorization("admin")]
        public IActionResult Test()
        {


            return View();
        }
    }
}
