using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kleu.Utility.Logging;
using Xunit.Abstractions;

namespace Kleu.Utility.Testing.Logging
{
    /// <summary>
    /// Base test class that configures LibLog to write to the Xunit test output.
    /// Must pass <see cref="ITestOutputHelper"/> to the base constructor.
    /// </summary>
    public abstract class TestClassWithLogging
    {
        protected TestClassWithLogging(ITestOutputHelper output)
        {
            LogProvider.SetCurrentLogProvider(new TestOutputLogProvider(output));
        }
    }
}
