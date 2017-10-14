using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity.Repositories
{
    public interface ISecurityRepository
    {
        Task<IdentityUserClaim> AddClaim(IdentityUserClaim claim);
        Task<Client> AddClient(Client client);
        Task<ClientSecret> AddClientSecret(ClientSecret secret);
        Task<ClientScope> AddClientScope(ClientScope scope);
        Task<Scope> AddScope(Scope scope);
        Task<ApplicationUser> AddUser(ApplicationUser user);
        Task<ClientCorsOrigin> AddOrigin(ClientCorsOrigin origin, string clientId);
        Task<ClientRedirectUri> AddRedirectUri(ClientRedirectUri redirectUri, string clientId);
        Task<ClientPostLogoutRedirectUri> AddPostLogoutRedirectUri(ClientPostLogoutRedirectUri postLogoutRedirectUri, string clientId);
        Task<ClientClaim> AddClientClaim(ClientClaim clientClaim);
        Task<ScopeClaim> AddScopeClaim(ScopeClaim scopeClaim, string scopeName);

        Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}