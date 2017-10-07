using Autofac;
using Castle.DynamicProxy;
using Kleu.Utility.Logging;

namespace Kleu.Utility.IoC
{
    /// <summary>
    /// Registers the <see cref="CallLogInterceptor"/> so that InterceptWithCallLogger can be used.
    /// </summary>
    public sealed class CallLogInterceptorModule : Module
    {
        private readonly ILog _logger;

        public CallLogInterceptorModule(ILog logger)
        {
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            _logger.Debug($"[Autofac Module] Loading {nameof(CallLogInterceptorModule)}");

            builder
                .Register(c =>
                {
                    var logger = c.Resolve<ILog>();
                    return new CallLogInterceptor(logger);
                })
                .Named<IInterceptor>(nameof(CallLogInterceptor))
                .InstancePerDependency();
        }
    }
}
