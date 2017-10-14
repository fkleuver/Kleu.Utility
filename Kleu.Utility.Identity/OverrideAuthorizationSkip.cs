namespace Kleu.Utility.Identity
{
    public sealed class OverrideAuthorizationSkip : IOverrideAuthorization
    {
        public bool SkipAuthorization => true;
        public string UserName { get; }

        public OverrideAuthorizationSkip(string userName)
        {
            UserName = userName;
        }
    }
}
