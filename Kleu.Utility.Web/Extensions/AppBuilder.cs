using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using Kleu.Utility.Logging;
using Kleu.Utility.Web.Logging;
using Kleu.Utility.Web.Middlewares;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Kleu.Utility.Web
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseHttpLogging(this IAppBuilder app)
        {
            return app.Use<HttpLoggingMiddleware>();
        }

        /// <summary>
        /// Must come directly AFTER security middlewares such as CORS and AccessTokenValidation
        /// </summary>
        /// <param name="app"></param>
        /// <param name="container"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IAppBuilder UseAutofacWebApiStack(this IAppBuilder app, ILifetimeScope container, HttpConfiguration config)
        {
            var logger = container.Resolve<ILog>();

            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
            config.Services.Add(typeof(IExceptionLogger), new LogProviderExceptionLogger(logger));

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            ConfigureJsonFormatter(config.Formatters.JsonFormatter);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

            config.EnsureInitialized();

            return app;
        }

        private static void ConfigureJsonFormatter(JsonMediaTypeFormatter formatter)
        {
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            formatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            formatter.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            formatter.UseDataContractJsonSerializer = false;
            formatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            formatter.MediaTypeMappings.Add(new RequestHeaderMapping("Accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, "application/json"));

            formatter.SerializerSettings.Error += (sender, args) =>
            {
                var scopedLogger = HttpContext.Current.ResolveFromRequestScope(typeof(ILog)) as ILog;
                scopedLogger.ErrorException("An error occured during JSON serialization", args.ErrorContext.Error);
                HttpContext.Current.AddError(args.ErrorContext.Error);
            };
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
                StaticFileOptions = {ServeUnknownFileTypes = true},
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
        /// <param name="baseDirectory"></param>
        /// <param name="defaultFile"></param>
        public static IAppBuilder UseDynamicFiles(this IAppBuilder app, string baseDirectory, string defaultFile = "index.html")
        {
            app.Use(new Func<AppFunc, AppFunc>(next => (async context =>
            {
                var method = (string)context[OwinKeys.RequestMethodKey];
                var scheme = (string)context[OwinKeys.RequestSchemeKey];

                if (method == "GET" && (scheme == "http" || scheme == "https"))
                {
                    var requestpath = (string)context[OwinKeys.RequestPathKey];
                    if (requestpath == "/")
                    {
                        requestpath += defaultFile;
                    }
                    var fullpath = baseDirectory + requestpath.Replace(@"/", @"\");


                    if (File.Exists(fullpath))
                    {

                        using (var file = File.OpenRead(fullpath))
                        {
                            await file.CopyToAsync((Stream)context[OwinKeys.ResponseBodyKey]);
                        }

                        var mime = MimeMapping.GetMimeMapping(fullpath);

                        if (!(context[OwinKeys.ResponseHeadersKey] is Dictionary<string, string[]> responseHeader))
                        {
                            context[OwinKeys.ResponseHeadersKey] = new Dictionary<string, string[]>();
                            responseHeader = (Dictionary<string, string[]>)context[OwinKeys.ResponseHeadersKey];
                        }
                        responseHeader.Add(OwinKeys.ContentTypeHeader, new[] { mime });

                        return;
                    }
                }

                await next.Invoke(context);
            })));

            return app;
        }
    }
}
