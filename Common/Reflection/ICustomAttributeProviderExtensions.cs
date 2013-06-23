namespace MyLibrary.Reflection
{
	using System;
	using System.Reflection;

	public static class ICustomAttributeProviderExtensions
	{
		/// <summary>
		/// Gets the attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static T GetAttribute<T>(this ICustomAttributeProvider provider)
			where T : Attribute
		{
			var attributes = provider.GetCustomAttributes(typeof(T), true);
			return attributes.Length > 0 ? attributes[0] as T : null;
		}
	}
}
