using KidEarn.Domain.Interfaces;
using KidEarn.Infrastructure.Data;
using KidEarn.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace KidEarn.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // База даних
            builder.Services.AddDbContext<KidEarnDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("KidEarn.Infrastructure")
            ));

            // Реєстрація бізнес-сервісів у DI
            builder.Services.AddScoped<IAllowanceCalculationService, AllowanceCalculationService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<KidEarnDbContext>();
                    var calcService = services.GetRequiredService<IAllowanceCalculationService>();
                    await DbInitializer.SeedAsync(context, calcService);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Помилка під час ініціалізації та сидування бази даних.");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            await app.RunAsync();
        }
    }
}
