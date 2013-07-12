namespace Avantech.Common.Data.Entity
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Returns a custom DbEntityEntry that wraps Entity Framework's version
        /// because EF's version does not provide an interface and therefore cannot be mocked to facilitate testing.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="thisDbContext">The current Entity Framework Dbcontext.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static IDbEntityEntryProxy<TEntity> GetProxyEntry<TEntity>(this DbContext thisDbContext, TEntity entity)
            where TEntity : class
        {
            return new DbEntityEntryProxy<TEntity>(thisDbContext, entity);
        }
    }
}
