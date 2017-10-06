using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kleu.Utility.Common
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesWithCustomAttribute<TAttribute>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(a => a.GetTypes(), (a, t) => t)
                .Where(t => t
                    .GetCustomAttributes(typeof(TAttribute), true)
                    .Any())
                .Select(t => t);
        }

        public static string GetShortName(this Assembly assembly)
        {
            Guard.AgainstNull(nameof(assembly), assembly);

            var shortName = assembly.FullName;
            if (shortName.Contains(","))
            {
                shortName = shortName.Split(',')[0];
            }

            return shortName;
        }

        public static string GetShortName(this AssemblyName assemblyName)
        {
            Guard.AgainstNull(nameof(assemblyName), assemblyName);

            var shortName = assemblyName.FullName;
            if (shortName.Contains(","))
            {
                shortName = shortName.Split(',')[0];
            }

            return shortName;
        }
    }
}
