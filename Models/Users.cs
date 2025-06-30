using Microsoft.AspNetCore.Identity;

namespace ProjectRempakTani.Models
{
    public class Users : IdentityUser
    {
        public string FullName{ get; set; }
    }
}
