using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Kleu.Utility.Identity.Services
{
    public interface IApplicationUserService
    {
        Task AuthenticateExternalAsync(ExternalAuthenticationContext ctx);
        Task AuthenticateLocalAsync(LocalAuthenticationContext ctx);
        Task GetProfileDataAsync(ProfileDataRequestContext ctx);
        Task IsActiveAsync(IsActiveContext ctx);
        Task PostAuthenticateAsync(PostAuthenticationContext context);
        Task PreAuthenticateAsync(PreAuthenticationContext context);
        Task SignOutAsync(SignOutContext context);
    }
}