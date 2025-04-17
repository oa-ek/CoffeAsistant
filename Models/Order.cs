namespace CafeAssistiant.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime DateOrder { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    }
}
