using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KidEarn.Web.ViewModels
{
    public class CreateGradeViewModel
    {
        [Required]
        public int ChildId { get; set; }

        [Required(ErrorMessage = "Оберіть шкільний предмет")]
        [Display(Name = "Шкільний предмет")]
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Вкажіть отриману оцінку")]
        [Range(1, 12, ErrorMessage = "Оцінка має бути в межах від 1 до 12 балів")]
        [Display(Name = "Оцінка (1-12)")]
        public int? Value { get; set; }

        [Required(ErrorMessage = "Вкажіть дату отримання оцінки")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateOnly DateReceived { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [MaxLength(200, ErrorMessage = "Коментар не може перевищувати 200 символів")]
        [Display(Name = "Коментар або тема уроку")]
        public string? Comment { get; set; }

        // Колекція для випадаючого списку (Dropdown)
        public IEnumerable<SelectListItem> AvailableSubjects { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
