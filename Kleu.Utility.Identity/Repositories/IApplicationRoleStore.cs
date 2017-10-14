using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity.Repositories
{
    public interface IApplicationRoleStore
    {
        Task CreateAsync(IdentityRole role);
        Task DeleteAsync(IdentityRole role);
        Task UpdateAsync(IdentityRole role);
    }
}