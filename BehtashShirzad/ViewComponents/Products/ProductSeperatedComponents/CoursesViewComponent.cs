using BehtashShirzad.Model.Context.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BehtashShirzad.ViewComponents.Products.ProductSeperatedComponents
{
    public class CoursesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = ProductDAL.GetCourses();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Components/CoursesComponent.cshtml", model));
        }


    }
}
