using BusinessLogic.ViewModels.Measurement;

namespace BusinessLogic.ViewModels.WorkingShift
{
    public class WorkingShiftForIot
    {
        public int Id { get; set; }

        public string Hash { get; set; }

        public DateTime? End { get; set; }

        public MeasurementViewModel Measurement { get; set; }
    }
}
