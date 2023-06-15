namespace BusinessLogic.ViewModels.Statistics
{
    public class StatisticViewModel
    {
        public int ShiftId { get; set; }

        public DateTime Date { get; set; }

        public decimal Profit { get; set; }

        public decimal Income { get; set; }

        public decimal Costs { get; set; }
    }
}
