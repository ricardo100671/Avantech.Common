namespace MyLibrary
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;

	public static class StringExtensions
	{
		/// <summary>
		/// Indicates whether the specifed string is null or <see cref="System.String.Empty"/>.
		/// </summary>
		/// <param name="thisString">The this string.</param>
		/// <returns>
		///   <c>true</c> if the string is null or <see cref="System.String.Empty"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty(this String thisString){
			return String.IsNullOrEmpty(thisString);
		}

		/// <summary>
		/// Determines whether a string is null or contains only of white space.
		/// </summary>
		/// <param name="thisString">The this string.</param>
		/// <returns>
		///   <c>true</c> if the string is null or contains only white space; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrWhiteSpace(this String thisString)
		{
			return String.IsNullOrWhiteSpace(thisString);
		}

		/// <summary>
		/// Replaces the format items in the specifed string with the string representation 
		/// of a correspondiong object ina specified array.
		/// <remarks>Same functionality as String.Format(), that allows for a fluid coding style.</remarks>
		/// </summary>
		/// <param name="thisString">A composite format string.</param>
		/// <param name="args">The object to format.</param>
		/// <returns></returns>
		public static string FFormat(this String thisString, params object[] args) {
			return String.Format(thisString, args);
		}

		/// <summary>
		/// Concatenates an array of strings with a specified separator.
		/// </summary>
		/// <param name="thisArray">The array of strings to be concatenated.</param>
		/// <param name="separator">The separator to be used for concatenation.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public static string FJoin(this string[] thisArray, string separator)
		{
			return String.Join(separator, thisArray);
		}

		/// <summary>
		/// Deserializes the specified string into a strongly type object instance. 
		/// <remarks>
		/// Will throw an exception if the string does not contain a serialized object of type <paramref name="T"/>
		/// or if the string is empty.
		/// </remarks>
		/// </summary>
		/// <typeparam name="T">The type of the obect instance to instantiate.</typeparam>
		/// <param name="thisString">The string containg the serialized object of type <paramref name="T"/>.</param>
		/// <returns>An instance of the deserialized object.</returns>
		public static T Deserialize<T>(this string thisString)
		{
			var serializer = new DataContractSerializer(typeof(T));
			using (var reader = new StringReader(thisString))
			using (var stm = new XmlTextReader(reader))
			{
				return (T)serializer.ReadObject(stm);
			}
		}

		/// <summary>
		/// Attempts to deserializes the specified string into a strongly type object instance. 
		///	<remarks>
		/// Will not throw an exception if the string does not contain a serialized object of type <paramref name="T"/>
		/// or if the string is empty.
		/// </remarks>
		/// </summary>
		/// <typeparam name="T">The type of the object instance to instantiate.</typeparam>
		/// <param name="thisString">The string containg the serialized object of type <paramref name="T"/>.</param>
		/// <param name="t">An out parameter that will be assigned an instance of the deserialized object,
		/// if deserialization is successfull. Otherwise <see cref="System.Null"/>
		/// </param>
		/// <returns>
		///		<c>true</c> if deserialization was successfull; otherwise, <c>false</c>.
		/// </returns>
		public static bool TryDeserialize<T>(this string thisString, out T t)
			where T : class
		{
			try
			{
				t = Deserialize<T>(thisString);
				return true;
			}
			catch
			{
				t = null;
				return false;
			}
		}

		private static string ToHumanFromPascal(this string s)
		{
			var sb = new StringBuilder();
			char[] ca = s.ToCharArray();
			sb.Append(ca[0]);
			for (int i = 1; i < ca.Length - 1; i++)
			{
				char c = ca[i];
				if (char.IsUpper(c) && (char.IsLower(ca[i + 1]) || char.IsLower(ca[i - 1])))
				{
					sb.Append(" ");
				}
				sb.Append(c);
			}
			sb.Append(ca[ca.Length - 1]);

			return sb.ToString();
		}

		public static string Remove(this String thisString, string textToRemove, RegexOptions regexOptions = 0)
		{
			return regexOptions != 0
				? Regex.Replace(thisString, textToRemove, "", regexOptions)
				: Regex.Replace(thisString, textToRemove, "");
		}

		public static bool IsAlphaNumeric(this String thisString, bool allowEmpty = false, bool allowSpaces = true)
		{
			return Regex.IsMatch(
				thisString,
				"^[a-zA-Z0-9_{0}]{1}$".FFormat(allowSpaces ? "\\s" : "", allowEmpty ? "*" : "+")
			);
		}

		public static bool IsNumeric(this String thisString, bool allowSpaces = false)
		{
			return Regex.IsMatch(
				thisString,
				"^[0-9_{0}]$".FFormat(allowSpaces ? "\\s" : "")
			);
		}

		public static string[] Split(this String thisString, int segmentCount)
		{
			int interval = (int)Math.Round((decimal)(thisString.Length / segmentCount), 0, MidpointRounding.AwayFromZero);

			var segments = new List<string>();

			Regex paragraphs = new Regex(@"(.{1,20}\b('(s|t)|\s*((""|')\s*\.|\.\s*(""|')\s*\.?|\.|=|-|;|:|(""|')))?)", RegexOptions.Singleline);
			MatchCollection matches = paragraphs.Matches(thisString);

			foreach (Match match in matches)
			{
				segments.Add(match.ToString().Trim());
			}

			if (segments.Count > segmentCount)
				segments[segments.Count - 2] = segments[segments.Count - 2] + " " + segments[segments.Count - 1];

			return segments.ToArray();
		}
	}
}
