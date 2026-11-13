namespace KidEarn.Web.ViewModels
{
    public class ChildSummaryViewModel
    {
        public int ChildId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal DailyBaseAllowance { get; set; }
        public decimal CurrentMultiplier { get; set; }
        public decimal NutBalance { get; set; }
    }
}
