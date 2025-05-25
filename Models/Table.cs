namespace CafeAssistiant.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool IsReserved { get; set; }
        public DateTime? ReservedFrom { get; set; }
        public DateTime? ReservedUntil { get; set; }
    }
}
