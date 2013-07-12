
using System;
using System.Web.Mvc;
using System.Web.UI;

namespace Avantech.Common.Web.Mvc.Html
{
    public static class UrlHelperExtensions {
		public static string WebResource(this UrlHelper urlHelper, Type type, string resourcePath)
		{
			using(var page = new Page()) {
				return Page.ClientScript.GetWebResourceUrl(type, resourcePath);
			}
		}

		private static Page Page {
			get { return _Page ?? (_Page = new Page()); }
		}
		private static Page _Page;


	}
}
