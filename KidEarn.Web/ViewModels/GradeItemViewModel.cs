namespace KidEarn.Web.ViewModels
{
    public class GradeItemViewModel
    {
        public string SubjectName { get; set; } = string.Empty;
        public int Value { get; set; }
        public DateOnly DateReceived { get; set; }
        public string? Comment { get; set; }
    }
}
