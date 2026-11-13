using KidEarn.Domain.Entities;
using KidEarn.Infrastructure.Data.Identity;
using KidEarn.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KidEarn.Web.Controllers
{
    [Authorize(Roles = "Parent")] // Доступ дозволено лише користувачам із роллю "Parent"
    public class FamilyController : Controller
    {
        private readonly KidEarnDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FamilyController(KidEarnDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.FamilyId == null)
            {
                return Forbid();
            }

            var familyId = currentUser.FamilyId.Value;

            // Завантажуємо інформацію про сім'ю та дітей
            var family = await _context.Families
                .AsNoTracking()
                .Include(f => f.Children)
                .FirstOrDefaultAsync(f => f.Id == familyId);

            if (family == null)
            {
                return NotFound("Сім'ю не знайдено.");
            }

            // Отримуємо всіх користувачів Identity, прив'язаних до цієї сім'ї
            var familyUsers = await _context.Users
                .AsNoTracking()
                .Where(u => u.FamilyId == familyId)
                .ToListAsync();

            // Визначаємо, хто з користувачів є батьками
            var parentList = new List<FamilyMemberItemViewModel>();
            foreach (var user in familyUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Parent"))
                {
                    parentList.Add(new FamilyMemberItemViewModel
                    {
                        FullName = $"{user.FirstName} {user.LastName}".Trim(),
                        PhoneNumber = user.PhoneNumber ?? "—"
                    });
                }
            }

            // Мапінг дітей з підтягуванням їхніх номерів телефону з Identity
            var childrenList = family.Children.Select(c =>
            {
                var childIdentity = familyUsers.FirstOrDefault(u => u.Id == c.IdentityUserId);
                return new ChildSummaryViewModel
                {
                    ChildId = c.Id,
                    FullName = $"{c.FirstName} {c.LastName}".Trim(),
                    PhoneNumber = childIdentity?.PhoneNumber ?? "Не прив'язано",
                    DailyBaseAllowance = c.DailyBaseAllowance,
                    CurrentMultiplier = c.CurrentWeeklyMultiplier,
                    NutBalance = c.NutBalance
                };
            }).ToList();

            var viewModel = new FamilyIndexViewModel
            {
                FamilyName = family.Name,
                Parents = parentList,
                Children = childrenList
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateChild()
        {
            return View(new CreateChildViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChild(CreateChildViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.FamilyId == null)
            {
                return Forbid();
            }

            var normalizedPhone = NormalizePhoneNumber(model.PhoneNumber);

            // Перевірка унікальності номера в межах усієї системи
            var existingUser = await _userManager.FindByNameAsync(normalizedPhone);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Цей номер телефону вже зареєстровано в системі.");
                return View(model);
            }

            // Атомарна транзакція: створення Identity User + Child Entity + DepositAccount
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Створюємо Identity акаунт для входу дитини
                var childUser = new ApplicationUser
                {
                    UserName = normalizedPhone,
                    PhoneNumber = normalizedPhone,
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    FamilyId = currentUser.FamilyId.Value,
                    PhoneNumberConfirmed = true
                };

                var createUserResult = await _userManager.CreateAsync(childUser, model.PinCode);
                if (!createUserResult.Succeeded)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    await transaction.RollbackAsync();
                    return View(model);
                }

                // Призначаємо роль Child
                await _userManager.AddToRoleAsync(childUser, "Child");

                // Створюємо доменну сутність Child та стартовий депозит
                var childEntity = new Child
                {
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    FamilyId = currentUser.FamilyId.Value,
                    IdentityUserId = childUser.Id,
                    DailyBaseAllowance = model.DailyBaseAllowance,
                    CurrentWeeklyMultiplier = 1.00m,
                    NutBalance = 0.00m,
                    DepositAccount = new DepositAccount
                    {
                        Balance = 0.00m,
                        InterestRate = model.DepositInterestRatePercent / 100m, // Переводимо % у частку (напр. 5% -> 0.05)
                        CreatedAt = DateTime.UtcNow
                    }
                };

                _context.Children.Add(childEntity);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = $"Профіль дитини {childEntity.FirstName} успішно створено! Логін: {normalizedPhone}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Помилка під час створення профілю дитини.");
                return View(model);
            }
        }

        private static string NormalizePhoneNumber(string phone)
        {
            return phone.Trim().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        }
    }
}
