using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Avantech.Common.Data.Entity
{
    /// <summary>
    /// A custom interface to get around the issue that Entity Frameworks DbContext.Entity
    /// returns the concrete, meaning it cannot be mocked
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDbEntityEntryProxy<TEntity>
        where TEntity : class
    {
        EntityState State { get; set; }
        DbPropertyEntry<TEntity, TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> property);
    }
}
