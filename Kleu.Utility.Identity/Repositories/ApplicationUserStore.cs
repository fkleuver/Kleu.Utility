using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Kleu.Utility.Identity.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityDbContext = Kleu.Utility.Identity.Context.IdentityDbContext;

namespace Kleu.Utility.Identity.Repositories
{
    public sealed class ApplicationUserStore : UserStore<ApplicationUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IApplicationUserStore
    {
        public ApplicationUserStore(IIdentityDbContext context) : base((IdentityDbContext)context)
        {

        }

        public override Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            return base.AddClaimAsync(user, claim);
        }

        public override Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return base.AddLoginAsync(user, login);
        }

        public override Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return base.AddToRoleAsync(user, roleName);
        }

        public override Task CreateAsync(ApplicationUser user)
        {
            return base.CreateAsync(user);
        }

        public override Task DeleteAsync(ApplicationUser user)
        {
            return base.DeleteAsync(user);
        }

        public override Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return base.FindAsync(login);
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return base.FindByEmailAsync(email);
        }

        public override Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return base.FindByIdAsync(userId);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return base.FindByNameAsync(userName);
        }

        public override Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return base.GetAccessFailedCountAsync(user);
        }

        public override Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return base.GetClaimsAsync(user);
        }

        public override Task<string> GetEmailAsync(ApplicationUser user)
        {
            return base.GetEmailAsync(user);
        }

        public override Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return base.GetEmailConfirmedAsync(user);
        }

        public override Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return base.GetLockoutEnabledAsync(user);
        }

        public override Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return base.GetLockoutEndDateAsync(user);
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return base.GetLoginsAsync(user);
        }

        public override Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return base.GetPasswordHashAsync(user);
        }

        public override Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return base.GetPhoneNumberAsync(user);
        }

        public override Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return base.GetPhoneNumberConfirmedAsync(user);
        }

        public override Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return base.GetRolesAsync(user);
        }

        public override Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return base.GetSecurityStampAsync(user);
        }

        public override Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return base.GetTwoFactorEnabledAsync(user);
        }

        protected override Task<ApplicationUser> GetUserAggregateAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            return base.GetUserAggregateAsync(filter);
        }

        public override Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return base.HasPasswordAsync(user);
        }

        public override Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return base.IncrementAccessFailedCountAsync(user);
        }

        public override Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return base.IsInRoleAsync(user, roleName);
        }

        public override Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            return base.RemoveClaimAsync(user, claim);
        }

        public override Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            return base.RemoveFromRoleAsync(user, roleName);
        }

        public override Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return base.RemoveLoginAsync(user, login);
        }

        public override Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return base.ResetAccessFailedCountAsync(user);
        }

        public override Task SetEmailAsync(ApplicationUser user, string email)
        {
            return base.SetEmailAsync(user, email);
        }

        public override Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return base.SetEmailConfirmedAsync(user, confirmed);
        }

        public override Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return base.SetLockoutEnabledAsync(user, enabled);
        }

        public override Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            return base.SetLockoutEndDateAsync(user, lockoutEnd);
        }

        public override Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return base.SetPasswordHashAsync(user, passwordHash);
        }

        public override Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            return base.SetPhoneNumberAsync(user, phoneNumber);
        }

        public override Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return base.SetPhoneNumberConfirmedAsync(user, confirmed);
        }

        public override Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            return base.SetSecurityStampAsync(user, stamp);
        }

        public override Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return base.SetTwoFactorEnabledAsync(user, enabled);
        }

        public override Task UpdateAsync(ApplicationUser user)
        {
            return base.UpdateAsync(user);
        }
    }
}
