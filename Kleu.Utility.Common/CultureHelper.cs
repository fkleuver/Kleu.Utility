using System.Globalization;
using System.Reflection;
// ReSharper disable InconsistentNaming

// Credits: https://github.com/tallesl/net-Culture
namespace Kleu.Utility.Common
{

    /// <summary>
    /// Sets a default CurrentCulture for .NET threads.
    /// </summary>
    public static class CultureHelper
    {
        /// <summary>
        /// Sets a default for Thread.CurrentCulture.
        /// </summary>
        /// <param name="culture">Default culture</param>
        /// <returns>True if the set successfully, false otherwise</returns>
        public static bool SetDefault(CultureInfo culture)
        {
            return TrySetCulture45(culture) || TrySetCulture40(culture) || TrySetCulture20(culture);
        }

        /// <summary>
        /// Sets a default for Thread.CurrentUICulture.
        /// </summary>
        /// <param name="culture">Default UI culture</param>
        /// <returns>True if the set successfully, false otherwise</returns>
        public static bool SetUIDefault(CultureInfo culture)
        {
            return TrySetUICulture45(culture) || TrySetUICulture40(culture) || TrySetUICulture20(culture);
        }

        #region .NET 4.5

        private static bool TrySetCulture45(CultureInfo culture)
        {
            return SetStaticPublicProperty("DefaultThreadCurrentCulture", culture);
        }

        private static bool TrySetUICulture45(CultureInfo culture)
        {
            return SetStaticPublicProperty("DefaultThreadCurrentUICulture", culture);
        }

        #endregion

        #region .NET 4.0

        private static bool TrySetCulture40(CultureInfo culture)
        {
            return SetStaticPrivateField("s_userDefaultCulture", culture);
        }

        private static bool TrySetUICulture40(CultureInfo culture)
        {
            return SetStaticPrivateField("s_userDefaultUICulture", culture);
        }

        #endregion

        #region .NET 2.0

        private static bool TrySetCulture20(CultureInfo culture)
        {
            return SetStaticPrivateField("m_userDefaultCulture", culture);
        }

        private static bool TrySetUICulture20(CultureInfo culture)
        {
            return SetStaticPrivateField("m_userDefaultUICulture", culture);
        }

        #endregion

        private static bool SetStaticPublicProperty(string name, CultureInfo value)
        {
            var property = typeof(CultureInfo).GetProperty(name, BindingFlags.Static | BindingFlags.Public);
            if (property == null)
            {
                return false;
            }
            else
            {
                property.SetValue(null, value, null);
                return true;
            }
        }

        private static bool SetStaticPrivateField(string name, CultureInfo value)
        {
            var field = typeof(CultureInfo).GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
            if (field == null)
            {
                return false;
            }
            else
            {
                field.SetValue(null, value);
                return true;
            }
        }
    }
}
