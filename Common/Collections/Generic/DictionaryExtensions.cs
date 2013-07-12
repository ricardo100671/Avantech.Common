
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace Avantech.Common.Collections.Generic
{
    public static class DictionaryExtensions
	{
		/// <summary>
		/// Converts the dictionary to a Json encoded string.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="thisDictionary">The this dictionary.</param>
		/// <returns></returns>
		public static string ToJson<TKey, TValue>(this Dictionary<TKey, TValue> thisDictionary)
		{
			return Json.Encode(thisDictionary.ToArray());
		}
	}
}
