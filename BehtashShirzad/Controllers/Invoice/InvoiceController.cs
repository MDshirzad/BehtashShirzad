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
        public async Task<IActionResult> Create([FromBody] InvoiceDto invoice)
        {
            try
            {

                var currentUser = Infrastructure.GetCurrentuserId(HttpContext.Request.Cookies["Token"]);
               
                var username = Infrastructure.GetCurrentuser(HttpContext.Request.Cookies["Token"]);
                var userBeforeProducts = new List<Product>();
                var userInvoices = InvoiceDAL.GetInvoiceByuser(username);
                if (userInvoices.Count() > 0 && userInvoices != null)
                {
                    foreach (var item in userInvoices)
                    {
                        foreach (var product in item.Products!)
                        {
                            userBeforeProducts.Add(product);
                        }
                    }

                }

                var products = new List<Product>();
                foreach (var item in invoice.products)
                {
                    var product =await ProductDAL.GetProductByName(item);
                    if (!userBeforeProducts.Exists(_ => _.Id == product.Id))
                    {
                        products.Add(product) ;
                    }

                }

                 
                if (products.Count<=0)
                {
                    return NotFound();
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
