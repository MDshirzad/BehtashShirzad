namespace BehtashShirzad.Models.DbModels
{
    public class Invoice
    {
        public int Id { get; set; }

        public Guid FactoNumber { get; set; } = Guid.NewGuid();
        public User? User { get; set; }
        public List<Product>? Products { get; set; }
        public decimal sum
        {
            get
            {
                if (Products == null || Products.Count == 0)
                    return 0;

                return Products.Sum(p => p.Price);
            }
        }

    }
}
