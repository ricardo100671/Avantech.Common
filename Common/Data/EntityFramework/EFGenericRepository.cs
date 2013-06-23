namespace MyLibrary.Data.EntityFramework
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Entity;
	using System.Data.Entity.Design.PluralizationServices;
	using System.Data.Entity.Infrastructure;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using Specification;

	/// <summary>
    /// Generic repository
    /// </summary>
    public class EFGenericRepository : IRepository, IRepositoryConfiguration, IDisposable {
        #region Member Fields

        private readonly string _connectionStringName;
        private IUnitOfWork _unitOfWork;
        private readonly PluralizationService _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
        #endregion Member Fields

        #region Constructors
        public EFGenericRepository()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public EFGenericRepository(string connectionStringName)
        {
            this._connectionStringName = connectionStringName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EFGenericRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            _context = context;
        }
        #endregion Constructors
        
        #region .Add
        public void Add<TEntity>(TEntity entity, bool applyToStore = false) where TEntity : class {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            DbContext.Set<TEntity>().Add(entity);

            if (applyToStore)
                DbContext.SaveChanges();
        }
        #endregion .Add

        #region .Update
        public void Update<TEntity>(
			TEntity entity, 
			TEntity originalEntity,
			params Expression<Func<TEntity, object>>[] modifiedProperties
		) where TEntity : class {
        	Update(
        		entity,
        		modifiedProperties
        	);

			DbContext.Entry(entity).OriginalValues.SetValues(originalEntity);
        }

		public void Update<TEntity>(
			TEntity entity,
			params Expression<Func<TEntity, object>>[] modifiedProperties
		) where TEntity : class {
			if (DbContext.Entry(entity).State == EntityState.Detached)
				DbContext.Set<TEntity>().Attach(entity);

			if (modifiedProperties.Any())
				modifiedProperties
					.ToList()
					.ForEach(mp =>
						DbContext.Entry(entity).Property(mp).IsModified = true
					);
			else
				DbContext.Entry(entity).State = EntityState.Modified;

			// TODO: Add logic to inspect navigation and enumerable properties of conmplex types that may be entit
			// to perform insert for new objects and update for existing ones
        }
		#endregion .Update

        #region .GetByKey
        public TEntity GetByKey<TEntity>(params object[] keyValues) where TEntity : class {
            EntityKey key = GetEntityKeyObject<TEntity>(keyValues);

            return (TEntity)((IObjectContextAdapter)DbContext).ObjectContext.GetObjectByKey(key);
        }
        #endregion .GetByKey

        #region .TryGetByKey
        public bool TryGetByKey<TEntity>(out TEntity entity, params object[] keyValues) where TEntity : class {
            EntityKey key = GetEntityKeyObject<TEntity>(keyValues);

            object retrievedEntity = null;
            var result = ((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out retrievedEntity);
            entity = (TEntity)retrievedEntity;
            return result;
        }
        #endregion .TryGetByKey
        
        #region .GetQuery
        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            var entityName = GetEntityName<TEntity>();
            return ((IObjectContextAdapter)DbContext).ObjectContext.CreateQuery<TEntity>(entityName);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return GetQuery<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>());
        }
        #endregion .GetQuery

        #region .Get
        public IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class {
            return GetQuery<TEntity>().Where(criteria);
        }

        public IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, string>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>().Where(predicate).OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>().Where(predicate).OrderByDescending(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity>(ISpecification<TEntity> criteria) where TEntity : class {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity>(ISpecification<TEntity> specification, int pageIndex, int pageSize, Expression<Func<TEntity, string>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
            }
            return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderByDescending(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
        }
        #endregion .Get

        #region .GetAll
        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class {
            return GetQuery<TEntity>().AsEnumerable();
        }

        public IEnumerable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, string>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class {
            if (sortOrder == SortOrder.Ascending) {
                return GetQuery<TEntity>().OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>().OrderByDescending(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
        }
        #endregion .GetAll
        
        #region .Single
        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Single<TEntity>(criteria);
        }

        public TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
        }
        #endregion .Single

        #region .First
        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return GetQuery<TEntity>().First(predicate);
        }

        public TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).First();
        }
        #endregion .First
        
        #region .FirstOrDefault
        public TEntity FirstOrNull<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class {
            return GetQuery<TEntity>().Where(criteria).FirstOrDefault();
        }

        public TEntity FirstOrNull<TEntity>(ISpecification<TEntity> criteria) where TEntity : class {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).FirstOrDefault();
        }
        #endregion .FirstOrDefault

        #region .SingleOrDefault
        public TEntity SingleOrNull<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class {
            return GetQuery<TEntity>().Where(criteria).SingleOrDefault();
        }

        public TEntity SingleOrNull<TEntity>(ISpecification<TEntity> criteria) where TEntity : class {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).SingleOrDefault();
        }
        #endregion .SingleOrDefault

        #region .CountAll
        public int CountAll<TEntity>() where TEntity : class {
            return GetQuery<TEntity>().Count();
        }
        #endregion .CountAll

        #region .Count
        public int Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class {
            return GetQuery<TEntity>().Count(criteria);
        }

        public int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).Count();
        }
        #endregion .Count
        
        #region .Remove
        public void Remove<TEntity>(TEntity entity, bool applyToStore = false) where TEntity : class {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            if (!DbContext.Set<TEntity>().Local.Contains(entity))
                DbContext.Set<TEntity>().Attach(entity);

            DbContext.Entry(entity).State = EntityState.Deleted;

            if (applyToStore)
                DbContext.SaveChanges();
        }

        public void Remove<TEntity>(ISpecification<TEntity> criteria) where TEntity : class {
            IEnumerable<TEntity> records = Get<TEntity>(criteria);
            foreach (TEntity record in records) {
                Remove<TEntity>(record);
            }
        }
        #endregion .Remove
        
        #region .Attach
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }

            DbContext.Set<TEntity>().Attach(entity);
        }
        #endregion .Attach

        #region .SetOriginalValues
        public void SetOriginalValues<TEntity>(TEntity entity, TEntity originalEntity) where TEntity : class {
            if (!DbContext.Set<TEntity>().Local.Contains(entity))
                DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).OriginalValues.SetValues(originalEntity);
        }
        #endregion .SetOriginalValues
        
        #region .GetOriginalValues
        public TEntity GetOriginalValues<TEntity>(TEntity entity) where TEntity : class {
            if (!DbContext.Set<TEntity>().Local.Contains(entity))
                DbContext.Set<TEntity>().Attach(entity);
            return (TEntity)(DbContext.Entry(entity).OriginalValues.ToObject());
        }
        #endregion .GetOriginalValues
        
        #region .SetCurrentValues
        public void SetCurrentValues<TEntity>(TEntity entity, TEntity currentEntity) where TEntity : class {
            if (!DbContext.Set<TEntity>().Local.Contains(entity))
                DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).CurrentValues.SetValues(currentEntity);
        }
        #endregion .SetCurrentValues
        
        #region .GetCurrentValues
        public TEntity GetCurrentValues<TEntity>(TEntity entity) where TEntity : class {
            if (!DbContext.Set<TEntity>().Local.Contains(entity))
                DbContext.Set<TEntity>().Attach(entity);
            return (TEntity)(DbContext.Entry(entity).CurrentValues.ToObject());
        }
        #endregion .GetCurrentValues

        #region .GetStoreValues
        public TEntity GetStoreValues<TEntity>(TEntity entity) where TEntity : class {
            var keyNamesAndValues = new Dictionary<string,object>();

            foreach (string entityKeyName in GetEntityKeyNames<TEntity>())
                keyNamesAndValues.Add(
                    entityKeyName,
                    entity.GetType().GetProperty(entityKeyName).GetValue(entity,null)
                );
            
            return (TEntity)((IObjectContextAdapter)DbContext).ObjectContext.GetObjectByKey(
                    GetEntityKeyObject<TEntity>(keyNamesAndValues)
            );
        }
        #endregion .GetStoreValues  

        #region .Delete
        public void Delete<TEntity>(TEntity entity) where TEntity : class {
            Remove<TEntity>(entity);
            DbContext.SaveChanges();
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class {
            IEnumerable<TEntity> records = Get<TEntity>(criteria);

            foreach (TEntity record in records) {
                Remove<TEntity>(record);
            }
            DbContext.SaveChanges();
        }

        public void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class {
            IEnumerable<TEntity> records = Get<TEntity>(criteria);
            foreach (TEntity record in records) {
                Remove<TEntity>(record);
            }
            DbContext.SaveChanges();
        }
        #endregion .Delete

        #region .Reload
        public void Reload<TEntity>(TEntity entity) where TEntity : class{
            ((IObjectContextAdapter)DbContext).ObjectContext
                .Refresh(System.Data.Objects.RefreshMode.StoreWins,entity);
        }
        #endregion .Reload

        #region .Current
        //public TEntity Current<TEntity>() {

        //}
        #endregion .Current

        #region .SaveChanges
        public void SaveChanges() {
            UnitOfWork.SaveChanges();
        }
        #endregion .SaveChanges

        #region .BeginTransaction
        public void BeginTransaction() {
            UnitOfWork.BeginTransaction();
        }
        public void BeginTransaction(IsolationLevel isolationLevel) {
            UnitOfWork.BeginTransaction(isolationLevel);
        }
        #endregion .BeginTransaction

        #region .CommitTransaction
        public void CommitTransaction() {
            UnitOfWork.CommitTransaction();
        }
        #endregion .CommitTransaction

        #region .RollbackTransaction
        public void RollbackTransaction() {
            UnitOfWork.RollBackTransaction();
        }
        #endregion .RollbackTransaction

        #region .IsInTransaction
        public bool IsInTransaction() {
            return UnitOfWork.IsInTransaction;
        }
        #endregion .IsInTransaction

        #region IDisposable
        public void Dispose() {
            DbContextManager.DisposeAll();
        }
        #endregion IDisposable

        #region Helpers
        #region .GetEntityKeyObject
        private EntityKey GetEntityKeyObject<TEntity>(params object[] keyValues) where TEntity : class {
            var entitySetName = GetEntityName<TEntity>();
            var objectSet = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<TEntity>();
            
            List<EntityKeyMember> keyMembers = new List<EntityKeyMember>();
            for (var i = 0; i < objectSet.EntitySet.ElementType.KeyMembers.Count; i++)
                keyMembers.Add(new EntityKeyMember(objectSet.EntitySet.ElementType.KeyMembers[i].Name, keyValues[i]));

            return new EntityKey(entitySetName, keyMembers.ToArray());
        }
        private EntityKey GetEntityKeyObject<TEntity>(Dictionary<string,object> keyNamesAndValues) where TEntity : class {
            var entitySetName = GetEntityName<TEntity>();
            EntityKeyMember[] keyMembers = keyNamesAndValues.Select(kv=>new EntityKeyMember(kv.Key,kv.Value)).ToArray();

            return new EntityKey(entitySetName, keyMembers);
        }
        #endregion .GetEntityKeyObject
        
        #region .GetEntityKeyNames
        private IEnumerable<string> GetEntityKeyNames<TEntity>() where TEntity : class {
            var entitySetName = GetEntityName<TEntity>();
            var objectSet = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<TEntity>();

            return objectSet.EntitySet.ElementType.KeyMembers.Select(km => km.Name);
        }
        #endregion .GetEntityKeyNames

        #region .GetEntityName
        private string GetEntityName<TEntity>() where TEntity : class
        {
            return string.Format("{0}.{1}", ((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName, _pluralizer.Pluralize(typeof(TEntity).Name));
        }
        #endregion .GetEntityName

        #region .DbContext
        private DbContext _context;
        private DbContext DbContext {
            get {
                if (this._context == null) {
                    if (this._connectionStringName == string.Empty)
                        this._context = DbContextManager.Current;
                    else
                        this._context = DbContextManager.CurrentFor(this._connectionStringName);
                }
                
                return this._context;
            }
        }
        #endregion .DbContext

        #region .UnitOfWork
        private IUnitOfWork UnitOfWork {
            get {
                if (_unitOfWork == null) {
                    _unitOfWork = new EFUnitOfWork(this.DbContext);
                }
                return _unitOfWork;
            }
        }
        #endregion .UnitOfWork

        #endregion Helpers

        #region IRepositoryConfiguration
        bool _AutoDetectChanges = true;
        public bool AutoDetectChanges {
            get {
                return DbContext.Configuration.AutoDetectChangesEnabled;
            }
            set {
                DbContext.Configuration.AutoDetectChangesEnabled=value;
            }
        }
        #endregion IRepositoryConfiguration
    }
}
