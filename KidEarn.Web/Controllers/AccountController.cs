using KidEarn.Domain.Entities;
using KidEarn.Infrastructure.Data;
using KidEarn.Infrastructure.Data.Identity;
using KidEarn.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KidEarn.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly KidEarnDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            KidEarnDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterParent()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new RegisterParentViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterParent(RegisterParentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var normalizedPhone = NormalizePhoneNumber(model.PhoneNumber);

            // Перевірка, чи не зайнятий номер телефону
            var existingUser = await _userManager.FindByNameAsync(normalizedPhone);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Користувач із цим номером телефону вже існує.");
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Створюємо сутність сім'ї
                var family = new Family
                {
                    Name = model.FamilyName.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Families.Add(family);
                await _context.SaveChangesAsync();

                // 2. Створюємо користувача-батька
                var user = new ApplicationUser
                {
                    UserName = normalizedPhone,
                    PhoneNumber = normalizedPhone,
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    FamilyId = family.Id,
                    PhoneNumberConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user, model.Password);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    await transaction.RollbackAsync();
                    return View(model);
                }

                // 3. Додаємо роль Parent
                await _userManager.AddToRoleAsync(user, "Parent");

                await transaction.CommitAsync();

                // 4. Логінимо користувача
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["SuccessMessage"] = $"Сім'ю '{family.Name}' створено!";
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Помилка при створенні сім'ї. Спробуйте пізніше.");
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var normalizedPhone = NormalizePhoneNumber(model.PhoneNumber);

            // Вхід за нормалізованим номером (який записано в UserName)
            var result = await _signInManager.PasswordSignInAsync(
                normalizedPhone,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Невірний номер телефону або пароль.");
            return View(model);
        }

        // Утилітарний метод нормалізації формату номера
        private static string NormalizePhoneNumber(string phone)
        {
            return phone.Trim().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
