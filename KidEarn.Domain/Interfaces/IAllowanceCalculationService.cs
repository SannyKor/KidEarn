using KidEarn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Interfaces
{
    public interface IAllowanceCalculationService
    {
        /// <summary>
        /// Розраховує тижневий коефіцієнт ставки на основі списку оцінок за період.
        /// </summary>
        /// <param name="grades">Колекція оцінок з підвантаженими предметами (Subject).</param>
        /// <returns>Коефіцієнт від 0.00 до 5.00</returns>
        decimal CalculateWeeklyMultiplier(IEnumerable<Grade> grades);

        /// <summary>
        /// Повертає ваговий коефіцієнт для конкретного балу (1-12).
        /// </summary>
        decimal GetGradeWeight(int gradeValue);
    }
}
