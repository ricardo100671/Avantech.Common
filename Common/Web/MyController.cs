namespace MyLibrary.Web
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Web.Mvc;
	using Linq.Expressions;
	using MySystem.Web.Mvc;

	public class MyController : Controller
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

		MyViewDataDictionary _ViewData;
		new public MyViewDataDictionary ViewData
		{
			get
			{
				return _ViewData 
					?? (_ViewData = new MyViewDataDictionary());
			}
			set
			{
				_ViewData = value;
			}
		}
	}
}
