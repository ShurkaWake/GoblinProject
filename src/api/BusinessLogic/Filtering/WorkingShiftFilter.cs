using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using BusinessLogic.Abstractions;

namespace BusinessLogic.Filtering
{
    public class WorkingShiftFilter : CustomFilterBase
    {
        [OperatorComparison(OperatorType.Equal)]
        public int? Id { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string ForemanId { get; set; }

        [OperatorComparison(OperatorType.GreaterThanOrEqual)]
        public DateTime? Start { get; set; }

        [OperatorComparison(OperatorType.LessThanOrEqual)]
        public DateTime? End { get; set; }
    }
}
