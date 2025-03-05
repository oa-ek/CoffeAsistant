namespace CafeAssistiant.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public int Salary { get; set; }
        public DateTime EmployeeCooperation { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}
