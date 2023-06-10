using BusinessLogic.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogic.Filtering
{
    public class BusinessFilter : CustomFilterBase
    {
        [FromQuery(Name = "id.eq")]
        public int Id { get; set; }

        [FromQuery(Name = "name.stw")]
        public string Name { get; set; }

        [FromQuery(Name = "location.stw")]
        public string Location { get; set; }
    }
}
