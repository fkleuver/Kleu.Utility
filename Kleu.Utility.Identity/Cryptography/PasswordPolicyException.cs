using System;

namespace Kleu.Utility.Identity.Cryptography
{
    [Serializable]
    public class PasswordPolicyException : Exception
    {
        public PasswordPolicyException(string message) : base(message)
        {

        }
    }
}