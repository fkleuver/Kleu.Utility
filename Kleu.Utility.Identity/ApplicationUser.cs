using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdministrator { get; set; }

    }
}