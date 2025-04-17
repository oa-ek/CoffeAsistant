namespace CafeAssistiant.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }  // Якщо треба вказувати кількість товарів у замовленні
    }
}
