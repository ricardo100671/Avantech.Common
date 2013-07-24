using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Avantech.Common.Web
{
    using Avantech.Common.Linq.Expressions;
    using Avantech.Common.Web.Mvc;

    public class Controller : System.Web.Mvc.Controller
	{
		/// <summary>
		/// Updates the model from registered Value Providers excluding the specified properties.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="model">The model.</param>
		/// <param name="properties">The properties.</param>
		protected void UpdateModelExcluding<TModel>(TModel model, params Expression<Func<TModel, object>>[] properties) where TModel : class
		{
			var propertyNames = new List<string>();

			properties
				.ToList()
				.Aggregate(
					propertyNames,
					(epn, ex) => {
						epn.Add(ex.ToText());
						return epn;
					}
				);

			UpdateModel(model,
				String.Empty,
				null,
				propertyNames.ToArray(),
				ValueProvider
			);
		}

		/// <summary>
		/// Updates the model from registered Value Providers for only the specified properties.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="model">The model.</param>
		/// <param name="properties">The properties.</param>
		protected void UpdateModelIncluding<TModel>(TModel model, params Expression<Func<TModel, object>>[] properties) where TModel : class
		{
			var propertyNames = new List<string>();

			properties
				.ToList()
				.Aggregate(
					propertyNames,
					(epn, ex) => {
						epn.Add(ex.ToText());
						return epn;
					}
				);


			UpdateModel(model,
				String.Empty,
				propertyNames.ToArray(),
				null,
				ValueProvider
			);
		}

		ViewDataDictionary _ViewData;
		new public ViewDataDictionary ViewData
		{
			get
			{
				return _ViewData 
					?? (_ViewData = new ViewDataDictionary());
			}
			set
			{
				_ViewData = value;
			}
		}
	}
}
