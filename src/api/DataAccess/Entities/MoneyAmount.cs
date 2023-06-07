using DataAccess.Abstractions;
using DataAccess.Enums;

namespace DataAccess.Entities
{
    public class MoneyAmount : IEntity<int>
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }
    }
}
