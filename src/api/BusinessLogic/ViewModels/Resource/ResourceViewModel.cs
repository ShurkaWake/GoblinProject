using BusinessLogic.ViewModels.MoneyAmount;
using DataAccess.Enums;

namespace BusinessLogic.ViewModels.Resource
{
    public class ResourceViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MoneyAmountViewModel Ammortization { get; set; }

        public string Status { get; set; }
    }
}
