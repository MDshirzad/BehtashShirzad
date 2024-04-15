
using BehtashShirzad.Controllers.Attrubutes;
using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.Controllers.User
{
    public class UserController : Controller
    {
        [JwtAuthorization("user")]
        public IActionResult Index()
        {

            var user = HttpContext.Request.Cookies.TryGetValue("Token", out var token);

            


            return View();
        }

        

        [JwtAuthorization("admin")]
        public IActionResult Test()
        {


            return View();
        }
    }
}
