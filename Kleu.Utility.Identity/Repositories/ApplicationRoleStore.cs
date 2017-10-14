using System.Threading.Tasks;
using Kleu.Utility.Identity.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityDbContext = Kleu.Utility.Identity.Context.IdentityDbContext;

namespace Kleu.Utility.Identity.Repositories
{
    public sealed class ApplicationRoleStore : RoleStore<IdentityRole>, IApplicationRoleStore
    {
        public ApplicationRoleStore(IIdentityDbContext context) : base((IdentityDbContext)context)
        {
            
        }

        public override Task UpdateAsync(IdentityRole role)
        {
            return base.UpdateAsync(role);
        }

        public override Task CreateAsync(IdentityRole role)
        {
            return base.CreateAsync(role);
        }

        public override Task DeleteAsync(IdentityRole role)
        {
            return base.DeleteAsync(role);
        }
    }
}