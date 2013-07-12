

namespace Avantech.Common.Runtime.Caching
{
	public interface ICacheProvider<in TCacheKeyEnum>
		where TCacheKeyEnum : struct
    {
        /// <summary>
        /// Gets the specified cached content for the supplied key.
        /// </summary>
        /// <param name="key">The key.</param>
        object Get(string key);

        /// <summary>
        /// Gets the specified cached content for the supplied key.
        /// </summary>
        /// <param name="key">The key.</param>
		object Get(TCacheKeyEnum key);

        /// <summary>
        /// Sets the specified value in the cache.
        /// </summary>
        /// <param name="key">The key to cache the data against.</param>
        /// <param name="data">The data to be cached.</param>
        /// <param name="cacheTime">The cache time in minutes.</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Sets the specified value in the cache.
        /// </summary>
        /// <param name="key">The key to cache the data against.</param>
        /// <param name="data">The data to be cached.</param>
        /// <param name="cacheTime">The cache time in minutes.</param>
		void Set(TCacheKeyEnum key, object data, int cacheTime);

        /// <summary>
        /// Determines whether the specified key is set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is set; otherwise, <c>false</c>.
        /// </returns>
        bool IsSet(string key);

        /// <summary>
        /// Determines whether the specified key is set.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is set; otherwise, <c>false</c>.
        /// </returns>
		bool IsSet(TCacheKeyEnum key);

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Invalidate(string key);

        /// <summary>
        /// Invalidates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
		void Invalidate(TCacheKeyEnum key);
    }
}
