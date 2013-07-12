using System;
using System.Linq;
using MvcExtensions;

namespace Avantech.Common.Web.Mvc
{
    public class ModelMetadataProvider : ExtendedModelMetadataProvider
    {
        readonly IModelMetadataRegistry _Registry;

        public ModelMetadataProvider(IModelMetadataRegistry registry)
            : base(registry)
        {
            if (registry == null) throw new ArgumentNullException("registry");

            _Registry = registry;
        }

        /// <summary>
        /// Merges the specified <see cref="MvcExtensions.ModelMetadataConfiguration{TModel}"/> with existing metadata 
        /// that may have been specified via attributes or via model metadata configurartion classes.
        /// </summary>
        /// <param name="metaDataConfig">The meta data configuration to be merged.</param>
        public void Merge(IModelMetadataConfiguration metaDataConfig)
        {
            var configurations = metaDataConfig.Configurations;

            configurations
                .ToList()
                .ForEach(c =>
                {
                    var currentMetadata = _Registry.GetModelPropertiesMetadata(metaDataConfig.ModelType);

                    if (currentMetadata == null)
                    {
                        _Registry.RegisterModelProperties(metaDataConfig.ModelType, configurations);
                        return;
                    }
                    c.Value.MergeTo(
                        currentMetadata[c.Key]
                    );
                }
            );
        }
    }
}
