namespace MyLibrary.Web.Mvc {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	public class HubDefaultModelBinder : DefaultModelBinder {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType) {
        	var typeToCreate = modelType;
			bool hasDefaultConstructor = false;
        	bool hasExplicitConstructors = false;
        	var requestParameters = controllerContext.HttpContext.Request.Params;
        	
			// TODO(RDS): No required with current architecture
			//// Check whether we can get an instance from dependency injection 
			//if (modelType.IsInterface) {
			//    object model = DependencyResolver.Current.GetService(modelType);
			//    if (model != null)
			//        return model;
			//}

            // we can understand some collection interfaces, e.g. IList<>, IDictionary<,>
            if (modelType.IsGenericType) {
                Type genericTypeDefinition = modelType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IDictionary<,>)) {
                    typeToCreate = typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
                }
                else if (genericTypeDefinition == typeof(IEnumerable<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IList<>)) {
                    typeToCreate = typeof(List<>).MakeGenericType(modelType.GetGenericArguments());
                }
            }

			// Check the request for constructor parameter values
        	var prefix = bindingContext.ModelName + ".";

			var completeConstructorArguments = new List<List<object>>();
			var constructors = modelType.GetConstructors();

			foreach(var constructor in constructors){
				var constructorArguments = new List<object>();

				var parameters = constructor.GetParameters();

				hasDefaultConstructor = parameters.Length == 0;
				hasExplicitConstructors = hasExplicitConstructors || parameters.Length > 0;
	
				foreach (var parameter in parameters) {
					var type = parameter.ParameterType;
					var name = parameter.Name;

					var parameterValue = requestParameters[prefix + name];

					if (parameterValue == null)
						break;

					constructorArguments.Add( 
						type.IsEnum
							? Enum.Parse(type, parameterValue)
							: Convert.ChangeType(parameterValue, type)
					);
				}

				if (parameters.Length != 0 && constructorArguments.Count == parameters.Length)
					completeConstructorArguments.Add(constructorArguments);
			}

			// Select Set of Constructor Arguments to use
			var effectiveConstructorArguments = completeConstructorArguments.SingleOrDefault(ca =>
				ca.Count() == completeConstructorArguments.Max(ca1 => ca1.Count())
        	);

			var effectiveConstructorArgumentsArray = effectiveConstructorArguments != null
				? effectiveConstructorArguments.ToArray()
        		: null;

			object modelInstance;

			try
			{
				modelInstance = Activator.CreateInstance(
					typeToCreate,
					effectiveConstructorArgumentsArray
				);
			}
			catch
			{
				string errorMessage = "The Model Binder is attempting to instantiate '{0}', but it does not have a default constructor{1}".FFormat(
					modelType.Name
					,"{0}"
				);

				if (!hasDefaultConstructor && hasExplicitConstructors)
				{
					errorMessage = errorMessage.FFormat(
						" and the request does not contain values that satisfy any of its explicit constructors{0}"
					);
				}

				throw new ApplicationException(
					errorMessage.FFormat(".")
				);
			}

        	return modelInstance;
		}
	}
}
