using BehtashShirzad.Models.DbModels;
using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;
using SharedObjects;
using BehtashShirzad.Tools;
using System.Security.Claims;
using BehtashShirzad.Controllers.Attrubutes;

namespace BehtashShirzad.Controllers.Invoice
{
    public class InvoiceController : Controller
    {
        [JwtAuthorization("user","admin")]
        public async Task<IActionResult> Create(InvoiceDto invoice)
        {
            try
            {
 
            var currentUser = Infrastructure.GetClaims(HttpContext.Request.Cookies["Token"]).FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier).ToString();

                var products = new List<Product>();
                foreach (var item in invoice.products)
                {
                    Product product = new Product() { Id = item };
                    products.Add(product);  
                }
                
            var res =await InvoiceDAL.CreateInvoice(new() { User=new() {  Id=Convert.ToInt32(currentUser)},Products= products });
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
