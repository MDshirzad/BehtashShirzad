using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Model.Context.DAL;
using BehtashShirzad.Models.ApiModels;
using Logger;
using Microsoft.AspNetCore.Mvc;
using SharedObjects;

namespace BehtashShirzad.Controllers
{
    public class ProductController : Controller
    {
        

        [HttpGet("/Product/{id}")]
        public async Task<IActionResult> Index([FromRoute] string id ) {
            if (id == null)
            {
                return Redirect("/Home/Products");
            }
            int.TryParse(id, out var productId);
            var product =await ProductDAL.GetProductById(productId);
            if (product is not null)
            {
            ViewBag.Product = product;
            return View("~/Views/ProductView/ProductView.cshtml");

            }
            return Redirect("/Home/Products");

        }

        [HttpPost()]
        public async Task<string> ChangeVisibility([FromBody] ChangeProductVisibilityDto changeProductVisibilityDto)
        {
            bool visibility = false;
            if (changeProductVisibilityDto.Visible.Equals("True"))
            {
                visibility = true;
            }

            var result = await ProductDAL.ChangeVisibility(changeProductVisibilityDto.Name, visibility);
            if (result == true) {
                return "Ok";
            
            }

            return "Error";
        }

        [HttpPost()]
        public async Task<string> Create([FromBody] ProductDto productdt)
        {
            Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = $"new craete request:category{  productdt.Category }, Name:{productdt.Name},Price:{productdt.Price} Descript:{productdt.Description} , Is visible:{productdt.IsVisible},image :{productdt.Image}  " });
            if (productdt == null)
            {
                HttpContext.Response.StatusCode = 400;
                return "Error";
            }
            
            var product = await ProductDAL.CreateProduct(productdt);
            if (product )
            {
                HttpContext.Response.StatusCode = 200;
                return "Ok";
                

            }
            HttpContext.Response.StatusCode = 500;
            return "Error";

        }

    }
}
