using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kleu.Utility.Common
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypesWithCustomAttribute<TAttribute>(this IEnumerable<Type> types)
        {
            return types
                .Where(t => t
                    .GetCustomAttributes(typeof(TAttribute), true)
                    .Any())
                .Select(t => t);
        }

        public static bool HasAttribute<TAttribute>(this Type type)
        {
            return type.GetCustomAttributes(typeof(TAttribute), true).Any();
        }

        public static IEnumerable<FieldInfo> GetInstanceFields(this Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        public static IEnumerable<Type> GetGenericTypeArgumentsImplementingInterface<T>(this Type type)
        {
            return type.BaseType.GenericTypeArguments
                .Where(t => t.GetInterfaces().Any(i => i == typeof(T)));
        }

        public static Type GetSingleGenericTypeArgument(this Type type)
        {
            var baseType = type;
            while (baseType != null)
            {
                if (baseType.GetGenericArguments().Count() == 1)
                {
                    return baseType.GetGenericArguments().First();
                }
                baseType = baseType.BaseType;
            }
            return null;
        }

        public static bool ImplementsGenericClass(this Type type, Type unboundGenericType)
        {
            return type.IsClass &&
                   type.BaseType.IsGenericType &&
                   type.BaseType.GetGenericTypeDefinition() == unboundGenericType;
        }

        public static bool ImplementsGenericInterface(this Type type, Type genericType)
        {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == genericType);
        }

        public static bool ImplementsAnyGenericInterface(this Type type, params Type[] genericTypes)
        {
            return genericTypes.Any(type.ImplementsGenericInterface);
        }

        public static dynamic ConvertFrom(this Type destinationType, dynamic source)
        {
            return Convert.ChangeType(source, destinationType);
        }
    }
}
