using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Kleu.Utility.Logging;
using Kleu.Utility.Web.Middlewares;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Kleu.Utility.Web
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Enable HTTP logging for the middleware that comes after this one, using Autofac to resolve the logger.
        /// The Autofac middleware registration must precede this one, and the <see cref="HttpLoggingMiddleware"/> must be registered with Autofac.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IAppBuilder UseHttpLogging(this IAppBuilder app)
        {
            return app.Use<HttpLoggingMiddleware>();
        }

        /// <summary>
        /// Enable HTTP logging for the middleware that comes after this one, using the supplied logger
        /// </summary>.
        /// <param name="app"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IAppBuilder UseHttpLogging(this IAppBuilder app, ILog logger)
        {
            return app.Use<HttpLoggingMiddleware>(logger);
        }

        /// <summary>
        /// Enable exception handling for the middleware that comes after this one, using <see cref="ExceptionHandlingMiddleware"/> and using the supplied logger.
        /// </summary>.
        /// <param name="app"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IAppBuilder UseExceptionHandler(this IAppBuilder app, ILog logger)
        {
            return app.Use<ExceptionHandlingMiddleware>(logger);
        }

        /// <summary>
        /// Must come directly AFTER security middlewares such as CORS and AccessTokenValidation.
        /// Any modifications to HttpConfiguration must happen BEFORE this call, since it will be made immutable.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="container"></param>
        /// <param name="config"></param>
        /// <param name="configureSerializationWithCommonDefaults"></param>
        /// <returns></returns>
        public static IAppBuilder UseAutofacWebApiStack(
            this IAppBuilder app,
            ILifetimeScope container,
            HttpConfiguration config,
            bool configureSerializationWithCommonDefaults = true,
            bool mapHttpAttributeRoutes = true,
            bool setHttpDependencyResolverToAutofac = true,
            bool useAutofacMiddleware = true,
            bool useAutofacWebApi = true,
            bool useWebApi = true)
        {
            if (configureSerializationWithCommonDefaults)
            {
                config.ConfigureSerializationWithCommonDefaults();
            }
            if (mapHttpAttributeRoutes)
            {
                config.MapHttpAttributeRoutes();
            }

            if (setHttpDependencyResolverToAutofac)
            {
                config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            }

            if (useAutofacMiddleware)
            {
                app.UseAutofacMiddleware(container);
            }
            if (useAutofacWebApi)
            {
                app.UseAutofacWebApi(config);
            }
            if (useWebApi)
            {
                app.UseWebApi(config);
            }

            config.EnsureInitialized();

            return app;
        }

        /// <summary>
        /// Must come BEFORE any other middlewares
        /// </summary>
        /// <param name="app"></param>
        /// <param name="exposedHeaders"></param>
        /// <returns></returns>
        public static IAppBuilder UseCorsWithExposedHeaders(this IAppBuilder app, params string[] exposedHeaders)
        {
            var policy = new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                SupportsCredentials = true
            };

            foreach (var header in exposedHeaders)
            {
                policy.ExposedHeaders.Add(header);
            }

            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });

            return app;
        }

        public static IAppBuilder UseEmbeddedFiles(this IAppBuilder app, Assembly assembly, string baseNamespace)
        {
            var fileSystem = new EmbeddedResourceFileSystem(assembly, baseNamespace);
            var fileServerOptions = new FileServerOptions
            {
                StaticFileOptions = { ServeUnknownFileTypes = true },
                FileSystem = fileSystem
            };

            app.UseFileServer(fileServerOptions);

            return app;
        }

        /// <summary>
        /// When hosting in IIS, the ExtensionlessUrlHandler needs to be bypassed. Add the following &lt;handlers&gt; element under &lt;system.webServer&gt;:
        /// &lt;add name=&quot;Owin&quot; verb=&quot;&quot; path=&quot;*&quot; type=&quot;Microsoft.Owin.Host.SystemWeb.OwinHttpHandler, Microsoft.Owin.Host.SystemWeb&quot; /&gt;
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public static IAppBuilder UseDynamicFiles(this IAppBuilder app, DynamicFilesOptions options)
        {
            return app.Use<DynamicFilesMiddleware>(options);
        }
    }
}
