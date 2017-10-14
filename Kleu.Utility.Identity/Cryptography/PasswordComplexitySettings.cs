namespace Kleu.Utility.Identity.Cryptography
{
    public class PasswordComplexitySettings
    {
        public int MinUpperCase { get; }
        public int MinNonAlphaNumeric { get; }
        public int MinNumeric { get; }
        public int MinLength { get; }
        public int MaxLength { get; }

        public PasswordComplexitySettings(
            int minUpperCase,
            int minNonAlphaNumeric,
            int minNumeric,
            int minLength,
            int maxLength)
        {
            MinUpperCase = minUpperCase;
            MinNonAlphaNumeric = minNonAlphaNumeric;
            MinNumeric = minNumeric;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public PasswordComplexitySettings() : this(1, 1, 1, 6, int.MaxValue)
        {
        }
    }
}