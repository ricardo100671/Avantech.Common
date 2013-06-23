namespace MyLibrary
{
	using System;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Xml;
	using System.Collections.Generic;

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
	}
}
