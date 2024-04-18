using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.ViewComponents.Products.ProductSeperatedComponents
{
    public class TopThreeSourcesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = ProductDAL.GetTopThreeSources();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Components/SourcesComponent.cshtml", model));
        }


    }
}
