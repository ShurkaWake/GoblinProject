using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using BusinessLogic.Abstractions;

namespace BusinessLogic.Filtering
{
    public class ScalesFilter : CustomFilterBase
    {
        [OperatorComparison(OperatorType.Equal)]
        public int? Id { get; set; }

        [StringFilterOptions(StringFilterOption.Contains)]
        public string SerialNumber { get; set; }
    }
}
