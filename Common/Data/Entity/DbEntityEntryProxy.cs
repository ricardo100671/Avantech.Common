using System;
using System.Data;
using System.Linq.Expressions;

namespace Avantech.Common.Data.Entity
{
    /// <summary>
    /// Wrapper class for Entity Framework's DbEntityEntry
    /// that implements a custom interface that can be used as a return type
    /// when implementing Entitiy Framework contexts that implement, IEfContext, and passed into repositories
    /// This is so that methods of EF context can be mocked returning interfaces, that can also be mocked
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [Obsolete("Use DbEntityEntryProxy<TEntity>")]
    public class DbEntityEntryProxy<TEntity> : IDbEntityEntry<TEntity>
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly TEntity _entity;

        public DbEntityEntryProxy(DbContext context, TEntity entity)
        {
            _context = context;
            _entity = entity;
        }

        public EntityState State
        {
            get
            {
                return _context.Entry(_entity).State;
            }
            set
            {
                _context.Entry(_entity).State = value;
            }
        }

        public DbEntityEntryProxy<TEntity, TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            return _context.Entry(_entity).Property(property);
        }
    }
}
