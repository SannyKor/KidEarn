using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class Grade
    {
        public int Id { get; set; }

        // Значення оцінки (наприклад, від 1 до 12)
        public int Value { get; set; }

        // Дата отримання оцінки (важливо для вибірки оцінок за конкретний тиждень)
        public DateOnly DateReceived { get; set; }

        public string? Comment { get; set; }

        // Foreign Key та навігаційна властивість на дитину
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;

        // Foreign Key та навігаційна властивість на предмет
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }
}
