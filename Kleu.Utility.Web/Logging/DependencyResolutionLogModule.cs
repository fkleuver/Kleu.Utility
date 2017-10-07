using Autofac;
using Autofac.Core;
using Kleu.Utility.Common;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Web.Logging
{
    /// <summary>
    /// An optional module that logs the depency resolution process in a structured format.
    /// </summary>
    public sealed class DependencyResolutionLogModule : Module
    {
        private readonly ILog _logger;

        public int Depth = 0;

        public DependencyResolutionLogModule(ILog logger)
        {
            Guard.AgainstNull(nameof(logger), logger);

            _logger = logger;
        }

        public DependencyResolutionLogModule() : this(LogProvider.GetCurrentClassLogger())
        {
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
            return new string('-', Depth * 2);
        }

        private void RegistrationOnPreparing(object sender, PreparingEventArgs preparingEventArgs)
        {
            _logger.Info($"{GetPrefix()} Resolving  {preparingEventArgs.Component.Activator.LimitType}");
            Depth++;
        }

        private void RegistrationOnActivating(object sender, ActivatingEventArgs<object> activatingEventArgs)
        {
            Depth--;
        }
    }
}
