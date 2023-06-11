using BusinessLogic.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace BusinessLogic.Filtering
{
    public class UserFilter : CustomFilterBase
    {
        [FromQuery(Name = "id.eq")]
        public string Id { get; set; }

        [FromQuery(Name = "email.stw")]
        public string Email { get; set; }

        [FromQuery(Name = "fullName.stw")]
        public string FullName { get; set; }
    }
}
