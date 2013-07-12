
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace Avantech.Common
{
    /// <summary>
	/// Provides extension method for any type
	/// Typically created as Generic extensions
	/// </summary>
	public static class TExtensions
	{
		public static T Clone<T>(this T obj)
		{
			return obj.Serialize().Deserialize<T>();
		}

		public static string Serialize<T>(this T obj)
		{
			var serializer = new DataContractSerializer(obj.GetType());
			using (var writer = new StringWriter())
			using (var stm = new XmlTextWriter(writer))
			{
				serializer.WriteObject(stm, obj);
				return writer.ToString();
			}
		}

		public static IEnumerable<string> GetEnumNames<T>(this T thisEnum) {
			var thisEnumType = thisEnum.GetType();
			if (!thisEnumType.IsEnum)
				throw new ArgumentException("Only valid for enumeration types.");

			return Enum.GetNames(thisEnumType);
		}

		public static bool IsNullable<T>(this T obj)
		{
			if (obj == null) return true; 
			Type type = typeof(T);
			if (!type.IsValueType) return true; 
			if (Nullable.GetUnderlyingType(type) != null) return true; 
			return false; 
		}

		public static void MergeWith<T>(this T primary, T secondary, bool overwrite = false)
		{
			foreach (var pi in typeof(T).GetProperties())
			{
				var priValue = pi.GetValue(primary, null);
				var secValue = pi.GetValue(secondary, null);
				if (
					(overwrite && secValue != null)
					|| priValue == null
					|| (pi.PropertyType.IsValueType && priValue == Activator.CreateInstance(pi.PropertyType))
				)
				{
					pi.SetValue(primary, secValue, null);
				}
			}
		}
        /// <summary>
        /// Wraps an object in a List.  
        /// </summary>
        /// <typeparam name="T">The type of object to be held in the list.</typeparam>
        /// <param name="thisT">The initial object instance to add to the list.</param>
        /// <param name="listType">The list types available.</param>
        /// <returns>A new, IList, base object containing the initial object.</returns>
        public static IList<T> EnList<T>(this T thisT, AsListListTypes listType = AsListListTypes.List)
        {
            IList<T> list;

            switch (listType)
            {
                case AsListListTypes.Collection:
                    list = new Collection<T>();
                    break;
                case AsListListTypes.List:
                    list = new List<T>();
                    break;
                default:
                    throw new InvalidOperationException("'listType' unhandled.");
            }

            list.Add(thisT);

            return list;
        }

        public enum AsListListTypes { List, Collection }

        /// <summary>
        /// Gets the underlying value enum value for an enum member.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <typeparam name="TEnumValue">The expected type of the underlying enum value.</typeparam>
        /// <param name="thisEnum">The this enum member whos value is required.</param>
        /// <returns>The underlying enumeration value.</returns>
        /// <exception cref="System.InvalidOperationException">Only valid for enumeration types.</exception>
        public static TEnumValue GetEnumValue<TEnum, TEnumValue>(this TEnum thisEnum)
            where TEnumValue : struct
            where TEnum : struct
        {
            var enumType = thisEnum.GetType();
            if (!enumType.IsEnum) throw new InvalidOperationException("Only valid for enumeration types.");

            return Enum.GetValues(enumType)
                .Cast<TEnumValue>()
                .Single(v => (
                    (TEnum)Enum.Parse(enumType, v.ToString())
                ).Equals(thisEnum));
        }
	}
}
