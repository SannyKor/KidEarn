using KidEarn.Domain.Entities;
using KidEarn.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(KidEarnDbContext context, IAllowanceCalculationService calculationService)
        {
            // Перевіряємо, чи застосовані всі міграції перед сидуванням
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            // 1. Сидування шкільних предметів (Subjects)
            if (!await context.Subjects.AnyAsync())
            {
                var subjects = new List<Subject>
            {
                new() { Name = "Математика", Weight = 1.30m },
                new() { Name = "Українська мова", Weight = 1.20m },
                new() { Name = "Англійська мова", Weight = 1.30m },
                new() { Name = "Інформатика", Weight = 1.20m },
                new() { Name = "Історія України", Weight = 1.10m },
                new() { Name = "Природознавство / Наука", Weight = 1.00m },
                new() { Name = "Література", Weight = 1.00m },
                new() { Name = "Образотворче мистецтво", Weight = 1.00m },
                new() { Name = "Фізична культура", Weight = 1.00m }
            };

                await context.Subjects.AddRangeAsync(subjects);
                await context.SaveChangesAsync();
            }
            // 2. Сидування тестової дитини для демонстрації
            if (!await context.Children.AnyAsync())
            {
                var math = await context.Subjects.FirstAsync(s => s.Name == "Математика");
                var english = await context.Subjects.FirstAsync(s => s.Name == "Англійська мова");

                var child = new Child
                {
                    FirstName = "Анна",
                    LastName = "Коваленко",
                    DailyBaseAllowance = 20.00m, // Базова ставка 20 Nut
                    NutBalance = 50.00m,
                    ParentId = "demo-parent-id",
                    DepositAccount = new DepositAccount
                    {
                        Balance = 100.00m,
                        InterestRate = 0.0500m
                    }
                };

                // Додаємо кілька свіжих оцінок за поточний період
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                child.Grades.Add(new Grade { Value = 11, DateReceived = today.AddDays(-2), Subject = math, Comment = "Контрольна робота" });
                child.Grades.Add(new Grade { Value = 12, DateReceived = today.AddDays(-1), Subject = english, Comment = "Діалог" });

                // Розраховуємо коефіцієнт одразу через наш сервіс
                child.CurrentWeeklyMultiplier = calculationService.CalculateWeeklyMultiplier(child.Grades);

                await context.Children.AddAsync(child);
                await context.SaveChangesAsync();
            }
        }
    }
}
