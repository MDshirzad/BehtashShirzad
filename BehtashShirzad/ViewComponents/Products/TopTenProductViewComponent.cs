using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.ViewComponents.Products
{
    public class TopTenProductViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await ProductDAL.GetTopTenProducts();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Components/ProductComponent.cshtml", model));
        }


    }
}
