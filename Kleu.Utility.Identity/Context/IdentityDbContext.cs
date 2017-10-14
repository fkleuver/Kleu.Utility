using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework;
using IdentityServer3.EntityFramework.Entities;
using Kleu.Utility.Common;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kleu.Utility.Identity.Context
{
    
    public class IdentityDbContext :
        IdentityDbContext<ApplicationUser>, 
        IOperationalDbContext, 
        IClientConfigurationDbContext, 
        IScopeConfigurationDbContext, 
        IIdentityDbContext
    {
        public Guid Id { get; } = GuidGenerator.GenerateTimeBasedGuid();
        private readonly bool _avoidDisposeForTesting;

        private void Configure()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public IdentityDbContext() : base(nameof(IdentityDbContext))
        {
            Configure();
        }

        public IdentityDbContext(string connectionString) : base(connectionString)
        {
            Configure();
        }

        public IdentityDbContext(string connectionString, DbCompiledModel model) : base(connectionString, model)
        {
            Configure();
        }

        public IdentityDbContext(DbConnection connection, bool avoidDisposeForTesting) : base(connection, true)
        {
            _avoidDisposeForTesting = avoidDisposeForTesting;
        }

        public new void Dispose()
        {
            if (!_avoidDisposeForTesting)
            {
                base.Dispose();
            }
        }
        public DbSet<ClientClaim> ClientClaims { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<ClientCustomGrantType> ClientCustomGrantTypes { get; set; }
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ClientSecret> ClientSecrets { get; set; }
        public DbSet<Consent> Consents { get; set; }
        public DbSet<ScopeClaim> ScopeClaims { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<ScopeSecret> ScopeSecrets { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override IDbSet<IdentityRole> Roles
        {
            get => base.Roles;
            set => base.Roles = value;
        }

        public override IDbSet<ApplicationUser> Users
        {
            get => base.Users;
            set => base.Users = value;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override DbSet Set(Type entityType)
        {
            return base.Set(entityType);
        }
        
        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            return base.ShouldValidateEntity(entityEntry);
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
