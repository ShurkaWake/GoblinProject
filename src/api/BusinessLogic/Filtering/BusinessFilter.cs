using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using BusinessLogic.Abstractions;

namespace BusinessLogic.Filtering
{
    public class BusinessFilter : CustomFilterBase
    {
        public int Id { get; set; }

        [StringFilterOptions(StringFilterOption.Equals)]
        public string Name { get; set; }

        [StringFilterOptions(StringFilterOption.Contains)]
        public string Location { get; set; }
    }
}
