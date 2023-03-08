using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsShoppingCart.Models
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NoMembers { get; set; }
        public string RoleName { get; set; }

        [NotMapped]
        public string[] AddIds { get; set; }

        [NotMapped]
        public string[] DeleteIds { get; set; }

    }
}
