
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Avantech.Common.Web.Mvc
{
    /// <summary>
	/// Provide functionality to allow parameter values of action methods to become sticky. 
	/// i.e. Parameter values provided in the first call to the actiopn, do not have to be specified on subsequent calls
	/// as the original values are held in Session. 
	/// Has the following effects on each respective parameter type:
	///		Non-Nullable: A value must be provided on initial call and will be used untill the session expires or a new value is provided in the request
	///			Attempting to set the value to 'null' will cause an exception
	///		Optional Non-Nullable: If a value is specified on initial call, it will be used untill the session expires or a new value is provided in the request
	///			If a value is not specified on initial call, the default value will be used untill the session expires or a new value is provided in the request
	///			Attempting to set the value to null will cause an exception
	///		Nullable: If a value is specified on the initial call, it will be used untill the session expires or a new value is provided in the request
	///			If a value is not specified on initial call, the default value will be used untill the session expires or a new value is provided in the request
	///			Value may be set to null
	///		Optional Nullable: If a value is specified on the initial call, it will be used untill the session expires, a new value is provided in the request or its value set to 'null'
	///			If a value is not specified on initial call, the default value will be used untill the session expires, a new value is provided in the request or its value set to 'null'
	///			Setting the value to 'null' will revert its value back to its default and used untill the session expires or a new value is provided in the request
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CacheParametersAttribute : ActionFilterAttribute 
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheParametersAttribute"/> class.
		/// </summary>
		/// <param name="excludedParameters">Parameter names to exclude from sticky behaviour.</param>
		public CacheParametersAttribute(params string[] excludedParameters) {
			_excludedParameters = excludedParameters;
		}

		private readonly string[] _excludedParameters;
		public IEnumerable<string> ExcludedParameters {
			get {
				return _excludedParameters;
			}
		}


		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			var sessionKeyPrefix = "{0}_{1}_StickyParameter_".FFormat(
				filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
				filterContext.ActionDescriptor.ActionName
			);
			var httpContext = filterContext.HttpContext;
			var store = httpContext.Session;

			if (store == null)
				throw new ApplicationException("Session is not available to use as cache store.");

			filterContext.ActionParameters
				.ToList()
				.ForEach(ap => {
					var descriptor = filterContext.ActionDescriptor.GetParameters().Single(pd => pd.ParameterName == ap.Key);

					filterContext.ActionParameters[ap.Key] = (ExcludedParameters.Contains(ap.Key)
						? ap.Value
						: httpContext.Request.Params.AllKeys
							.Where(k => k != null)
							.Any(k =>
								k.ToLower() == ap.Key.ToLower() // Was the parameter posted as a single key value pair
								|| k.ToLower() + "_" == ap.Key.ToLower() + "_" // If the parameter is a complex type, the properties will be prefixed with the name of the parameter followed by an underscore
							)
								? (store[sessionKeyPrefix + ap.Key] = (ap.Value != null && ap.Value.ToString() == "null"
									? descriptor.DefaultValue
									: ap.Value
									))
								: store[sessionKeyPrefix + ap.Key] ?? ap.Value
						);
				});

			base.OnActionExecuting(filterContext);
		}
	}
}
