using System.ComponentModel.DataAnnotations;

namespace CafeAssistiant.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [Display(Name = "Повне ім'я")]
        public string FullName { get; set; }

        [Display(Name = "Місто")]
        public string City { get; set; }

        [Display(Name = "Адреса")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Неправильний формат Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
