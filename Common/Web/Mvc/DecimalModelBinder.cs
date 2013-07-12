
using System;
using System.Globalization;
using System.Web.Mvc;

namespace Avantech.Common.Web.Mvc
{
    /// <summary>
	/// The Default model binder parses request variable with invariant culture and form variable with the current culture.
	/// This causes problems when decimal values that are posted, since some cultures use commas as periods,
	/// causing the values to fail to bind correctly when sent via ajax request.
	/// This model binder corrects that behaviour
	/// </summary>
	public class HubDecimalModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			if (controllerContext == null)
				throw new ArgumentNullException("controllerContext", "controllerContext is null.");
			if (bindingContext == null)
				throw new ArgumentNullException("bindingContext", "bindingContext is null.");
 
			var modelType = bindingContext.ModelType;
			object result = (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(Nullable<>))
				? (decimal?)null
				: default(decimal);

			var currencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
			var percentageSymbol = CultureInfo.CurrentCulture.NumberFormat.PercentSymbol;

			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			string value = valueProviderResult.AttemptedValue
				.Replace(currencySymbol," ")
				.Replace(percentageSymbol, " ")
				.Trim();

			if (value.Length != 0) {
				try
				{
					result = decimal.Parse(
						value,
						CultureInfo.CurrentCulture.NumberFormat
					);				
				}
				catch(Exception exception) {
					bindingContext.ModelState.AddModelError(bindingContext.ModelName, exception);
					bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
				}
			}
			
			return result;
		}
	}
}
