using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.AspNetIdentity;
using IdentityServer3.Core.Models;

namespace Kleu.Utility.Identity.Services
{
    public sealed class ApplicationUserService : AspNetIdentityUserService<ApplicationUser, string>, IApplicationUserService
    {
        public ApplicationUserService(IApplicationUserManager userManager) : base((ApplicationUserManager)userManager)
        {
        }

        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext ctx)
        {
            return base.AuthenticateExternalAsync(ctx);
        }

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext ctx)
        {
            return base.AuthenticateLocalAsync(ctx);
        }

        protected override Task<ApplicationUser> FindUserAsync(string username)
        {
            return base.FindUserAsync(username);
        }

        protected override Task<IEnumerable<Claim>> GetClaimsForAuthenticateResult(ApplicationUser user)
        {
            return base.GetClaimsForAuthenticateResult(user);
        }

        protected override Task<IEnumerable<Claim>> GetClaimsFromAccount(ApplicationUser user)
        {
            return base.GetClaimsFromAccount(user);
        }

        protected override Task<string> GetDisplayNameForAccountAsync(string userID)
        {
            return base.GetDisplayNameForAccountAsync(userID);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext ctx)
        {
            return base.GetProfileDataAsync(ctx);
        }

        protected override Task<ApplicationUser> InstantiateNewUserFromExternalProviderAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            return base.InstantiateNewUserFromExternalProviderAsync(provider, providerId, claims);
        }

        public override Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            return base.PostAuthenticateAsync(context);
        }

        protected override Task<AuthenticateResult> PostAuthenticateLocalAsync(ApplicationUser user, SignInMessage message)
        {
            return base.PostAuthenticateLocalAsync(user, message);
        }

        public override Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            return base.PreAuthenticateAsync(context);
        }

        protected override Task<AuthenticateResult> ProcessExistingExternalAccountAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return base.ProcessExistingExternalAccountAsync(userID, provider, providerId, claims);
        }

        protected override Task<AuthenticateResult> ProcessNewExternalAccountAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            return base.ProcessNewExternalAccountAsync(provider, providerId, claims);
        }

        protected override Task<IEnumerable<Claim>> SetAccountEmailAsync(string userID, IEnumerable<Claim> claims)
        {
            return base.SetAccountEmailAsync(userID, claims);
        }

        protected override Task<IEnumerable<Claim>> SetAccountPhoneAsync(string userID, IEnumerable<Claim> claims)
        {
            return base.SetAccountPhoneAsync(userID, claims);
        }

        public override Task IsActiveAsync(IsActiveContext ctx)
        {
            return base.IsActiveAsync(ctx);
        }

        protected override Task<AuthenticateResult> SignInFromExternalProviderAsync(string userID, string provider)
        {
            return base.SignInFromExternalProviderAsync(userID, provider);
        }

        public override Task SignOutAsync(SignOutContext context)
        {
            return base.SignOutAsync(context);
        }

        protected override Task<ApplicationUser> TryGetExistingUserFromExternalProviderClaimsAsync(string provider, IEnumerable<Claim> claims)
        {
            return base.TryGetExistingUserFromExternalProviderClaimsAsync(provider, claims);
        }

        protected override Task<AuthenticateResult> AccountCreatedFromExternalProviderAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return base.AccountCreatedFromExternalProviderAsync(userID, provider, providerId, claims);
        }

        protected override Task<AuthenticateResult> UpdateAccountFromExternalClaimsAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return base.UpdateAccountFromExternalClaimsAsync(userID, provider, providerId, claims);
        }
    }
}
