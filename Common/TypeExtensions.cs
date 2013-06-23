namespace MyLibrary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class TypeExtensions
	{
		/// <summary>
		/// Converts an enumeration into a dictionary, 
		/// where the dictionary key is the numeric value of the enumeration members
		/// and the dictionary value is the names of the enumerations members.
		/// </summary>
		/// <param name="thisEnum"></param>
		/// <returns></returns>
		public static Dictionary<int,string> EnumToDictionary(this Type thisEnum){
			if (!thisEnum.IsEnum)
				throw new ArgumentException("Only valid for enumeration types.");

			return Enum.GetValues(thisEnum).Cast<Int32>().ToDictionary(e => e, e => Enum.GetName(thisEnum, e));
		}
	}
}
