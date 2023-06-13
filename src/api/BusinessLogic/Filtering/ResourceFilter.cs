using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using BusinessLogic.Abstractions;
using DataAccess.Enums;

namespace BusinessLogic.Filtering
{
    public class ResourceFilter : CustomFilterBase
    {
        public int Id { get; set; }

        [StringFilterOptions(StringFilterOption.Contains)]
        public string Name { get; set; }

        [StringFilterOptions(StringFilterOption.Contains)]
        public string Description { get; set; }

        public ResourceStatus Status { get; set; }
    }
}
