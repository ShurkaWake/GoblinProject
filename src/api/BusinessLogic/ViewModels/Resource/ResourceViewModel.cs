using DataAccess.Entities;
using DataAccess.Enums;

namespace BusinessLogic.ViewModels.Resource
{
    public class ResourceViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MoneyAmount Ammortization { get; set; }

        public ResourceStatus Status { get; set; }
    }
}
