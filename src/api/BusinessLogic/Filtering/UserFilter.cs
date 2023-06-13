using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using BusinessLogic.Abstractions;

namespace BusinessLogic.Filtering
{
    public class UserFilter : CustomFilterBase
    {
        [StringFilterOptions(StringFilterOption.Equals)]
        public string Id { get; set; }


        [StringFilterOptions(StringFilterOption.Contains)]
        public string Email { get; set; }


        [StringFilterOptions(StringFilterOption.Contains)]
        public string FullName { get; set; }
    }
}
