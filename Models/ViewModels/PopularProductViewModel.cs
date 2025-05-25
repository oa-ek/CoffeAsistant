namespace CafeAssistiant.Models.ViewModels
{
    public class PopularProductViewModel
    {
        public string ProductName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public DateTime? LastOrderDate { get; set; }
    }
}
