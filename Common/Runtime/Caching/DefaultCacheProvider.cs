
using System;
using System.Runtime.Caching;

namespace Avantech.Common.Runtime.Caching
{
    public class DefaultCacheProvider<TCacheKeyEnum> : ICacheProvider<TCacheKeyEnum>
		where TCacheKeyEnum : struct
    {
        private static ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        #region ICacheProvider Members

        /// <summary>
        /// Gets the specified cached content for the supplied key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// Gets the specified cached content for the supplied key.
        /// </summary>
        /// <param name="keyEnum">The key.</param>
		public object Get(TCacheKeyEnum keyEnum)
        {
            return Get(keyEnum.ToString());
        }

        /// <summary>
        /// Sets the specified value in the cache.
        /// </summary>
        /// <param name="key">The key to cache the data against.</param>
        /// <param name="data">The data to be cached.</param>
        /// <param name="cacheTime">The cache time in minutes.</param>
        public void Set(string key, object data, int cacheTime)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Sets the specified value in the cache.
        /// </summary>
        /// <param name="keyEnum">The key to cache the data against.</param>
        /// <param name="data">The data to be cached.</param>
        /// <param name="cacheTime">The cache time in minutes.</param>
		public void Set(TCacheKeyEnum keyEnum, object data, int cacheTime)
        {
            Set(keyEnum.ToString(), data, cacheTime);
        }

        /// <summary>
        /// Determines whether the specified key is set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is set; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        /// <summary>
        /// Determines whether the specified key is set.
        /// </summary>
        /// <param name="keyEnum">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is set; otherwise, <c>false</c>.
        /// </returns>
		public bool IsSet(TCacheKeyEnum keyEnum)
        {
            return IsSet(keyEnum.ToString());
        }

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="keyEnum">The key.</param>
		public void Invalidate(TCacheKeyEnum keyEnum)
        {
            Invalidate(keyEnum.ToString());
        }

        #endregion
    }
}
