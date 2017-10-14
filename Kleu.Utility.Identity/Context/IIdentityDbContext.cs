using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework.Entities;
using Kleu.Utility.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity.Context
{
    public interface IIdentityDbContext : IDbContext
    {
        DbSet<ClientClaim> ClientClaims { get; set; }
        DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        DbSet<ClientCustomGrantType> ClientCustomGrantTypes { get; set; }
        DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<ClientScope> ClientScopes { get; set; }
        DbSet<ClientSecret> ClientSecrets { get; set; }
        DbSet<Consent> Consents { get; set; }
        IDbSet<IdentityRole> Roles { get; set; }
        DbSet<ScopeClaim> ScopeClaims { get; set; }
        DbSet<Scope> Scopes { get; set; }
        DbSet<ScopeSecret> ScopeSecrets { get; set; }
        DbSet<Token> Tokens { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}