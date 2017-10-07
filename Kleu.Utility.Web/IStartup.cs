using Owin;

namespace Kleu.Utility.Web
{
    public interface IStartup
    {
        void Configuration(IAppBuilder app);
    }
}
