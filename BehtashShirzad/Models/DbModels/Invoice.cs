namespace BehtashShirzad.Models.DbModels
{
    public class Invoice
    {
        public int Id { get; set; }

        public Guid FactoNumber { get; set; } = Guid.NewGuid();
        public User? User { get; set; }
        public List<Product>? Products { get; set; }
     

    }
}
