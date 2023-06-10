using BusinessLogic.Abstractions;

namespace BusinessLogic.Filtering
{
    public class UserFilter : CustomFilterBase
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
    }
}
