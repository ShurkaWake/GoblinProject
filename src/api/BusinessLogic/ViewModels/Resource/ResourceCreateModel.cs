using DataAccess.Entities;

namespace BusinessLogic.ViewModels.Resource
{
    public class ResourceCreateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public MoneyAmount Ammortization { get; set; }
    }
}
