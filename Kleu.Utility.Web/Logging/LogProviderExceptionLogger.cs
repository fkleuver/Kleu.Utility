using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Web.Logging
{
    public sealed class LogProviderExceptionLogger : IExceptionLogger
    {
        private readonly ILog _logger;
        
        public LogProviderExceptionLogger(ILog logger)
        {
            _logger = logger;
        }

        public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            await Task.Run(() => _logger.ErrorException("Unhandled exception", context.Exception), cancellationToken);
        }
    }
}
