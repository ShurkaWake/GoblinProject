using BusinessLogic.ViewModels.MoneyAmount;

namespace BusinessLogic.ViewModels.Resource
{
    public class ResourceUpdateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public MoneyAmountCreateModel Ammortization { get; set; }
    }
}
