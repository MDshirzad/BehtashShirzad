
using BehtashShirzad.Controllers.Attrubutes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.Controllers.User
{
    public class UserController : Controller
    {
        [JwtAuthorization]
        public IActionResult Index()
        {
           
             
            return View();
        }
    }
}
