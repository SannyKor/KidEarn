using KidEarn.Infrastructure.Data;
using KidEarn.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidEarn.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly KidEarnDbContext _context;

        // Впровадження залежності (Constructor Dependency Injection)
        public DashboardController(KidEarnDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            // Для MVP, якщо id не передано, беремо першу доступну дитину
            var childQuery = _context.Children
                .AsNoTracking()
                .Include(c => c.DepositAccount)
                .Include(c => c.Grades)
                .ThenInclude(g => g.Subject);

            var child = id.HasValue
                ? await childQuery.FirstOrDefaultAsync(c => c.Id == id.Value)
                : await childQuery.FirstOrDefaultAsync();

            if (child == null)
            {
                return NotFound("Профіль дитини не знайдено. Переконайтеся, що Seeding відпрацював.");
            }

            // Мапінг сутності на ViewModel
            var viewModel = new DashboardViewModel
            {
                ChildId = child.Id,
                ChildName = $"{child.FirstName} {child.LastName}".Trim(),
                NutBalance = child.NutBalance,
                DepositBalance = child.DepositAccount?.Balance ?? 0.00m,
                DepositInterestRate = (child.DepositAccount?.InterestRate ?? 0.00m) * 100, // Переводимо у відсотки
                DailyBaseAllowance = child.DailyBaseAllowance,
                CurrentMultiplier = child.CurrentWeeklyMultiplier,
                RecentGrades = child.Grades
                    .OrderByDescending(g => g.DateReceived)
                    .Select(g => new GradeItemViewModel
                    {
                        SubjectName = g.Subject.Name,
                        Value = g.Value,
                        DateReceived = g.DateReceived,
                        Comment = g.Comment
                    })
                    .ToList()
            };

            return View(viewModel);
        }
    }
}
