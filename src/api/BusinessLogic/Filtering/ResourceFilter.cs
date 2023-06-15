using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using BusinessLogic.Abstractions;
using DataAccess.Enums;

namespace BusinessLogic.Filtering
{
    public class ResourceFilter : CustomFilterBase
    {
        [OperatorComparison(OperatorType.Equal)]
        public int? Id { get; set; }

        [StringFilterOptions(StringFilterOption.Contains)]
        public string Name { get; set; }

        [StringFilterOptions(StringFilterOption.Contains)]
        public string Description { get; set; }

        [OperatorComparison(OperatorType.Equal)]
        public ResourceStatus? Status { get; set; }
    }
}
