using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Kleu.Utility.Common;
using Kleu.Utility.Logging;
using Kleu.Utility.Web.Serialization;
using Microsoft.Owin;

namespace Kleu.Utility.Web.Middlewares
{
    public sealed class DynamicFilesOptions
    {
        public string BaseDirectory { get; set; }
        public string DefaultFile { get; set; } = "index.html";
        public ILog Logger { get; set; }
        public bool LogHandledRequests { get; set; }
        public bool LogSkippedRequests { get; set; }
        public bool AbortIfFileNotFound { get; set; }
        public string SkipPathRegex { get; set; }
        public static DynamicFilesOptions Default => new DynamicFilesOptions();
    }

    public sealed class DynamicFilesMiddleware : OwinMiddleware
    {
        private readonly DynamicFilesOptions _options;

        public DynamicFilesMiddleware(OwinMiddleware next, DynamicFilesOptions options) : base(next)
        {
            Guard.AgainstNull(nameof(options), options);
            if (options.LogHandledRequests || options.LogSkippedRequests)
            {
                Guard.AgainstNull(nameof(options.Logger), options.Logger);
            }
            _options = options;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var method = context.Request.Method;
            var scheme = context.Request.Scheme;
            var requestpath = context.Request.Path.Value;
            var requestJson = $"{{ \"{nameof(method)}\": \"{method}\", \"{nameof(scheme)}\": \"{scheme}\", \"{nameof(requestpath)}\": \"{requestpath}\" }}";

            if (method != "GET" || !Regex.IsMatch(scheme, "https?") || Regex.IsMatch(requestpath, _options.SkipPathRegex))
            {
                if (_options.LogSkippedRequests)
                {
                    Log($"Skipping request: {requestJson}");
                }
                await Next.Invoke(context);
            }
            else
            {
                Log($"Handling request: {requestJson}");
                if (requestpath == "/")
                {
                    requestpath += _options.DefaultFile;
                }
                var fullpath = _options.BaseDirectory + requestpath.Replace(@"/", @"\");
                if (!File.Exists(fullpath))
                {
                    if (_options.AbortIfFileNotFound)
                    {
                        Log($"File not found: {{ \"{nameof(fullpath)}\": \"{fullpath}\" }}. Aborting..");
                    }
                    else
                    {
                        Log($"File not found: {{ \"{nameof(fullpath)}\": \"{fullpath}\" }}. Proceeding..");
                        await Next.Invoke(context);
                    }
                }
                else
                {
                    var mime = MimeMapping.GetMimeMapping(fullpath);

                    Log($"Serving file: {{ \"{nameof(fullpath)}\": \"{fullpath}\", \"{nameof(mime)}\": \"{mime}\" }}");
                    using (var file = File.OpenRead(fullpath))
                    {
                        await file.CopyToAsync(context.Response.Body);
                    }
                    context.Response.Headers.Set(OwinKeys.ContentTypeHeader, mime);
                }
            }
        }

        private void Log(string message)
        {
            _options.Logger.Debug($"[{nameof(DynamicFilesMiddleware)}] {message}");
        }
    }
}
