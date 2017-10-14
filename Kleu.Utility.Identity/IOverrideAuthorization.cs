namespace Kleu.Utility.Identity
{
    public interface IOverrideAuthorization
    {
        bool SkipAuthorization { get; }
        string UserName { get; }
    }
}
