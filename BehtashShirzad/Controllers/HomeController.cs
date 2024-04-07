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

        public async void Test()
        {
           

        }

        public IActionResult Index()
        {
            Test();
            return View();
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }
        [Route("/Register")]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Privacy()
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
