using ElliotStore.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.ViewComponents
{
    public class ProductViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = ProductDAL.GetProducts();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Components/ProductComponent.cshtml", model));
        }

    }
}
