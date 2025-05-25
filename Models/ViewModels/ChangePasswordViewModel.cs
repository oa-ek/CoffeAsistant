using System.ComponentModel.DataAnnotations;

namespace CafeAssistiant.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Поточний пароль обов'язковий")]
        [DataType(DataType.Password)]
        [Display(Name = "Поточний пароль")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Новий пароль обов'язковий")]
        [StringLength(100, ErrorMessage = "Пароль має бути не менше {2} символів.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю обов'язкове")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердьте новий пароль")]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}
