namespace MyLibrary.Globalization.DbResourceProvider
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Data;

	internal static class DbResourceManager
    {
        //Hold a static list of resource providers
        private static readonly List<DbResourceProvider> LoadedProviders = new List<DbResourceProvider>();

        internal static void AddProvider(DbResourceProvider dbResourceProvider)
        {
            LoadedProviders.Add(dbResourceProvider);
        }

        public static void UnloadProviders(string resourceType = null)
        {
            if (resourceType == null)
            {
                //We want to unload all providers
                foreach (var dbResourceProvider in LoadedProviders)
                {
                    dbResourceProvider.ClearResourceCache();
                }
            }
            else
            {
                //We want to unload a specific provider
                foreach (var loadedProvider in LoadedProviders.Where(loadedProvider => loadedProvider.ResourceType.Equals(resourceType)))
                {
                    loadedProvider.ClearResourceCache();
                }
            }
        }

        public static string GetResource(string resourceType, string resourceKey, string cultureCode)
        {
            return GetProvider(resourceType).GetResource(cultureCode, resourceKey);
        }

        /// <summary>
        /// Adds or updates the specific resource value. If culture code is omitted then the invariant culture is set.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="resourceValue">The resource value.</param>
        /// <param name="cultureCode">The culture code.</param>
        public static void SetResource(string resourceType, string resourceKey, string resourceValue, string cultureCode)
        {
            //Add it to the provider cache
            GetProvider(resourceType).SetResource(resourceKey, resourceValue, cultureCode);

            //Add it to the database
            using (var dbResourceContext = new DbResourceContext())
            {
                //Check if resource already exists
                var resource = dbResourceContext.Resources.FirstOrDefault(x => x.ResourceType == resourceType && x.ResourceKey == resourceKey && x.CultureCode == cultureCode);

                if (resource == null)
                {
                    //New resource so add
                    resource = new DbResource();
                    resource.ResourceType = resourceType;
                    resource.CultureCode = cultureCode;
                    resource.ResourceKey = resourceKey;
                    resource.ResourceValue = resourceValue;
                    resource.LastUpdatedDate = DateTime.UtcNow;
                    dbResourceContext.Resources.Add(resource);
                }
                else
                {
                    resource.ResourceValue = resourceValue;
                    resource.LastUpdatedDate = DateTime.UtcNow;
                }
                dbResourceContext.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the resource.
        /// </summary>
        /// <param name = "resourceType">Type of the resource.</param>
        /// <param name = "resourceKey">The resource key.</param>
        /// <param name = "cultureCode">The culture code.</param>
        public static void DeleteResource(string resourceType, string resourceKey, string cultureCode)
        {
            //Get the relevant resource type provider
            var provider = GetProvider(resourceType);
            using (var dbResourceContext = new DbResourceContext())
            {

                if (cultureCode == ResourceService.InvariantCultureCode)
                {
                    //We're removing the invariant so remove all specific resources too
                    var cultureCodes = GetCultureCodeCoverage(resourceType, resourceKey);

                    foreach (var code in cultureCodes)
                    {
                        //Remove from provider cache and database
                        provider.RemoveResource(resourceKey, code);
                        RemoveResourceFromDatabase(dbResourceContext, resourceType, resourceKey, code);
                    }
                }
                else
                {
                    //Remove specified culture from provider cache and database
                    provider.RemoveResource(resourceKey, cultureCode);
                    RemoveResourceFromDatabase(dbResourceContext, resourceType, resourceKey, cultureCode);
                }

                dbResourceContext.SaveChanges();
            }
        }


        public static IEnumerable<string> GetResourceTypeList()
        {
            using (var dbResourceContext = new DbResourceContext())
            {
                return dbResourceContext.Resources.Select(x => x.ResourceType).Distinct().OrderBy(x => x).ToList();
            }
        }

        public static IEnumerable<string> GetResourceKeyList(string resourceType)
        {
            using (var dbResourceContext = new DbResourceContext())
            {
                return dbResourceContext.Resources.Where(x => x.ResourceType == resourceType).Select(x => x.ResourceKey).Distinct().OrderBy(x => x).ToList();
            }

        }

        /// <summary>
        ///   Gets the culture code coverage.
        /// </summary>
        /// <param name = "resourceType">Type of the resource.</param>
        /// <param name = "resourceKey">The resource key.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetCultureCodeCoverage(string resourceType, string resourceKey)
        {
            using (var db = new DbResourceContext())
            {
                return db.Resources.Where(x => x.ResourceType == resourceType && x.ResourceKey == resourceKey).Select(x => x.CultureCode).Distinct().ToList();
            }
        }

        /// <summary>
        /// Gets the db resource provider for the specified resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns></returns>
        private static DbResourceProvider GetProvider(string resourceType)
        {
            if (LoadedProviders == null)
                throw new NullReferenceException("LoadedProviders cannot be null");

            if (LoadedProviders.Exists(x => x.ResourceType.Equals(resourceType)))
            {
                return LoadedProviders.Find(x => x.ResourceType.Equals(resourceType));
            }
            else
            {
                //We need create and add a provider to the collection of providers
                var dbResourceProvider = new DbResourceProvider(resourceType);
                AddProvider(dbResourceProvider);
                return dbResourceProvider;
            }
        }


        /// <summary>
        /// Gets the resource dictionary.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns></returns>
        internal static IDictionary GetResourceDictionary(string resourceType, string cultureCode)
        {
            if (resourceType == null) throw new ArgumentNullException("resourceType");

            using (var dbResourceContext = new DbResourceContext())
            {
                return dbResourceContext.Resources.Where(x => x.ResourceType == resourceType && x.CultureCode == cultureCode).AsEnumerable().ToDictionary(k => k.ResourceKey, v => v.ResourceValue);
            }
        }

        #region Database Helper Methods

        private static void RemoveResourceFromDatabase(DbResourceContext dbResourceContext, string resourceType, string resourceKey, string cultureCode)
        {
            //Get the resource
            var resource = dbResourceContext.Resources.FirstOrDefault(x => x.ResourceType == resourceType && x.ResourceKey == resourceKey && x.CultureCode == cultureCode);

            if (resource != null)
            {
                var resourceValue = resource.ResourceValue;
                SetResourcePrimer(dbResourceContext, resourceType, resourceKey, resourceValue, cultureCode);
                dbResourceContext.Resources.Remove(resource);
            }
        }

        internal static string GetResourcePrimer(string resourceType, string resourceKey, string cultureCode)
        {
            using (var db = new DbResourceContext())
            {
                return db.ResourcePrimers.Where(x => x.ResourceType == resourceType && x.ResourceKey == resourceKey && x.CultureCode == cultureCode).Select(x => x.ResourceValue).FirstOrDefault();
            }
        }

        private static void SetResourcePrimer(DbResourceContext dbResourceContext, string resourceType, string resourceKey, string resourceValue, string cultureCode)
        {
            //We need to see if we have a primer value to update or add
            var resourcePrimer = dbResourceContext.ResourcePrimers.FirstOrDefault(x => x.ResourceType == resourceType && x.ResourceKey == resourceKey && x.CultureCode == cultureCode);

            if (resourcePrimer == null)
            {
                resourcePrimer = new DbResourcePrimer();
                resourcePrimer.ResourceType = resourceType;
                resourcePrimer.ResourceKey = resourceKey;
                resourcePrimer.CultureCode = cultureCode;
                resourcePrimer.ResourceValue = resourceValue;

                dbResourceContext.ResourcePrimers.Add(resourcePrimer);
            }
            else
            {
                resourcePrimer.ResourceValue = resourceValue;
            }
        }
        
        #endregion


    }
}
