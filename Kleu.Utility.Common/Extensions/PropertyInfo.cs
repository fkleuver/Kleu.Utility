using System.Linq;
using System.Reflection;

namespace Kleu.Utility.Common
{
    public static class PropertyInfoExtensions
    {
        public static bool HasSetter(this PropertyInfo property)
        {
            return property.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Any(m => m.Name == "set_" + property.Name);
        }

        public static bool HasGetter(this PropertyInfo property)
        {
            return property.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Any(m => m.Name == "get_" + property.Name);
        }
    }
}
