using System.ComponentModel.DataAnnotations;

namespace KidEarn.Web.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введіть номер телефону")]
    [Display(Name = "Номер телефону")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введіть пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Запам'ятати мене")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
