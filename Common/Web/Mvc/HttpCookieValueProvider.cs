namespace MyLibrary.Web.Mvc {
	using System.Globalization;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.Mvc;

	public class HttpCookieValueProvider : IValueProvider {
        private readonly HttpCookieCollection _cookieCollection;

        public HttpCookieValueProvider(HttpCookieCollection cookieCollection) {
            _cookieCollection = cookieCollection;
        }

        public bool ContainsPrefix(string prefix) {
            return prefix.EndsWith("HttpCookie")
                ? _cookieCollection[Regex.Replace(prefix, "HttpCookie", "")] != null
                : false;
        }

        public ValueProviderResult GetValue(string key) {
            if (!key.EndsWith("HttpCookie"))
                return null;

            HttpCookie cookie = _cookieCollection[Regex.Replace(key, "HttpCookie", "")];
            return cookie != null
                ? new ValueProviderResult(cookie.Value, cookie.Value, CultureInfo.CurrentUICulture)
                : null;
        }
    }
}
