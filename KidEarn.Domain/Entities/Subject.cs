using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Ваговий коефіцієнт предмета для розрахунку (наприклад: 1.0 - стандарт, 1.5 - профільний/важкий предмет)
        public decimal Weight { get; set; } = 1.00m;

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
