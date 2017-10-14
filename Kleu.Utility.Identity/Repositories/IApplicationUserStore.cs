using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Kleu.Utility.Identity.Repositories
{
    public interface IApplicationUserStore
    {
        Task AddClaimAsync(ApplicationUser user, Claim claim);
        Task AddLoginAsync(ApplicationUser user, UserLoginInfo login);
        Task AddToRoleAsync(ApplicationUser user, string roleName);
        Task CreateAsync(ApplicationUser user);
        Task DeleteAsync(ApplicationUser user);
        Task<ApplicationUser> FindAsync(UserLoginInfo login);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<int> GetAccessFailedCountAsync(ApplicationUser user);
        Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<string> GetEmailAsync(ApplicationUser user);
        Task<bool> GetEmailConfirmedAsync(ApplicationUser user);
        Task<bool> GetLockoutEnabledAsync(ApplicationUser user);
        Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user);
        Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user);
        Task<string> GetPasswordHashAsync(ApplicationUser user);
        Task<string> GetPhoneNumberAsync(ApplicationUser user);
        Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<string> GetSecurityStampAsync(ApplicationUser user);
        Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user);
        Task<bool> HasPasswordAsync(ApplicationUser user);
        Task<int> IncrementAccessFailedCountAsync(ApplicationUser user);
        Task<bool> IsInRoleAsync(ApplicationUser user, string roleName);
        Task RemoveClaimAsync(ApplicationUser user, Claim claim);
        Task RemoveFromRoleAsync(ApplicationUser user, string roleName);
        Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login);
        Task ResetAccessFailedCountAsync(ApplicationUser user);
        Task SetEmailAsync(ApplicationUser user, string email);
        Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed);
        Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled);
        Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd);
        Task SetPasswordHashAsync(ApplicationUser user, string passwordHash);
        Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber);
        Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed);
        Task SetSecurityStampAsync(ApplicationUser user, string stamp);
        Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled);
        Task UpdateAsync(ApplicationUser user);
    }
}