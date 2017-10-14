using System;

namespace Kleu.Utility.Identity.Cryptography
{
    [Serializable]
    public class IterationsLessThanRecommended : Exception
    {
        public IterationsLessThanRecommended() : base("Iteration count is less than the 1000 recommended in Rfc2898")
        {

        }
    }
}