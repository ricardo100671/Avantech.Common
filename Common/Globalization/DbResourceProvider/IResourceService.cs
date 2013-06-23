namespace MyLibrary.Globalization.DbResourceProvider
{
	using System.Collections.Generic;

	public interface IResourceService
    {
        /// <summary>
        /// Sets the resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="resourceValue">The resource value.</param>
        /// <param name="cultureCode">The culture code.</param>
        void SetResource(string resourceType, string resourceKey, string resourceValue, string cultureCode = null);

        /// <summary>
        /// Deletes the resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture code.</param>
        void DeleteResource(string resourceType, string resourceKey, string cultureCode = null);

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns></returns>
        string GetResource(string resourceType, string resourceKey, string cultureCode = null);

        /// <summary>
        /// Gets the resource type list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetResourceTypeList();

        /// <summary>
        /// Gets the resource key list.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns></returns>
        IEnumerable<string> GetResourceKeyList(string resourceType);

        /// <summary>
        /// Unloads the resources.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        void UnloadResources(string resourceType = null);
    }
}