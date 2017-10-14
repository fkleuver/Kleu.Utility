using System;

namespace Kleu.Utility.Identity.Cryptography
{
    [Serializable]
    public class PasswordTooLongException : Exception
    {
        public PasswordTooLongException(string message)
            : base(message)
        {

        }
    }
}