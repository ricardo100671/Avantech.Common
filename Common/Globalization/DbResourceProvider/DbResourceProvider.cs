namespace MyLibrary.Globalization.DbResourceProvider
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Globalization;
	using System.Resources;
	using System.Web.Compilation;

	internal sealed class DbResourceProvider : IResourceProvider
    {
        private IDictionary _resourceCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbResourceProvider"/> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        public DbResourceProvider(string resourceType)
        {
            ResourceType = resourceType;
            DbResourceManager.AddProvider(this);
        }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        internal string ResourceType { get; private set; }

        /// <summary>
        /// Returns a resource object for the key and culture.
        /// </summary>
        /// <param name="resourceKey">The key identifying a particular resource.</param>
        /// <param name="culture">The culture identifying a localized value for the resource.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that contains the resource value for the <paramref name="resourceKey"/> and <paramref name="culture"/>.
        /// </returns>
        object IResourceProvider.GetObject(string resourceKey, CultureInfo culture)
        {
            //set culture name to specificly passed culture or use current ui culture as fallback
            var cultureCode = culture != null ? culture.Name : CultureInfo.CurrentUICulture.Name;

            return GetResource(cultureCode, resourceKey);
        }

        /// <summary>
        /// Gets an object to read resource values from a source.
        /// </summary>
        /// <returns>The <see cref="T:System.Resources.IResourceReader"/> associated with the current resource provider.</returns>
        IResourceReader IResourceProvider.ResourceReader
        {
            get { return new DbResourceReader(GetResourceCache(ResourceService.InvariantCultureCode)); }
        }

        /// <summary>
        ///   Clears the resource cache for this provider instance.
        /// </summary>
        internal void ClearResourceCache()
        {
            _resourceCache.Clear();
        }

        /// <summary>
        /// Gets the resource value.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        internal string GetResource(string cultureCode, string resourceKey)
        {
            //Try to return specific localisation
            if (GetResourceCache(cultureCode).Contains(resourceKey))
                return GetResourceCache(cultureCode)[resourceKey].ToString();

            //Try to return Invariant localisation
            if (!cultureCode.Equals(ResourceService.InvariantCultureCode) && GetResourceCache(ResourceService.InvariantCultureCode).Contains(resourceKey))
                return GetResourceCache(ResourceService.InvariantCultureCode)[resourceKey].ToString();

            //We didn't find a localisation so see if we have a specific one in primers
            var resourceValue = DbResourceManager.GetResourcePrimer(ResourceType, resourceKey, cultureCode);

            if (resourceValue == null)
            {
                //We didn't find a specific primer so check invariant primers
                resourceValue = DbResourceManager.GetResourcePrimer(ResourceType, resourceKey, ResourceService.InvariantCultureCode);

                if (resourceValue == null)
                {
                    //we didn't find any primer so create a surrogate invariant and add it to resource table and cache
                    var surrogateValue = string.Format("[{0}-{1}]", ResourceType, resourceKey);
                    DbResourceManager.SetResource(ResourceType, resourceKey, surrogateValue, ResourceService.InvariantCultureCode);
                    resourceValue = surrogateValue;
                }
                else
                {
                    //found invariant value in primers so add it to resource table and cache
                    DbResourceManager.SetResource(ResourceType, resourceKey, resourceValue, ResourceService.InvariantCultureCode);
                }
            }
            else
            {
                //found specific value in primers so add it to resource table and cache
                DbResourceManager.SetResource(ResourceType, resourceKey, resourceValue, cultureCode);
            }

            return resourceValue;
        }

        /// <summary>
        /// Sets the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="resourceValue">The resource value.</param>
        /// <param name="cultureCode">The culture code.</param>
        internal void SetResource(string resourceKey, string resourceValue, string cultureCode)
        {
            if (GetResourceCache(cultureCode).Contains(resourceKey))
                GetResourceCache(cultureCode)[resourceKey] = resourceValue;
            else
                GetResourceCache(cultureCode).Add(resourceKey, resourceValue);
        }

        /// <summary>
        /// Removes the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture code.</param>
        internal void RemoveResource(string resourceKey, string cultureCode)
        {
            if (GetResourceCache(cultureCode).Contains(resourceKey))
                GetResourceCache(cultureCode).Remove(resourceKey);
        }

        /// <summary>
        /// Gets the resource cache from memory if available otherwise builds it from the db.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns></returns>
        private IDictionary GetResourceCache(string cultureCode)
        {
            if (_resourceCache == null)
                _resourceCache = new ListDictionary();

            var resourceDict = _resourceCache[cultureCode] as IDictionary;
            if (resourceDict == null)
            {
                resourceDict = DbResourceManager.GetResourceDictionary(ResourceType, cultureCode);
                _resourceCache[cultureCode] = resourceDict;
            }
            return resourceDict;
        }

        #region Nested type: DbResourceReader

        private sealed class DbResourceReader : IResourceReader
        {
            private readonly IDictionary _resources;

            public DbResourceReader(IDictionary resources)
            {
                this._resources = resources;
            }

            IDictionaryEnumerator IResourceReader.GetEnumerator()
            {
                return _resources.GetEnumerator();
            }

            void IResourceReader.Close()
            {
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _resources.GetEnumerator();
            }

            void IDisposable.Dispose()
            {
            }

        }

        #endregion
    }
}
