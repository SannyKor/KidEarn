using System.ComponentModel.DataAnnotations;

namespace KidEarn.Web.ViewModels
{
    public class CreateChildViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я дитини")]
        [MaxLength(50, ErrorMessage = "Ім'я дитини не може перевищувати 50 символів")]
        [Display(Name = "Ім'я дитини")]

        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Введіть прізвище дитини")]
        [MaxLength(50, ErrorMessage = "Прізвище дитини не може перевищувати 50 символів")]
        [Display(Name = "Прізвище дитини")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть номер телефону")]
        [Phone(ErrorMessage = "Некоректний формат номеру телефону")]
        [RegularExpression(@"^\+?380\d{9}$", ErrorMessage = "Введіть номер у форматі +380XXXXXXXXX")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Задайте PIN-код або пароль для входу дитини")]
        [StringLength(20, ErrorMessage = "Мінімальна довжина — {2} символи", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "PIN-код / Пароль (мін. 4 символи)")]
        public string PinCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть базову щоденну ставку")]
        [Range(1, 10000, ErrorMessage = "Ставка має бути від 1 до 10 000 Nut")]
        [Display(Name = "Базова денна ставка (Nut)")]
        public decimal DailyBaseAllowance { get; set; } = 20.00m;

        [Range(0, 100, ErrorMessage = "Відсоток має бути від 0 до 100%")]
        [Display(Name = "Ставка по депозиту (%)")]
        public decimal DepositInterestRatePercent { get; set; } = 5.00m;
    }
}
