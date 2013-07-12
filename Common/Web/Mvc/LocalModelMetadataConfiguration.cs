using System;
using System.Web.Mvc;
using MvcExtensions;

namespace Avantech.Common.Web.Mvc
{
    public class LocalModelMetadataConfigurationAttribute : ActionFilterAttribute
    {
        readonly Type _ModelMetadataConfigurationType;

        public LocalModelMetadataConfigurationAttribute(Type modelMetadataConfigurationType)
        {
            if (modelMetadataConfigurationType == null) throw new ArgumentNullException("modelMetadataConfigurationType");

            _ModelMetadataConfigurationType = modelMetadataConfigurationType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelMetadataConfiguration = (IModelMetadataConfiguration)Activator.CreateInstance(_ModelMetadataConfigurationType, null);

            ((ModelMetadataProvider)ModelMetadataProviders.Current)
                .Merge(modelMetadataConfiguration);

            base.OnActionExecuting(filterContext);
        }
    }
}