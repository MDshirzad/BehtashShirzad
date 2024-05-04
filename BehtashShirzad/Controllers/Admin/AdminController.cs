using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = UserDAL.GetUsers();
            return View(users);
        }

        public IActionResult Products()
        {
            var products = ProductDAL.GetProductsAdmin();
            return View(products);
        }
    }
}
