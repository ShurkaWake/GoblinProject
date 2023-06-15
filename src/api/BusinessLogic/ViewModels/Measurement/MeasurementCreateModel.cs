using BusinessLogic.Enums;

namespace BusinessLogic.ViewModels.Measurement
{
    public class MeasurementCreateModel
    {
        public decimal Weight { get; set; }
        
        public WeightUnits Units { get; set; }

        public int ShiftId { get; set; }
    }
}
