
using System.IO;
using System.Web.Compilation;

namespace Avantech.Common.Globalization.DbResourceProvider
{
    /// <remarks>
    ///   Runtime resource provider using a Database.
    /// </remarks>
    internal sealed class DbResourceProviderFactory : ResourceProviderFactory
    {
        /// <summary>
        /// Creates a new local resource provider.
        /// </summary>
        /// <param name="virtualPath">The virtual path of the local provider.</param>
        /// <returns></returns>
        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            var resourceType = Path.GetFileName(virtualPath);
            return new DbResourceProvider(resourceType);
        }

        /// <summary>
        /// Creates a new global resource provider.
        /// </summary>
        /// <param name="resourceType">The resource type of the global provider.</param>
        /// <returns></returns>
        public override IResourceProvider CreateGlobalResourceProvider(string resourceType)
        {
            return new DbResourceProvider(resourceType);
        }
    }
}
