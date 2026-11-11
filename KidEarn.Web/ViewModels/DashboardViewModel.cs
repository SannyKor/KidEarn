using System.Globalization;

namespace KidEarn.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; } = string.Empty;

        // Фінансові показники
        public decimal NutBalance { get; set; }
        public decimal DepositBalance { get; set; }
        public decimal DepositInterestRate { get; set; }

        // Показники ставки
        public decimal DailyBaseAllowance { get; set; }
        public decimal CurrentMultiplier { get; set; }
        // Формула: Базова ставка * Множник
        public decimal ExpectedDailyAllowance => Math.Round(DailyBaseAllowance * CurrentMultiplier, 2);

        // Відформатований рядок для UI з доменною валютою
        public string FormattedExpectedDailyAllowance =>
            $"{ExpectedDailyAllowance.ToString("F2", CultureInfo.InvariantCulture)} Nut";

        // Відформатований множник для додаткової інформації
        public string FormattedMultiplier =>
            $"x{CurrentMultiplier.ToString("F2", CultureInfo.InvariantCulture)}";

        // Розрахована виплата на сьогодні
        public decimal EffectiveDailyAllowance => Math.Round(DailyBaseAllowance * CurrentMultiplier, 2);

        // Оцінки за поточний період для відображення у віджеті
        public List<GradeItemViewModel> RecentGrades { get; set; } = new();
    }
}
