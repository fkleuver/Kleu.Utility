using System;

namespace Kleu.Utility.Identity.Cryptography
{
    [Serializable]
    public class SaltLessThanRecommended : Exception
    {
        public SaltLessThanRecommended() : base("Salt is less than the 8 byte size recommended in Rfc2898")
        {

        }
    }
}