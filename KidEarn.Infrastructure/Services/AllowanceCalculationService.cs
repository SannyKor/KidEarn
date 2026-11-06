using KidEarn.Domain.Entities;
using KidEarn.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Services
{
    public class AllowanceCalculationService : IAllowanceCalculationService
    {
        private const decimal MaxMultiplier = 5.00m;
        private const decimal DefaultMultiplier = 1.00m;

        public decimal GetGradeWeight(int gradeValue) => gradeValue switch
        {
            12 => 1.50m,
            11 => 1.40m,
            10 => 1.25m,
            9 => 1.15m,
            8 => 1.05m,
            7 => 1.00m, // Базовий рівень
            6 => 0.85m,
            5 => 0.70m,
            4 => 0.50m,
            3 => 0.30m,
            2 => 0.20m,
            1 => 0.10m,
            _ => 1.00m  // Невідома або позасистемна оцінка
        };

        public decimal CalculateWeeklyMultiplier(IEnumerable<Grade> grades)
        {
            var gradeList = grades.ToList();
            if (!gradeList.Any())
            {
                return DefaultMultiplier;
            }

            // Групуємо оцінки за предметами (LINQ)
            var subjectGroups = gradeList
                .GroupBy(g => g.SubjectId)
                .ToList();

            var subjectScores = new List<decimal>();

            foreach (var group in subjectGroups)
            {
                // 1. Середня вага оцінок по даному предмету
                var averageGradeWeight = group.Average(g => GetGradeWeight(g.Value));

                // 2. Вага самого предмета (якщо Subject не завантажено, беремо 1.0)
                var subjectWeight = group.First().Subject?.Weight ?? 1.00m;

                // 3. Підсумок по предмету = (середня вага оцінок) * (вага предмета)
                var subjectScore = averageGradeWeight * subjectWeight;
                subjectScores.Add(subjectScore);
            }

            // 4. Середнє значення по всіх предметах
            var rawMultiplier = subjectScores.Average();

            // 5. Округлюємо до 2 знаків та обмежуємо максимальним значенням x5
            var finalMultiplier = Math.Round(rawMultiplier, 2, MidpointRounding.AwayFromZero);

            return Math.Min(finalMultiplier, MaxMultiplier);
        }
    }
}
