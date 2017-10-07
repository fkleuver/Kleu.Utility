using Autofac;
using Autofac.Core;
using Kleu.Utility.Common;
using Kleu.Utility.Logging;

namespace Kleu.Utility.IoC
{
    /// <summary>
    /// An optional module that logs the depency resolution process in a structured format.
    /// </summary>
    public sealed class DependencyResolutionLogModule : Module
    {
        private readonly ILog _logger;

        private int _depth = 0;

        public DependencyResolutionLogModule(ILog logger)
        {
            Guard.AgainstNull(nameof(logger), logger);

            _logger = logger;
        }

        public DependencyResolutionLogModule() : this(LogProvider.GetCurrentClassLogger())
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            _logger.Debug($"[Autofac Module] Loading {nameof(DependencyResolutionLogModule)}");
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            registration.Preparing += RegistrationOnPreparing;
            registration.Activating += RegistrationOnActivating;
            base.AttachToComponentRegistration(componentRegistry, registration);
        }

        private string GetPrefix()
        {
            return new string('-', _depth * 2);
        }

        private void RegistrationOnPreparing(object sender, PreparingEventArgs preparingEventArgs)
        {
            Log($"{GetPrefix()} Resolving  {preparingEventArgs.Component.Activator.LimitType}");
            _depth++;
        }

        private void RegistrationOnActivating(object sender, ActivatingEventArgs<object> activatingEventArgs)
        {
            _depth--;
        }
        private void Log(string message)
        {
            _logger.Debug($"[{nameof(DependencyResolutionLogModule)}] {message}");
        }
    }
}
