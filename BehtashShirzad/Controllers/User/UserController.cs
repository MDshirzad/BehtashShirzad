 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.Controllers.User
{
    public class UserController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var jwtToken = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(jwtToken))
            {
                // JWT token not found in cookie
                // Handle the situation accordingly, e.g., return an error response or redirect to login page
                return Unauthorized();
            }

            // You can now use the JWT token as needed
            return View();
        }
    }
}
