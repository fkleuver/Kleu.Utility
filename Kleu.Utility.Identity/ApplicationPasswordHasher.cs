using System;
using Kleu.Utility.Identity.Cryptography;
using Microsoft.AspNet.Identity;
using PasswordHasher = Kleu.Utility.Identity.Cryptography.PasswordHasher;

namespace Kleu.Utility.Identity
{
    public sealed class ApplicationPasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher _hasher;

        public ApplicationPasswordHasher()
        {
           // _hasher = Common.Core.Security.PasswordHasher.WithDefaultSettings;
           _hasher = new PasswordHasher(new PasswordComplexitySettings(0, 0, 0, 0, Int32.MaxValue));
        }

        public string HashPassword(string password)
        {
            return _hasher.CalculateHash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var success = PasswordHasher.CompareHash(providedPassword, hashedPassword);
            if (success)
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}