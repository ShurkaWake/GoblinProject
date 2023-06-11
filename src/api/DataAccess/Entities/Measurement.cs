using DataAccess.Abstractions;

namespace DataAccess.Entities
{
    public class Measurement : IEntity<int>
    {
        public int Id { get; set; }

        public Scales Scales { get; set; }

        /// <summary>
        /// Weight of gold in kilos
        /// </summary>
        public decimal Weight { get; set; }

        public DateTime DateTime { get; set; }
    }
}
