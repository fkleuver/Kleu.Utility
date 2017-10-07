using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Castle.DynamicProxy;
using Kleu.Utility.Logging;

namespace Kleu.Utility.Web.Logging
{
    /// <summary>
    /// An optional module that registers the <see cref="CallLogInterceptor"/> so that InterceptWithCallLogger can be used.
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
            _logger.Info($"Loading module: {nameof(CallLogInterceptorModule)}");

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
