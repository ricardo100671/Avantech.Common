namespace MySystem.Web.Mvc.Html
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Web.Handlers;
	using System.Web.Mvc;

	public static class HtmlHelperExtensions
	{
		public static MvcHtmlString DropDownList
		(
			this HtmlHelper htmlHelper,
			string name,
			IDictionary<int, string> options = null,
			string optionLabel = null,
			string cssClass = null,
			string dir = null,
			bool disabled = false,
			string id = null,
			string lang = null,
			int? size = null,
			string style = null,
			int? tabIndex = null,
			string title = null
		)
		{
			return Microsoft.Web.Mvc.Html.HtmlHelperExtensions.DropDownList
			(
				htmlHelper,
				name,
				ConvertToSelecList(options),
				optionLabel,
				cssClass,
				dir,
				disabled,
				id,
				lang,
				size,
				style,
				tabIndex,
				title
			);

		}

		public static MvcHtmlString DropDownListFor<TModel, TProperty>
		(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IDictionary<int, string> options = null,
			string optionLabel = null,
			string cssClass = null,
			string dir = null,
			bool disabled = false,
			string id = null,
			string lang = null,
			int? size = null,
			string style = null,
			int? tabIndex = null,
			string title = null
		)
		{
			return Microsoft.Web.Mvc.Html.HtmlHelperExtensions.DropDownListFor(
				htmlHelper,
				expression,
				ConvertToSelecList(options),
				optionLabel,
				cssClass,
				dir,
				disabled,
				id,
				lang,
				size,
				style,
				tabIndex,
				title
			);
		}

		public static MvcHtmlString DropDownListFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper, 
			Expression<Func<TModel, TProperty>> expression, 
			IDictionary<int, string> options, 
			IDictionary<string,object> htmlAttributes
		)
		{
			return System.Web.Mvc.Html.SelectExtensions.DropDownListFor(
				htmlHelper, 
				expression, 
				ConvertToSelecList(options), 
				htmlAttributes
			);
		}

		public static MvcHtmlString ListBox
		(
			this HtmlHelper htmlHelper,
			string name,
			IDictionary<int, string> options = null,
			string cssClass = null,
			string dir = null,
			bool disabled = false,
			string id = null,
			string lang = null,
			int? size = null,
			string style = null,
			int? tabIndex = null,
			string title = null
		)
		{
			return Microsoft.Web.Mvc.Html.HtmlHelperExtensions.ListBox
			(
				htmlHelper,
				name,
				ConvertToSelecList(options),
				cssClass,
				dir,
				disabled,
				id,
				lang,
				size,
				style,
				tabIndex,
				title
			);
		}

		public static MvcHtmlString ListBoxFor<TModel, TProperty>
		(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IDictionary<int, string> options = null,
			string cssClass = null,
			string dir = null,
			bool disabled = false,
			string id = null,
			string lang = null,
			int? size = null,
			string style = null,
			int? tabIndex = null,
			string title = null
		)
		{
			return Microsoft.Web.Mvc.Html.HtmlHelperExtensions.ListBoxFor<TModel, TProperty>
				(
					htmlHelper,
					expression,
					ConvertToSelecList(options),
					cssClass,
					dir,
					disabled,
					id,
					lang,
					size,
					style,
					tabIndex,
					title
				);
		}

		private static IEnumerable<SelectListItem> ConvertToSelecList(IDictionary<int, string> options)
		{
			return options
				.Select(o =>
					new SelectListItem()
					{
						Text = o.Value,
						Value = o.Key.ToString()
					}
				);
		}

	private static MethodInfo getWebResourceUrlMethod;
    private static object getWebResourceUrlLock = new object();
    public static string WebResourceUrl<T>(this HtmlHelper htmlHelper, string resourceName) {
		if (string.IsNullOrEmpty(resourceName)) throw new ArgumentNullException("resourceName");

		if (getWebResourceUrlMethod == null) {
			lock (getWebResourceUrlLock)
			{

				if (getWebResourceUrlMethod == null) {
					getWebResourceUrlMethod = typeof(AssemblyResourceLoader).GetMethod(
						"GetWebResourceUrlInternal",
						BindingFlags.NonPublic | BindingFlags.Static);
					}
				}
			}

			return "/" + (string) getWebResourceUrlMethod.Invoke(
				null,
				new object[] { Assembly.GetAssembly(typeof(T)), resourceName, false }
			);

			}

	}
}
