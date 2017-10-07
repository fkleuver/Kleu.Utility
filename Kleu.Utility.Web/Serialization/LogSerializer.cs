using Kleu.Utility.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Kleu.Utility.Web.Serialization
{
    public class LogSerializer
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };

        static LogSerializer()
        {
            JsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }

        public static string Serialize(object logObject)
        {
            return JsonConvert.SerializeObject(logObject, JsonSettings);
        }

        public static void SerializeAndLog(string message, object logObject)
        {
            var json = Serialize(logObject);
            Logger.Info($"{message}: {json}");
        }
    }
}
