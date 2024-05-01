using BehtashShirzad.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
 
 
using ApiCommunicator;
using SharedObjects.DTO;
using Services;
namespace BehtashShirzad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

      

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Products()
        {

            return View();
        }

        public IActionResult Cart()
        {

            return View();
        }

        public IActionResult Courses()
        {

            return View();
        }

        public IActionResult Sources()
        {

            return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
