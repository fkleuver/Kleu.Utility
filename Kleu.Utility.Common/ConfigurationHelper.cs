using System;
using System.Configuration;

namespace Kleu.Utility.Common
{
    public static class ConfigurationHelper
    {
        public static T Get<T>(string name)
        {
            var value = ConfigurationManager.AppSettings.Get(name);
            if (value == null)
            {
                return default(T);
            }

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), value);

            return typeof(T).ConvertFrom(value);
        }
    }
}
