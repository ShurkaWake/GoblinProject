using DataAccess.Entities;

namespace BusinessLogic.ViewModels.Resource
{
    public class ResourceUpdateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public MoneyAmount Ammortization { get; set; }
    }
}
