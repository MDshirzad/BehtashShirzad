
using BehtashShirzad.Controllers.Attrubutes;
using BehtashShirzad.Model.Context.DAL;
using BehtashShirzad.Models.ApiModels;
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

        [JwtAuthorization("admin")]
        public async Task<string> VerifyUser([FromBody] VerifyUserInAdminModel phone)
        {

            var veruser = await UserDAL.VerifyUser(phone.phone.ToString());
            if (veruser)
            {
                return "Ok";
            }
            return "Error";
        }

        [JwtAuthorization("admin")]
        public async Task<string> ChangePassword([FromBody] ChangePasswordUser cred)
        {

            var res = await UserDAL.ChangePassword(cred);
            if (res)
            {
                return "Ok";
            }
            return "Error";
        }

		[JwtAuthorization("admin")]
		public async Task<string> SetAdmin([FromBody] SetAdminDto cred)
		{

			var res = await UserDAL.SetAdmin(cred);
			if (res)
			{
				return "Ok";
			}
			return "Error";
		}

        [JwtAuthorization("admin")]
        public async Task<string> SetUser([FromBody] SetAdminDto cred)
        {

            var res = await UserDAL.SetUser(cred);
            if (res)
            {
                return "Ok";
            }
            return "Error";
        }


    }
}
