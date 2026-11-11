using KidEarn.Domain.Entities;
using KidEarn.Domain.Interfaces;
using KidEarn.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(
        KidEarnDbContext context,
        IAllowanceCalculationService calculationService,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        // Створення системних ролей у Identity
        string[] roles = ["Parent", "Child"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Сидування шкільних предметів (Subjects)
        if (!await context.Subjects.AnyAsync())
        {
            var subjects = new List<Subject>
            {
                new() { Name = "Математика", Weight = 1.30m },
                new() { Name = "Українська мова", Weight = 1.20m },
                new() { Name = "Англійська мова", Weight = 1.30m },
                new() { Name = "Інформатика", Weight = 1.20m },
                new() { Name = "Історія України", Weight = 1.10m },
                new() { Name = "Природознавство", Weight = 1.00m },
                new() { Name = "Фізична культура", Weight = 1.00m }
            };

            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();
        }

        // Сидування тестової сім'ї та користувача-батька
        if (!await context.Families.AnyAsync())
        {
            var family = new Family
            {
                Name = "Сім'я Коваленків"
            };

            context.Families.Add(family);
            await context.SaveChangesAsync();

            // Створюємо демо-акаунт батька за номером телефону
            var parentPhone = "+380970000001";
            var parentUser = await userManager.FindByNameAsync(parentPhone);
            if (parentUser == null)
            {
                parentUser = new ApplicationUser
                {
                    UserName = parentPhone,
                    PhoneNumber = parentPhone,
                    FirstName = "Олександр",
                    LastName = "Коваленко",
                    FamilyId = family.Id,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(parentUser, "Parent123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(parentUser, "Parent");
                }
            }

            // Створюємо профіль дитини, прив'язаний до сім'ї
            var math = await context.Subjects.FirstAsync(s => s.Name == "Математика");
            var english = await context.Subjects.FirstAsync(s => s.Name == "Англійська мова");

            var child = new Child
            {
                FirstName = "Анна",
                LastName = "Коваленко",
                FamilyId = family.Id,
                DailyBaseAllowance = 20.00m,
                NutBalance = 50.00m,
                DepositAccount = new DepositAccount
                {
                    Balance = 100.00m,
                    InterestRate = 0.0500m
                }
            };

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            child.Grades.Add(new Grade { Value = 11, DateReceived = today.AddDays(-2), Subject = math, Comment = "Контрольна робота" });
            child.Grades.Add(new Grade { Value = 12, DateReceived = today.AddDays(-1), Subject = english, Comment = "Діалог" });

            child.CurrentWeeklyMultiplier = calculationService.CalculateWeeklyMultiplier(child.Grades);

            context.Children.Add(child);
            await context.SaveChangesAsync();
        }
    }
}