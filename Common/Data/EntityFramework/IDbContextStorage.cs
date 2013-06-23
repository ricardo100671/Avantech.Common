namespace MyLibrary.Data.EntityFramework
{
	using System.Collections.Generic;
	using System.Data.Entity;

	/// <summary>
    /// Stores object context
    /// </summary>
    public interface IDbContextStorage
    {
        /// <summary>
        /// Gets the db context for key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        DbContext GetDbContextForKey(string key);

        /// <summary>
        /// Sets the db context for key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="objectContext">The object context.</param>
        void SetDbContextForKey(string key, DbContext objectContext);

        /// <summary>
        /// Gets all db contexts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DbContext> GetAllDbContexts();
        
        /// <summary>
        /// Disposes DbContexts
        /// </summary>
        void DisposeAll();

        /// <summary>
        /// Disposes the DbContext related with the key
        /// </summary>
        void Dispose(string key);
    }
}
