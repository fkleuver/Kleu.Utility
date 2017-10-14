using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kleu.Utility.Identity.Repositories;
using Microsoft.AspNet.Identity;

namespace Kleu.Utility.Identity.Services
{
    public sealed class ApplicationUserManager : UserManager<ApplicationUser, string>, IApplicationUserManager
    {
        public ApplicationUserManager(IApplicationUserStore store, IPasswordHasher hasher) : base((ApplicationUserStore)store)
        {
            PasswordHasher = hasher;

        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return base.CreateAsync(user, password);
        }

        public override Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            return base.DeleteAsync(user);
        }

        public override Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return base.FindByIdAsync(userId);
        }

        public override Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return base.FindByNameAsync(userName);
        }

        public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return base.UpdateAsync(user);
        }

        public override Task<IdentityResult> AccessFailedAsync(string userId)
        {
            return base.AccessFailedAsync(userId);
        }

        public override Task<IdentityResult> AddClaimAsync(string userId, Claim claim)
        {
            return base.AddClaimAsync(userId, claim);
        }

        public override Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            return base.AddLoginAsync(userId, login);
        }

        public override Task<IdentityResult> AddPasswordAsync(string userId, string password)
        {
            return base.AddPasswordAsync(userId, password);
        }

        public override Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            return base.AddToRoleAsync(userId, role);
        }

        public override Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles)
        {
            return base.AddToRolesAsync(userId, roles);
        }

        public override Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public override Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token)
        {
            return base.ChangePhoneNumberAsync(userId, phoneNumber, token);
        }

        public override Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return base.CheckPasswordAsync(user, password);
        }

        public override Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            return base.ConfirmEmailAsync(userId, token);
        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        {
            return base.CreateIdentityAsync(user, authenticationType);
        }

        public override Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return base.FindAsync(login);
        }

        public override Task<ApplicationUser> FindAsync(string userName, string password)
        {
            return base.FindAsync(userName, password);
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return base.FindByEmailAsync(email);
        }

        public override Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber)
        {
            return base.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            return base.GenerateEmailConfirmationTokenAsync(userId);
        }

        public override Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            return base.GeneratePasswordResetTokenAsync(userId);
        }

        public override Task<string> GenerateTwoFactorTokenAsync(string userId, string twoFactorProvider)
        {
            return base.GenerateTwoFactorTokenAsync(userId, twoFactorProvider);
        }

        public override Task<string> GenerateUserTokenAsync(string purpose, string userId)
        {
            return base.GenerateUserTokenAsync(purpose, userId);
        }

        public override Task<int> GetAccessFailedCountAsync(string userId)
        {
            return base.GetAccessFailedCountAsync(userId);
        }

        public override Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            return base.GetClaimsAsync(userId);
        }

        public override Task<string> GetEmailAsync(string userId)
        {
            return base.GetEmailAsync(userId);
        }

        public override Task<bool> GetLockoutEnabledAsync(string userId)
        {
            return base.GetLockoutEnabledAsync(userId);
        }

        public override Task<DateTimeOffset> GetLockoutEndDateAsync(string userId)
        {
            return base.GetLockoutEndDateAsync(userId);
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            return base.GetLoginsAsync(userId);
        }

        public override Task<string> GetPhoneNumberAsync(string userId)
        {
            return base.GetPhoneNumberAsync(userId);
        }

        public override Task<IList<string>> GetRolesAsync(string userId)
        {
            return base.GetRolesAsync(userId);
        }

        public override Task<string> GetSecurityStampAsync(string userId)
        {
            return base.GetSecurityStampAsync(userId);
        }

        public override Task<bool> GetTwoFactorEnabledAsync(string userId)
        {
            return base.GetTwoFactorEnabledAsync(userId);
        }

        public override Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId)
        {
            return base.GetValidTwoFactorProvidersAsync(userId);
        }

        public override Task<bool> HasPasswordAsync(string userId)
        {
            return base.HasPasswordAsync(userId);
        }

        public override Task<bool> IsEmailConfirmedAsync(string userId)
        {
            return base.IsEmailConfirmedAsync(userId);
        }

        public override Task<bool> IsInRoleAsync(string userId, string role)
        {
            return base.IsInRoleAsync(userId, role);
        }

        public override Task<bool> IsLockedOutAsync(string userId)
        {
            return base.IsLockedOutAsync(userId);
        }

        public override Task<bool> IsPhoneNumberConfirmedAsync(string userId)
        {
            return base.IsPhoneNumberConfirmedAsync(userId);
        }

        public override Task<IdentityResult> NotifyTwoFactorTokenAsync(string userId, string twoFactorProvider, string token)
        {
            return base.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }

        public override void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<ApplicationUser, string> provider)
        {
            base.RegisterTwoFactorProvider(twoFactorProvider, provider);
        }

        public override Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim)
        {
            return base.RemoveClaimAsync(userId, claim);
        }

        public override Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            return base.RemoveFromRoleAsync(userId, role);
        }

        public override Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles)
        {
            return base.RemoveFromRolesAsync(userId, roles);
        }

        public override Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login)
        {
            return base.RemoveLoginAsync(userId, login);
        }

        public override Task<IdentityResult> RemovePasswordAsync(string userId)
        {
            return base.RemovePasswordAsync(userId);
        }

        public override Task<IdentityResult> ResetAccessFailedCountAsync(string userId)
        {
            return base.ResetAccessFailedCountAsync(userId);
        }

        public override Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            return base.ResetPasswordAsync(userId, token, newPassword);
        }

        public override Task SendEmailAsync(string userId, string subject, string body)
        {
            return base.SendEmailAsync(userId, subject, body);
        }

        public override Task SendSmsAsync(string userId, string message)
        {
            return base.SendSmsAsync(userId, message);
        }

        public override Task<IdentityResult> SetEmailAsync(string userId, string email)
        {
            return base.SetEmailAsync(userId, email);
        }

        public override Task<IdentityResult> SetLockoutEnabledAsync(string userId, bool enabled)
        {
            return base.SetLockoutEnabledAsync(userId, enabled);
        }

        public override Task<IdentityResult> SetLockoutEndDateAsync(string userId, DateTimeOffset lockoutEnd)
        {
            return base.SetLockoutEndDateAsync(userId, lockoutEnd);
        }

        public override Task<IdentityResult> SetPhoneNumberAsync(string userId, string phoneNumber)
        {
            return base.SetPhoneNumberAsync(userId, phoneNumber);
        }

        public override Task<IdentityResult> SetTwoFactorEnabledAsync(string userId, bool enabled)
        {
            return base.SetTwoFactorEnabledAsync(userId, enabled);
        }

        public override bool SupportsQueryableUsers => base.SupportsQueryableUsers;

        public override bool SupportsUserClaim => base.SupportsUserClaim;

        public override bool SupportsUserEmail => base.SupportsUserEmail;

        public override bool SupportsUserLockout => base.SupportsUserLockout;

        public override bool SupportsUserLogin => base.SupportsUserLogin;

        public override bool SupportsUserPassword => base.SupportsUserPassword;

        public override bool SupportsUserPhoneNumber => base.SupportsUserPhoneNumber;

        public override bool SupportsUserRole => base.SupportsUserRole;

        public override bool SupportsUserSecurityStamp => base.SupportsUserSecurityStamp;

        public override bool SupportsUserTwoFactor => base.SupportsUserTwoFactor;

        public override IQueryable<ApplicationUser> Users => base.Users;

        protected override Task<IdentityResult> UpdatePassword(IUserPasswordStore<ApplicationUser, string> passwordStore, ApplicationUser user, string newPassword)
        {
            return base.UpdatePassword(passwordStore, user, newPassword);
        }

        public override Task<bool> VerifyChangePhoneNumberTokenAsync(string userId, string token, string phoneNumber)
        {
            return base.VerifyChangePhoneNumberTokenAsync(userId, token, phoneNumber);
        }

        public override Task<IdentityResult> UpdateSecurityStampAsync(string userId)
        {
            return base.UpdateSecurityStampAsync(userId);
        }

        protected override Task<bool> VerifyPasswordAsync(IUserPasswordStore<ApplicationUser, string> store, ApplicationUser user, string password)
        {
            return base.VerifyPasswordAsync(store, user, password);
        }

        public override Task<bool> VerifyTwoFactorTokenAsync(string userId, string twoFactorProvider, string token)
        {
            return base.VerifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }

        public override Task<bool> VerifyUserTokenAsync(string userId, string purpose, string token)
        {
            return base.VerifyUserTokenAsync(userId, purpose, token);
        }
    }
}