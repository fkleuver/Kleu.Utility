using Autofac;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Web
{
    public static class AutofacExtensions
    {
        public static void RegisterLogProvider(this ContainerBuilder builder, string loggerName = "LibLog")
        {
            builder.Register(c => LogProvider.GetLogger(loggerName)).AsImplementedInterfaces().InstancePerDependency();
        }
    }
}
