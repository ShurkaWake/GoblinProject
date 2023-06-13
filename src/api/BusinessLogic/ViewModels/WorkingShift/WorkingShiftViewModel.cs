namespace BusinessLogic.ViewModels.WorkingShift
{
    public class WorkingShiftViewModel
    {
        public int Id { get; set; }

        public string ForemanId { get; set; }

        public int BusinessId { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }
}
