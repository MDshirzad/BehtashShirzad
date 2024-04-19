
using BehtashShirzad.Controllers.Attrubutes;
using BehtashShirzad.Model.Context.DAL;
using BehtashShirzad.Models.DbModels;
using BehtashShirzad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System.Security.Claims;

namespace BehtashShirzad.Controllers.User
{
    public class UserController : Controller
    {
        [JwtAuthorization("user")]
        public IActionResult Index()
        {

            var user = Infrastructure.GetClaims(HttpContext.Request.Cookies["Token"]).FirstOrDefault(_ => _.Type.ToLower().ToString() == "unique_name").Value; 

            var invoices = InvoiceDAL.GetInvoiceByuser(user);
            

            return View("~/Views/User/Dashboard.cshtml",invoices );
        }
       

        

         
    }
}
