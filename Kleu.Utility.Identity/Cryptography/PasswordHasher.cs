using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Kleu.Utility.Identity.Cryptography
{
    public class PasswordHasher
    {
        private const string NumbersRegex = "[\\d]";
        private const string UppercaseRegex = "[A-Z]";
        private const string NonAlphaNumericRegex = "[^0-9a-zA-Z]";

        private readonly PasswordComplexitySettings _settings;
        public static int Iterations => 5000;
        public static int SaltLength => 64;
        public static int HashLength => 64;
        public static int MaxPasswordLength => 1024;

        public static char Delimiter => ':';
        public static int SaltIndex => 0;
        public static int HashIndex => 1;

        public static PasswordHasher WithDefaultSettings => new PasswordHasher(new PasswordComplexitySettings());

        public PasswordHasher(PasswordComplexitySettings settings)
        {
            _settings = settings;
        }

        public string CalculateHash(string plainTextPassword)
        {
            byte[] saltBytes;
            byte[] hashBytes;

            PasswordPolicyException error = null;
            var passwordMeetsPolicy = TryPasswordPolicyCompliance(plainTextPassword, _settings, ref error);
            if (passwordMeetsPolicy)
            {
                saltBytes = GenerateRandomSalt(SaltLength);

                hashBytes = PasswordToHash(saltBytes, plainTextPassword);
            }
            else
            {
                throw error;
            }

            var salt = HashBytesToHexString(saltBytes);
            var hash = HashBytesToHexString(hashBytes);

            var saltAndHash = $"{salt}{Delimiter}{hash}";
            return saltAndHash;
        }

        public static bool ComparePasswordToHash(byte[] salt, string password, byte[] hash)
        {
            return PasswordToHash(salt, StringToUtf8Bytes(password)).SequenceEqual(hash);
        }

        public static byte[] PasswordToHash(byte[] salt, string password)
        {
            var convertedPassword = StringToUtf8Bytes(password);

            CheckPasswordSizeCompliance(convertedPassword);

            return PasswordToHash(salt, convertedPassword);
        }

        public static byte[] StringToUtf8Bytes(string stringToConvert)
        {
            return new UTF8Encoding(false).GetBytes(stringToConvert);
        }

        public static string HashBytesToHexString(byte[] hash)
        {
            return new SoapHexBinary(hash).ToString();
        }

        public static byte[] HashHexStringToBytes(string hashHexString)
        {
            return SoapHexBinary.Parse(hashHexString).Value;
        }

        public static bool CompareHash(string plainTextPassword, string hashedPassword)
        {
            var saltAndHash = hashedPassword.Split(Delimiter);
            var salt = saltAndHash[SaltIndex];
            var hash = saltAndHash[HashIndex];

            var saltBytes = HashHexStringToBytes(salt);
            var hashBytes = HashHexStringToBytes(hash);

            return ComparePasswordToHash(saltBytes, plainTextPassword, hashBytes);
        }

        public static bool TryPasswordPolicyCompliance(string password, PasswordComplexitySettings settings, ref PasswordPolicyException pwdPolicyException)
        {
            try
            {
                CheckPasswordPolicyCompliance(password, settings);
                return true;
            }
            catch (PasswordPolicyException ex)
            {
                pwdPolicyException = ex;
            }

            return false;
        }

        private static byte[] GenerateRandomSalt(int saltLength)
        {
            var salt = new byte[saltLength];
            RandomNumberGenerator.Create().GetBytes(salt);
            return salt;
        }

        private static void CheckPasswordSizeCompliance(byte[] password)
        {
            if (password.Length > MaxPasswordLength)
            {
                throw new PasswordTooLongException("The supplied password is longer than allowed, it must be smaller than " + MaxPasswordLength + " bytes long as defined by CMaxPasswordLength");
            }
        }

        private static void CheckPasswordPolicyCompliance(string password, PasswordComplexitySettings settings)
        {
            if (new Regex(NumbersRegex).Matches(password).Count < settings.MinNumeric)
            {
                throw new PasswordPolicyException("The password must contain " + settings.MinNumeric + " numeric [0-9] characters");
            }

            if (new Regex(NonAlphaNumericRegex).Matches(password).Count < settings.MinNonAlphaNumeric)
            {
                throw new PasswordPolicyException("The password must contain " + settings.MinNonAlphaNumeric + " special characters");
            }

            if (new Regex(UppercaseRegex).Matches(password).Count < settings.MinUpperCase)
            {
                throw new PasswordPolicyException("The password must contain " + settings.MinUpperCase + " uppercase characters");
            }

            if (password.Length < settings.MinLength)
            {
                throw new PasswordPolicyException("The password does not have a length of at least " + settings.MinLength + " characters");
            }

            if (password.Length > settings.MaxLength)
            {
                throw new PasswordPolicyException("The password is longer than " + settings.MaxLength + " characters");
            }
        }

        private static byte[] PasswordToHash(byte[] salt, byte[] password)
        {
            return new Rfc2898(password, salt, Iterations).GetDerivedKeyBytes_PBKDF2_HMACSHA512(HashLength);
        }
    }
}
