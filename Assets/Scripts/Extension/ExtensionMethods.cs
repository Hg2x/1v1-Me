using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ICKT
{
	public static class ExtensionMethods
	{
		public static bool IsTypeOf<T>(this Type t)
		{
			return t.IsSubclassOf(typeof(T)) || t == typeof(T);
		}

		public static IEnumerable<Type> GetTypesWithCustomAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute
		{
			return assembly.GetTypes().Where(t => t.GetCustomAttribute<TAttribute>() != null);
		}
	}
}
