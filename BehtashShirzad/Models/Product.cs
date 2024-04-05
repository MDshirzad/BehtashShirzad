using System.Text.Json.Serialization;

namespace ElliotStore.Model
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore()]
        public List<Invoice> Invoices { get; set; }
    }
}
