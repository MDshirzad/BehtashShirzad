using BehtashShirzad.Models.DbModels;
using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;
using SharedObjects;
using BehtashShirzad.Tools;
using System.Security.Claims;

namespace BehtashShirzad.Controllers.Invoice
{
    public class InvoiceController : Controller
    {
        public async Task<IActionResult> Create(InvoiceDto invoice)
        {
            try
            {

            
            if (invoice.products == null || invoice.userId==null) throw new ArgumentNullException(nameof(invoice));

            var currentUser = Infrastructure.GetClaims(HttpContext.Request.Cookies["Token"]).Where(_=>_.Type==ClaimTypes.NameIdentifier).FirstOrDefault().ToString();
             var RequestForUser =await UserDAL.GetUserById(invoice.userId);
                if (RequestForUser.Id != Int32.Parse(currentUser)) { return Unauthorized("User are diffrent"); }
            var res =await InvoiceDAL.CreateInvoice(invoice);
            switch (res)
            {
                case Constants.Status.Fail:
                    return Problem("Error");
                case Constants.Status.Success:
                    return Ok(res);

            }
            }
            catch (Exception)
            {

                return BadRequest("badReqeust");
            }
            return BadRequest();


        }
    }
}
