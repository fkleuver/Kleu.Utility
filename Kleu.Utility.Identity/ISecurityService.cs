using System.Threading.Tasks;

namespace Kleu.Utility.Identity
{
    public interface ISecurityService
    {
        Task<bool> CreateHybridClientIfNotExists(string id, string name, string secret);
        Task<bool> CreateResourceOwnerClientIfNotExists(string id, string name, string secret, params string[] permissions);

        Task EnsureScopesPresent(params string[] scopeNames);

        Task<bool> CreateUserIfNotExists(string username, string password, params string[] permissions);
        Task<bool> CreateAdminUserIfNotExists(string username, string password);
        Task<bool> UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds);

        Task AllowCorsOriginIfNotYetAllowed(string clientId, string origin);
        Task AddRedirectUrisIfNotExists(string clientId, string postLogoutRedirectUri, params string[] redirectUris);

        Task AddNameClaimToScopeIfNotExists(string scope);
    }
}
