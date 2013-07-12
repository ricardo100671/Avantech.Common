
using System;
using System.Collections.Generic;
using System.Threading;

namespace Avantech.Common.Globalization.DbResourceProvider
{
    public class ResourceService : IResourceService
    {
        internal const string InvariantCultureCode = "Invariant";

        /// <summary>
        /// Sets the resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="resourceValue">The resource value.</param>
        /// <param name="cultureCode">The culture code.</param>
        public void SetResource(string resourceType, string resourceKey, string resourceValue, string cultureCode = null)
        {
            if (resourceType == null) throw new ArgumentNullException("resourceType");
            if (resourceKey == null) throw new ArgumentNullException("resourceKey");
            if (resourceValue == null) throw new ArgumentNullException("resourceValue");
            if (cultureCode == null) cultureCode = InvariantCultureCode;

            DbResourceManager.SetResource(resourceType, resourceKey, resourceValue, cultureCode);
        }

        /// <summary>
        /// Deletes the resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture code.</param>
        public void DeleteResource(string resourceType, string resourceKey, string cultureCode = null)
        {
            if (resourceType == null) throw new ArgumentNullException("resourceType");
            if (resourceKey == null) throw new ArgumentNullException("resourceKey");
            if (cultureCode == null) cultureCode = InvariantCultureCode;

            DbResourceManager.DeleteResource(resourceType, resourceKey, cultureCode);
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns></returns>
        public string GetResource(string resourceType, string resourceKey, string cultureCode = null)
        {
            if (resourceType == null) throw new ArgumentNullException("resourceType");
            if (resourceKey == null) throw new ArgumentNullException("resourceKey");
            if (cultureCode == null) cultureCode = Thread.CurrentThread.CurrentCulture.ToString();

            return DbResourceManager.GetResource(resourceType, resourceKey, cultureCode);
        }

        /// <summary>
        /// Gets the resource type list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetResourceTypeList()
        {
            return DbResourceManager.GetResourceTypeList();
        }

        /// <summary>
        /// Gets the resource key list.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns></returns>
        public IEnumerable<string> GetResourceKeyList(string resourceType)
        {
            if (resourceType == null) throw new ArgumentNullException("resourceType");
            return DbResourceManager.GetResourceKeyList(resourceType);
        }

        /// <summary>
        /// Unloads the resources.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        public void UnloadResources(string resourceType = null)
        {
            DbResourceManager.UnloadProviders(resourceType);
        }
    }
}
