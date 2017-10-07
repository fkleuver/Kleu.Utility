using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Kleu.Utility.Logging;
using Microsoft.Owin.Hosting;
using Owin;
using Serilog;
using Serilog.Events;

namespace Kleu.Utility.ConsoleHost
{
    public static class ConsoleBootstrapper
    {
        private static ILog _logger;
        private static ILog Logger => _logger ?? (_logger = LogProvider.GetCurrentClassLogger());


        private const string LogFormat =
            @"{Timestamp:HH:mm} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}";

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
            int wFlags);

        private static string AppName = "OWIN";
        
        public static void Run(string url, Action<IAppBuilder> startup)
        {
            AppName = $"{AppName} - {url}";
            RunInternal(() => WebApp.Start(url, startup));
        }
        
        public static void Run(StartOptions options, Action<IAppBuilder> startup)
        {
            AppName = $"{AppName} - {options.Urls.FirstOrDefault() ?? $"Port {options.Port}"}";
            RunInternal(() => WebApp.Start(options, startup));
        }
        
        public static void Run<TStartup>(string url)
        {
            AppName = $"{AppName} - {url}";
            RunInternal(() => WebApp.Start<TStartup>(url));
        }
        
        public static void Run<TStartup>(StartOptions options)
        {
            AppName = $"{AppName} - {options.Urls.FirstOrDefault() ?? $"Port {options.Port}"}";
            RunInternal(() => WebApp.Start<TStartup>(options));
        }
        
        public static void Run(string url)
        {
            AppName = $"{AppName} - {url}";
            RunInternal(() => WebApp.Start(url));
        }
        
        public static void Run(StartOptions options)
        {
            AppName = $"{AppName} - {options.Urls.FirstOrDefault() ?? $"Port {options.Port}"}";
            RunInternal(() => WebApp.Start(options));
        }

        private static void RunInternal(Func<IDisposable> startAction)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Console(outputTemplate: LogFormat)
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .CreateLogger();

            Logger.Info($"Starting application: {AppName}");

            SetWindowPos(
                hWnd: GetConsoleWindow(),
                hWndInsertAfter: 0,
                x: 0,
                y: 0,
                cx: 0,
                cy: 0,
                wFlags: 0);

            Console.Title = AppName;
            Console.CursorVisible = false;
            Console.SetWindowSize(80, 60);

            IDisposable server = null;
            try
            {
                server = startAction();
                PromptForExit(server);
            }
            catch (Exception ex)
            {
                var exception = ex;
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                Logger.FatalException(@"Unable to start application", exception);
                PromptForExit(server);
            }
        }

        private static void PromptForExit(IDisposable server)
        {
            Logger.Info(@"Press [Enter] to exit application");
            Console.ReadLine();
            Logger.Info(@"Shutting down..");
            Thread.Sleep(1000);
            try
            {
                server?.Dispose();
            }
            catch
            {
                // Ignored
            }
            finally
            {
                Environment.Exit(-1);
            }
        }
    }
}