
using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc.Html
{
    public static class HubCssHtmlHelperExtensions
	{
        /// <summary>
        /// Create the list of css classes to be in the class attribute for a given list of OOCSS classes.
        /// e.g. 'grid-row-even' will return 'grid grid-row grid-row-even'.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="classesToApply">The OOCSS classes to be applied seperated by a space.</param>
        /// <returns></returns>
		public static MvcHtmlString HubCssAttribute(this HtmlHelper htmlHelper, string classesToApply)
		{
			var attributes = HubHtmlAttributeDictionary.GetClassList(classesToApply);
			return MvcHtmlString.Create(attributes[HubHtmlAttributeDictionary.CssClass].ToString());
		}

		/// <summary>
		/// Create the correctly populated class and data- attributes for a given list of OOCSS classes.
		/// e.g. 'grid-row-even' will return 'class="grid grid-row grid-row-even" data-oo-class="grid-row-even"'.
		/// </summary>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="classesToApply">The OOCSS classes to be applied seperated by a space.</param>
		/// <param name="classesToApply">Any non-OOCSS classes to be applied (eg for JS identifiers) seperated by a space.</param>
		/// <returns></returns>
		public static MvcHtmlString HubCssClass(this HtmlHelper htmlHelper, string classesToApply, string nonOoClasses = null)
		{
			var attributes = HubHtmlAttributeDictionary.GetClassList(classesToApply);

			string output = "class=\"" + attributes[HubHtmlAttributeDictionary.CssClass];
			
			if (!string.IsNullOrEmpty(nonOoClasses))
			{
				output = output + " " + nonOoClasses;
			}

			output = output + "\" data-oo-class=\"" + attributes[HubHtmlAttributeDictionary.DataOoClass] + "\"";

			return MvcHtmlString.Create(output);
		}
	}
}