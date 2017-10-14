using System.Linq;
using System.Threading.Tasks;
using Kleu.Utility.Identity.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity.Services
{
    public sealed class ApplicationRoleManager : RoleManager<IdentityRole>, IApplicationRoleManager
    {
        public ApplicationRoleManager(IApplicationRoleStore store) : base((ApplicationRoleStore)store)
        {
        }

        public override Task<IdentityResult> CreateAsync(IdentityRole role)
        {
            return base.CreateAsync(role);
        }

        public override Task<IdentityResult> DeleteAsync(IdentityRole role)
        {
            return base.DeleteAsync(role);
        }

        public override Task<IdentityResult> UpdateAsync(IdentityRole role)
        {
            return base.UpdateAsync(role);
        }

        public override Task<IdentityRole> FindByIdAsync(string roleId)
        {
            return base.FindByIdAsync(roleId);
        }

        public override Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return base.FindByNameAsync(roleName);
        }

        public override Task<bool> RoleExistsAsync(string roleName)
        {
            return base.RoleExistsAsync(roleName);
        }

        public override IQueryable<IdentityRole> Roles => base.Roles;
    }
}