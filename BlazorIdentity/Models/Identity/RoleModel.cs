using Microsoft.AspNetCore.Identity;

namespace BlazorIdentity.Models.Identity
{
    public class RoleDetails
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }

    public class RoleEditModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsAdd { get; set; }
        public string[] IdsDelete { get; set; }

    }
}
