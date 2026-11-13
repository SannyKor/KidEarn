namespace KidEarn.Web.ViewModels
{
    public class FamilyIndexViewModel
    {
        public string FamilyName { get; set; } = string.Empty;
        public List<FamilyMemberItemViewModel> Parents { get; set; } = new List<FamilyMemberItemViewModel>();
        public List<ChildSummaryViewModel> Children { get; set; } = new List<ChildSummaryViewModel>();
    }
}
