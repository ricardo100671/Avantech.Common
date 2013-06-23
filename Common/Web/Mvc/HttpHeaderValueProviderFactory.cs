namespace MyLibrary.Web.Mvc {
	using System.Collections.Specialized;
	using System.Globalization;
	using System.Text.RegularExpressions;
	using System.Web.Mvc;

	public class HubHttpHeaderValueProviderFactory : ValueProviderFactory {
        /// <summary>
        /// Instantiates and returns a Value Provider which retrieves values from Request Headers
        /// Strips all hyphens from header field names so that they can be specified as, case insensative, action parameter names.
        /// Also suffixes header names with "HttpHeader" to avoid potential name conflicts.
        /// In addition to providing all the Http headers received, the following additional Name/Values pairs are added to simplify working with common HTTPHeaders
        /// isAjaxRequest = "true" (for X-Requested-With="XMLHttpRequest")
        /// </summary>
        /// <remarks>For a complete list of HTP Header fields see http://en.wikipedia.org/wiki/List_of_HTTP_header_fields</remarks>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        public override IValueProvider GetValueProvider(ControllerContext controllerContext) {
            NameValueCollection nameValueCollection = new NameValueCollection();
            NameValueCollection headers = controllerContext.HttpContext.Request.Headers;

            foreach (string key in headers.Keys) {
                nameValueCollection.Add(
                    Regex.Replace(key, "-", "") + "HttpHeader",
                    headers[key]
                );
                AddSimplifiedField(nameValueCollection, key, headers[key]);
            }

            return new NameValueCollectionValueProvider(nameValueCollection, CultureInfo.CurrentCulture);
        }

        private void AddSimplifiedField(NameValueCollection nameValueCollection, string key, string value) {
            switch (key) {
                case "X-Requested-With":
                    nameValueCollection.Add("isAjaxRequest", "true");
                    break;
            }
        }
    }
}
