using System;
using Kleu.Utility.Logging;
using Xunit.Abstractions;

namespace Kleu.Utility.Testing.Logging
{
    /// <summary>
    /// An implementation for LibLog's <see cref="ILog"/> interface that writes all logging output to XUnit's <see cref="ITestOutputHelper"/>, so that log messages can be seen from the test output.
    /// </summary>
    public sealed class TestOutputLogger : ILog
    {
        private readonly ITestOutputHelper _output;

        public TestOutputLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            if (messageFunc != null)
            {
                var exceptionText = "";
                if (exception != null)
                {
                    exceptionText = $" ({exception.GetType().Name}: {exception.Message})";
                }

                _output.WriteLine($"[{logLevel}]: {messageFunc()}{exceptionText}");

            }
            return true;
        }
    }
}