using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeAssistiant.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        public string? Coordinates { get; set; } // формат: "50.4501,30.5234"

        [Required]
        public DateTime Ordered { get; set; }

        [Required]
        public DateTime Arrive { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
