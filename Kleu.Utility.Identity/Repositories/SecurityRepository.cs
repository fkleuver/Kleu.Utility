using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework.Entities;
using Kleu.Utility.Identity.Context;
using Kleu.Utility.Logging;
using Microsoft.AspNet.Identity.EntityFramework;
using Client = IdentityServer3.EntityFramework.Entities.Client;
using Scope = IdentityServer3.EntityFramework.Entities.Scope;
using ScopeClaim = IdentityServer3.EntityFramework.Entities.ScopeClaim;

namespace Kleu.Utility.Identity.Repositories
{
    public sealed class SecurityRepository : ISecurityRepository
    {
        private readonly ILog _logger;
        private readonly IIdentityDbContext _context;

        public SecurityRepository(ILog logger, IIdentityDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IdentityUserClaim> AddClaim(IdentityUserClaim claim)
        {
            var user = await _context.Users.SingleAsync(u => u.Id == claim.UserId);
            user.Claims.Add(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task<Client> AddClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<ClientSecret> AddClientSecret(ClientSecret secret)
        {
            _context.ClientSecrets.Add(secret);
            await _context.SaveChangesAsync();
            return secret;
        }

        public async Task<ClientScope> AddClientScope(ClientScope scope)
        {
            _context.ClientScopes.Add(scope);
            await _context.SaveChangesAsync();
            return scope;
        }

        public async Task<Scope> AddScope(Scope scope)
        {
            _context.Scopes.Add(scope);
            await _context.SaveChangesAsync();
            return scope;
        }

        public async Task<ApplicationUser> AddUser(ApplicationUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<ClientCorsOrigin> AddOrigin(ClientCorsOrigin origin, string clientId)
        {
            var client = await _context.Clients.SingleAsync(c => c.ClientId == clientId);
            origin.Client = client;
            _context.ClientCorsOrigins.Add(origin);
            await _context.SaveChangesAsync();
            return origin;
        }

        public async Task<ClientRedirectUri> AddRedirectUri(ClientRedirectUri redirectUri, string clientId)
        {
            var client = await _context.Clients.SingleAsync(c => c.ClientId == clientId);
            redirectUri.Client = client;
            _context.ClientRedirectUris.Add(redirectUri);
            await _context.SaveChangesAsync();
            return redirectUri;
        }

        public async Task<ClientPostLogoutRedirectUri> AddPostLogoutRedirectUri(ClientPostLogoutRedirectUri postLogoutRedirectUri, string clientId)
        {
            var client = await _context.Clients.SingleAsync(c => c.ClientId == clientId);
            postLogoutRedirectUri.Client = client;
            _context.ClientPostLogoutRedirectUris.Add(postLogoutRedirectUri);
            await _context.SaveChangesAsync();
            return postLogoutRedirectUri;
        }

        public async Task<ClientClaim> AddClientClaim(ClientClaim clientClaim)
        {
            _context.ClientClaims.Add(clientClaim);
            await _context.SaveChangesAsync();
            return clientClaim;
        }

        public async Task<ScopeClaim> AddScopeClaim(ScopeClaim scopeClaim, string scopeName)
        {
            var scope = await _context.Scopes.SingleAsync(s => s.Name == scopeName);
            scopeClaim.Scope = scope;
            await _context.SaveChangesAsync();
            return scopeClaim;
        }

        public async Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }
    }
}
