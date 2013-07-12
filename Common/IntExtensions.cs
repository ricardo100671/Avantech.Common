using System;

namespace Avantech.Common
{
    public static class IntExtensions
	{
		/// <summary>
		/// Converts an Enum member for an integer
		/// </summary>
		/// <typeparam name="TEnum">The target enumeration type required.</typeparam>
		/// <param name="thisInt">The integer to convert</param>
		/// <returns>The enumeration member of the target enumeration type whos value matches the integer.</returns>
		public static TEnum ToEnum<TEnum>(this int thisInt)
			where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
				throw new ArgumentException("Generic argument must be a valid Enum type.");

			return (TEnum)Enum.Parse(typeof(TEnum), thisInt.ToString());
		}

		/// <summary>
		/// Converts a nullable integer to an enum member
		/// </summary>
		/// <typeparam name="TEnum">The target enumeration type required.</typeparam>
		/// <param name="thisInt">The integer to convert.</param>
		/// <returns>
		///		The enumeration member of the target enumeration type whos value matches the integer.
		///		If the integer is null the member whos value is zero will be returned.
		/// </returns>
		public static TEnum ToEnum<TEnum>(this int? thisInt)
			where TEnum : struct
		{
			if (!typeof(TEnum).IsEnum)
				throw new ArgumentException("Generic argument must be a valid Enum type.");

			if (thisInt == null)
				return default(TEnum);

			return (TEnum)Enum.Parse(typeof(TEnum),thisInt.ToString());
		}
	}
}
