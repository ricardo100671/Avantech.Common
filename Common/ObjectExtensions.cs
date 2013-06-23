namespace MyLibrary
{
	public static class ObjectExtensions
	{
		public static bool IsNumeric(this object thisObject, bool allowSpaces = false) {
			return thisObject.ToString().IsNumeric(allowSpaces);
		}

		public static string ToString(this object thisObject, string format)
		{
			return string.Format("{0:" + format + "}", thisObject);
		}
	}
}
