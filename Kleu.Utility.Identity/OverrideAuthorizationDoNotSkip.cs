using System;

namespace Kleu.Utility.Identity
{
    public sealed class OverrideAuthorizationDoNotSkip : IOverrideAuthorization
    {
        public bool SkipAuthorization => false;

        public string UserName => throw new InvalidOperationException($"Resolving {nameof(UserName)} is not allowed from {nameof(OverrideAuthorizationDoNotSkip)}");
    }
}