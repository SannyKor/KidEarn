using System.ComponentModel.DataAnnotations;

namespace KidEarn.Web.ViewModels
{
    public class RegisterParentViewModel
    {
        [Required(ErrorMessage = "Вкажіть ваше ім'я")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть ваше прізвище")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть назву сім'ї")]
        [Display(Name = "Назва сім'ї")]
        public string FamilyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть номер телефону")]
        [Phone(ErrorMessage = "Некоректний формат номеру телефону")]
        [RegularExpression(@"^\+?380\d{9}$", ErrorMessage = "Введіть номер у форматі +380XXXXXXXXX")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} повинен містити щонайменше {2} символів.", MinimumLength = 4)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження пароля")]
        [Compare("Password", ErrorMessage = "Пароль та підтвердження не співпадають.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
