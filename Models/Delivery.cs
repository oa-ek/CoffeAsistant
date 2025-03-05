namespace CafeAssistiant.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime Ordered { get; set; }
        public DateTime Arrive { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
