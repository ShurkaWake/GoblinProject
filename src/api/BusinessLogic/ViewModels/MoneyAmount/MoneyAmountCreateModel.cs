using DataAccess.Enums;

namespace BusinessLogic.ViewModels.MoneyAmount
{
    public class MoneyAmountCreateModel
    {
        public decimal Amount { get; set; }
        
        public Currency Currency { get; set; }
    }
}
