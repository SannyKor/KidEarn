using KidEarn.Domain.Entities;
using KidEarn.Domain.Interfaces;
using KidEarn.Infrastructure.Data;
using KidEarn.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KidEarn.Web.Controllers
{
    public class GradesController : Controller
    {
        private readonly KidEarnDbContext _context;
        private readonly IAllowanceCalculationService _calculationService;

        public GradesController(
            KidEarnDbContext context,
            IAllowanceCalculationService calculationService)
        {
            _context = context;
            _calculationService = calculationService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? childId)
        {
            // Якщо childId не передано, беремо першу активну дитину
            var targetChildId = childId ?? await _context.Children.Select(c => c.Id).FirstOrDefaultAsync();
            if (targetChildId == 0)
            {
                return NotFound("Не знайдено жодного профілю дитини.");
            }

            var viewModel = new CreateGradeViewModel
            {
                ChildId = targetChildId,
                AvailableSubjects = await GetSubjectsSelectListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Повторно наповнюємо список перед поверненням форми з помилками
                model.AvailableSubjects = await GetSubjectsSelectListAsync();
                return View(model);
            }

            // Завантажуємо дитину разом з її наявними оцінками та предметами
            var child = await _context.Children
                .Include(c => c.Grades)
                    .ThenInclude(g => g.Subject)
                .FirstOrDefaultAsync(c => c.Id == model.ChildId);

            if (child == null)
            {
                return NotFound("Профіль дитини не знайдено.");
            }

            var grade = new Grade
            {
                ChildId = model.ChildId,
                SubjectId = model.SubjectId!.Value,
                Value = model.Value!.Value,
                DateReceived = model.DateReceived,
                Comment = model.Comment?.Trim()
            };

            _context.Grades.Add(grade);
            child.Grades.Add(grade);

            // Перераховуємо очікуваний коефіцієнт
            child.CurrentWeeklyMultiplier = _calculationService.CalculateWeeklyMultiplier(child.Grades);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Оцінку {grade.Value} успішно збережено! Коефіцієнт оновлено.";
            return RedirectToAction("Index", "Dashboard", new { id = child.Id });
        }

        private async Task<IEnumerable<SelectListItem>> GetSubjectsSelectListAsync()
        {
            return await _context.Subjects
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Name} (вага: x{s.Weight})"
                })
                .ToListAsync();
        }
    }
}
