using BusinessLogic.Abstractions;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace BusinessLogic.Filtering
{
    public class ResourceFilter : CustomFilterBase
    {
        [FromQuery(Name = "id.eq")]
        public int Id { get; set; }

        [FromQuery(Name = "name.stw")]
        public string Name { get; set; }

        [FromQuery(Name = "description.stw")]
        public string Description { get; set; }

        [FromQuery(Name = "status.eq")]
        public ResourceStatus Status { get; set; }
    }
}
