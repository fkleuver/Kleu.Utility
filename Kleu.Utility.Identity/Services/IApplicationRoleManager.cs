using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity.Services
{
    public interface IApplicationRoleManager
    {
        IQueryable<IdentityRole> Roles { get; }

        Task<IdentityResult> CreateAsync(IdentityRole role);
        Task<IdentityResult> DeleteAsync(IdentityRole role);
        Task<IdentityRole> FindByIdAsync(string roleId);
        Task<IdentityRole> FindByNameAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> UpdateAsync(IdentityRole role);
    }
}