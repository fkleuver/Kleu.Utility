using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Kleu.Utility.Logging;
using Kleu.Utility.Web.Logging;

namespace Kleu.Utility.Web
{
    public static class HttpConfigurationExtensions
    {
        public static void EnableWebApiDiagnostics(
            this HttpConfiguration config,
            ILog logger,
            bool verbose = true,
            SourceLevels level = SourceLevels.All,
            string traceSourceName = "LibLog")
        {
            var liblog = new TraceSource(traceSourceName) { Switch = { Level = level } };
            liblog.Listeners.Add(LibLogTraceListener.CreateUsingLogger(logger));

            var diag = config.EnableSystemDiagnosticsTracing();
            diag.IsVerbose = verbose;
            diag.TraceSource = liblog;
        }

        public static void ConfigureErrorHandling(
            this HttpConfiguration config,
            ILog logger,
            IncludeErrorDetailPolicy includeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly)
        {
            config.Services.Add(typeof(IExceptionLogger), new LogProviderExceptionLogger(logger));
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
        }

        public static void ConfigureSerializationWithCommonDefaults(
            this HttpConfiguration config,
            bool removeXmlFormatter = true,
            bool configureCamelCaseForJsonFormatter = true,
            bool addLibLogErrorHandlerForJsonFormatter = true)
        {
            if (removeXmlFormatter)
            {
                config.Formatters.Remove(config.Formatters.XmlFormatter);
            }
            if (configureCamelCaseForJsonFormatter)
            {
                config.Formatters.JsonFormatter.ConfigureCamelCase();
            }
            if (addLibLogErrorHandlerForJsonFormatter)
            {
                config.Formatters.JsonFormatter.AddLibLogErrorHandler();
            }
        }
    }
}
