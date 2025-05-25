namespace CafeAssistiant.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; } // кількість в мл або г
        public string Unit { get; set; } = "г"; // мл або г


        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();


    }
}
