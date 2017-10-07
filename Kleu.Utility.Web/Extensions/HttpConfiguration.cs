using System.Diagnostics;
using System.Web.Http;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Web
{
    public static class HttpConfigurationExtensions
    {
        public static void EnableWebApiDiagnostics(
            this HttpConfiguration config,
            bool verbose = true,
            SourceLevels level = SourceLevels.All,
            string traceSourceName = "LibLog")
        {
            var liblog = new TraceSource(traceSourceName) { Switch = { Level = level } };
            liblog.Listeners.Add(LibLogTraceListener.CreateUsingCurrentClassLogger());

            var diag = config.EnableSystemDiagnosticsTracing();
            diag.IsVerbose = verbose;
            diag.TraceSource = liblog;
        }
    }
}
