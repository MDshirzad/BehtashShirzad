using System.Text.Json.Serialization;

namespace BehtashShirzad.Models.DbModels
{
    public class Product
    {
        public ProductCategory Category { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string image { get; set; }
        public decimal Price { get; set; }
        public bool IsVisible { get; set; } = true;
        [JsonIgnore()]
        public List<Invoice> Invoices { get; set; }

    }
}
