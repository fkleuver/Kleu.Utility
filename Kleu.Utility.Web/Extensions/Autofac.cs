using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Kleu.Utility.Logging;
using Kleu.Utility.Web.Logging;

namespace Kleu.Utility.Web
{
    public static class AutofacExtensions
    {
        public static void RegisterLogProvider(this ContainerBuilder builder, string loggerName = "LibLog")
        {
            builder.Register(c => LogProvider.GetLogger(loggerName)).AsImplementedInterfaces().InstancePerDependency();
        }
        
        /// <summary>
        /// Intercepts an interface registration with the <see cref="CallLogInterceptor"/> interceptor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registrationBuilder"></param>
        public static void InterceptWithCallLogger<T>(
            this IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> registrationBuilder)
        {
            registrationBuilder
                .EnableInterfaceInterceptors()
                .InterceptedBy(nameof(CallLogInterceptor));
        }
    }
}
