using BehtashShirzad.Models.Base;
using Newtonsoft.Json;

namespace BehtashShirzad.Model.ApiModels
{
    public class ProductDto : IBaseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
