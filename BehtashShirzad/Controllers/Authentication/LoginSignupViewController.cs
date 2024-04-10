using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.Controllers.Authentication
{
    public class LoginSignupViewController : Controller
    {
        [HttpGet]
        [Route("/Login")]
        public IActionResult Login()
        {
            return View("~/Views/LoginSignup/Login.cshtml");
        }

        [HttpGet]
        [Route("/Register")]
        public IActionResult Register()
        {
            return View("~/Views/LoginSignup/Register.cshtml");
        }
        [HttpGet]
        [Route("/Forgotpassword")]
        public IActionResult Forgotpassword()
        {
            return View("~/Views/LoginSignup/ForgotPassword.cshtml");
        }
    }
}
