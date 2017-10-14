using System;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.EntityFramework;
using Kleu.Utility.Identity.Context;
using Kleu.Utility.Identity.Repositories;
using Kleu.Utility.Identity.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityDbContext = Kleu.Utility.Identity.Context.IdentityDbContext;

namespace Kleu.Utility.Identity.Configuration
{
    public class DefaultIdentityServerFactory : IdentityServerServiceFactory
    {
        public DefaultIdentityServerFactory()
        {
            var iIdentityDbRegistration = new Registration<IdentityDbContext>(resolver => (IdentityDbContext)resolver.Resolve<IIdentityDbContext>());
            var IdentityDbRegistration = new Registration<IdentityDbContext<ApplicationUser>>(resolver => resolver.Resolve<IdentityDbContext>());
            var operationalDbContextRegistration = new Registration<IOperationalDbContext>(resolver => resolver.Resolve<IdentityDbContext>());
            var clientConfigurationDbContextRegistration = new Registration<IClientConfigurationDbContext>(resolver => resolver.Resolve<IdentityDbContext>());
            var scopeConfigurationDbContextRegistration = new Registration<IScopeConfigurationDbContext>(resolver => resolver.Resolve<IdentityDbContext>());

            Register(iIdentityDbRegistration);
            Register(IdentityDbRegistration);
            Register(operationalDbContextRegistration);
            Register(clientConfigurationDbContextRegistration);
            Register(scopeConfigurationDbContextRegistration);

            AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore>();
            TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore>();
            ConsentStore = new Registration<IConsentStore, ConsentStore>();
            RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore>();

            ClientStore = new Registration<IClientStore, ClientStore>();
            var clientConfigurationCorsPolicyRegistration = new Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService>();
            clientConfigurationCorsPolicyRegistration.AdditionalRegistrations.Add(clientConfigurationDbContextRegistration);
            CorsPolicyService = clientConfigurationCorsPolicyRegistration;

            ScopeStore = new Registration<IScopeStore, ScopeStore>();

            Register(new Registration<IPasswordHasher, ApplicationPasswordHasher>());
            Register(new Registration<IApplicationUserStore, ApplicationUserStore>());
            Register(new Registration<IApplicationUserManager, ApplicationUserManager>());
            UserService = new Registration<IUserService, ApplicationUserService>();

            this.ConfigureClientStoreCache();
            this.ConfigureScopeStoreCache();
            this.ConfigureUserServiceCache(TimeSpan.FromMinutes(1));
        }
    }
}
