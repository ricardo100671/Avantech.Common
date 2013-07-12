using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Avantech.Common.Data.Entity
{
    using Avantech.Common.Practices;

    /// <summary>
    /// Base implementation for an Entity Framework based repository, 
    /// implementing standard repository functionality.
    /// </summary>
    /// <typeparam name="TEntity">The type of the aggregate root entity that the repository serves.</typeparam>
    public abstract class EfRepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected EfRepositoryBase(
            IDbSet<TEntity> dbSet,
            Func<TEntity, IDbEntityEntryProxy<TEntity>> getEntry
        )
        {
            if (dbSet == null) throw new ArgumentNullException("dbSet");
            if (getEntry == null) throw new ArgumentNullException("getEntry");

            _DbSet = dbSet;
            _GetEntry = getEntry;
        }

        readonly IDbSet<TEntity> _DbSet;
        protected IDbSet<TEntity> DbSet
        {
            get { return _DbSet; }
        }

        readonly Func<TEntity, IDbEntityEntryProxy<TEntity>> _GetEntry;
        protected IDbEntityEntryProxy<TEntity> GetEntry(TEntity entity)
        {
            return _GetEntry(entity);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return DbSet.AsQueryable();
        }

        public virtual Dictionary<int, string> GetIndex(
            Expression<Func<TEntity, bool>> predicate = null
        )
        {
            if (
               !typeof(IIndexableEntity<int>).IsAssignableFrom(typeof(TEntity))
           ) throw new InvalidOperationException("Entity does not support indexing. Implement IIndexableEntity in order to use this feature.");

            return DbSet
                .Where(predicate ?? (_ => true))
                .Select(e => new
                {
                    e.Id,
                    IndexName = ((IIndexableEntity<TEntity>)e).IndexingName
                })
                .ToDictionary(
                    _ => _.Id,
                    _ => _.IndexName
                );
        }

        public virtual TEntity Summerise(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> SummariseAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            int recordCount;
            return SummariseAll(
                0,
                int.MaxValue,
                out recordCount,
                predicate
            );
        }

        public virtual IEnumerable<TEntity> SummariseAll(
            int pageIndex,
            int pageSize,
            out int recordCount,
            Expression<Func<TEntity, bool>> predicate = null
        )
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Get(
            int id,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return Get(_ => _.Id.Equals(id), includes);
        }

        public virtual TEntity Get(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            var queryableSet = DbSet.AsQueryable();

            includes.ToList().ForEach(i =>
                queryableSet = DbExtensions.Include(queryableSet, i)
            );

            return queryableSet.Single(predicate);
        }

        public virtual IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            int recordCount;
            return GetAll(
                0,
                int.MaxValue,
                out recordCount,
                predicate,
                includes
            );
        }

        // TODO(RDS) Add OrderBy expression
        public virtual IEnumerable<TEntity> GetAll(
            int pageIndex,
            int pageSize,
            out int recordCount,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            recordCount = DbSet.Count(predicate ?? (o => true));

            var queryableSet = DbSet.AsQueryable()
                .Where(predicate ?? (o => true))
                .OrderBy(e => e.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            includes.ToList().ForEach(i =>
                queryableSet = DbExtensions.Include(queryableSet, i)
            );

            return queryableSet.ToList();
        }

        public virtual void Add(TEntity entity)
        {
            entity.Id = default(int);
            DbSet.Add(entity);
        }

        public virtual void Update(
            TEntity entity,
            params Expression<Func<TEntity, object>>[] modifiedProperties
        )
        {
            DbSet.Attach(entity);
            var entry = GetEntry(entity);

            if (!modifiedProperties.Any())
                entry.State = EntityState.Modified;

            modifiedProperties
                .ToList()
                .ForEach(mp =>
                    entry.Property(mp).IsModified = true
                );
        }

        public virtual void Delete(int id)
        {
            var entity = new TEntity
            {
                Id = id
            };

            DbSet.Attach(entity);

            DbSet.Remove(entity);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public virtual bool All(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.All(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }
    }
}
