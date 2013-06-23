namespace MyLibrary.Data {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Specification;
	using System.Data;

	public enum SortOrder { Ascending, Descending }
    public enum RefreshMode { ClientWins, StoreWins }

    public interface IRepository {
        #region .GetByKey
        /// <summary>
        /// Gets an entity by its key(s). and throws an exception if the key does not exist
        /// For Entities with composite keys, multiple values must be provided in the same key order as they exist on the enityt
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keyValue">The key's value.</param>
        /// <returns></returns>
        TEntity GetByKey<TEntity>(params object[] keyValues) where TEntity : class;
        #endregion .GetByKey

        #region .TryGetByKey
        /// <summary>
        /// Attempts to get and entity by its key, returning it in out 'entity' parameters. 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keyValue">The key's value.</param>
        /// <returns>Boolean indicating an entity was retrieved</returns>
        bool TryGetByKey<TEntity>(out TEntity entity, params object[] keyValues) where TEntity : class;
        #endregion .TryGetByKey

        #region .GetQuery
        /// <summary>
        /// Provides a queryable object that can be used to build a query, using LINQ to Entities, before invoking it in the store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A Queryable type for the entity</returns>
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;

        /// <summary>
        /// Provides a queryable object, pre-populated with the where clause specified in 'predicate', that can be used to further build a query, using LINQ to Entities, before invoking it in the store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to use for the where clause.</param>
        /// <returns>A Queryable type for the entity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Supports the Specification pattern to provide a queryable object, pre-populated with the where clause specified in 'criteria', that can be used to further build a query, using LINQ to Entities, before invoking it in the store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to use for the where clause.</param>
        /// <returns>A Queryable type for the entity</returns>
        IQueryable<TEntity> GetQuery<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        #endregion .GetQuery

        #region .Single
        /// <summary>
        /// Retrieves one entity from the store based on matching 'criteria'.
        /// <remarks>Use to ensure the store contains one and only one record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match records on.</param>
        /// <returns>An entity instance representing the matched record</returns>
        /// <exception cref="InvalidOperationException">If 'criteria' matches multiple or no records in the store</exception>
        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Supports the Specification pattern to retrieve a single entity from the store based on matching 'criteria'.
        /// <remarks>Use to ensure the store contains one and only one record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match records on.</param>
        /// <returns>An entity instance representing the matched record</returns>
        /// <exception cref="InvalidOperationException">If 'criteria' matches multiple or no records in the store</exception>
        TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        #endregion .Single

        #region .SingleOrNull
        /// <summary>
        /// Retrieves one entity from the store based on matching 'criteria' or null if the criteria does not match any records.
        /// <remarks>Use to ensure the store contains one or no record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria as an ISpecification object.</param>
        /// <returns>An entity instance representing the matched record or Null.</returns>
        /// <exception cref="InvalidOperationException">If 'criteria' matches multiple records.</exception>
        TEntity SingleOrNull<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Supports the Specification pattern to retrieve one entity from the store based on matching 'criteria' or null if the criteria does not match any records.
        /// <remarks>Use to ensure the store contains one or no record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria as an ISpecification object.</param>
        /// <returns>An entity instance representing the matched record or Null.</returns>
        TEntity SingleOrNull<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        #endregion .SingleOrNull
        
        #region .First
        /// <summary>
        /// Retrieves the first entity from a set of records matching 'criteria' or throw an exception if no records match.
        /// <remarks>Use to ensure the store contains at least one record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria as a lambda expression.</param>
        /// <returns>An entity instance representing the matched record.</returns>
        /// <exception cref="InvalidOperationException">If no records match 'predicate'.</exception>
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Supports the Specification pattern to retireve the first entity from a set of records matching 'criteria' or throw an exception if no records match.
        /// <remarks>Use to ensure the store contains at least one record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria as a ISpecification object.</param>
        /// <returns>An entity instance representing the matched record.</returns>
        /// <exception cref="InvalidOperationException">If no records match 'predicate'.</exception>
        TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        #endregion .First

        #region .FirstOrNull
        /// <summary>
        /// Retrieves the first entity from a set of records matching 'criteria' or Null if no records match.
        /// <remarks>Use to ensure the store contains at least one or no record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to matchg records on.</param>
        /// <returns>An entity instance representing the matched record or Null.</returns>
        TEntity FirstOrNull<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Supports the Specification pattern to retireve the first entity from a set of records matching 'criteria' or Null if no records match.
        /// <remarks>Use to ensure the store contains at least one or no record that matches 'criteria'</remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to matchg records on.</param>
        /// <returns>An entity instance representing the matched record or Null.</returns>
        TEntity FirstOrNull<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        #endregion .FirstOrNull
        
        #region .Get
        /// <summary>
        /// Retrieves entities from the store that match 'criteria'.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match records on.</param>
        /// <returns>Enumerable set of all the records matching 'criteria'.</returns>
        IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Retrieves entities from the store that match 'criteria', 
        /// by applying 'orderby' and 'sortOrder', then returning only the records on the page indicated by 'pageIndex', using 'pageSize' to calculate the size of each page.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match record on.</param>
        /// <param name="pageIndex">Index of the page to be retreived .</param>
        /// <param name="pageSize">Size of the page to use when calculating the number of pages.</param>
        /// <param name="orderBy">The property on which to order the records, before selecting.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>Enumerable set of records matching 'criteria'.</returns>
        IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> criteria, int pageIndex, int pageSize, Expression<Func<TEntity, string>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;

        /// <summary>
        /// Supports the Specification pattern to retrieves entities from the store based on 'criteria'.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match record on.</param>
        /// <returns>Enumerable set of records matching 'criteria'.</returns>
        IEnumerable<TEntity> Get<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        /// <summary>
        /// Supports the specification pattern to retriueve entities from the store that match 'criteria', 
        /// by applying 'orderby' and 'sortOrder', then returning only the records on the page indicated by 'pageIndex', using 'pageSize' to calculate the size of each page.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match record on.</param>
        /// <param name="pageSize">Size of the page to use when calculating the number of pages.</param>
        /// <param name="pageIndex">Index of the page to be retreived .</param>
        /// <param name="orderBy">The property on which to order the records, before selecting.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>Enumerable set of records matching 'criteria'.</returns>
        IEnumerable<TEntity> Get<TEntity>(ISpecification<TEntity> criteria, int pageIndex, int pageSize, Expression<Func<TEntity, string>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;
        #endregion .Get
                        
        #region .GetAll
        /// <summary>
        /// Retrieves all entities from the store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Enumerable set of all records.</returns>
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;

        /// <summary>
        /// Retrieves all entities from the store, 
        /// by applying 'orderby' and 'sortOrder', then returning only the records on the page indicated by 'pageIndex', using 'pageSize' to calculate the size of each page.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="pageSize">Size of the page to use when calculating the number of pages.</param>
        /// <param name="orderBy">The property on which to order the records, before selecting.</param>
        /// <param name="pageIndex">Index of the page to be retreived .</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>Enumerable set of all records.</returns>
        IEnumerable<TEntity> GetAll<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, string>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;
        #endregion .GetAll

        #region .Count
        /// <summary>
        /// Provides the numer of records in the store that match 'criteria'.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match records on.</param>
        /// <returns>The number of records matching 'criteria'.</returns>
        int Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Supports teh Specification pattern to provides the numer of records in the store that match 'criteria'.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria to match records on.</param>
        /// <returns>The number of records matching 'criteria'.</returns>
        int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        #endregion .Count

        #region .CountAll
        /// <summary>
        /// Provides the numer of records in the store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The number of records in the store.</returns>
        int CountAll<TEntity>() where TEntity : class;
        #endregion CountAll
        
        #region .Add
        /// <summary>
        /// Adds an entity to the repository.
        /// <remarks>If 'applyToStore' is ommited or is specified as false, the entity will only be added to the temporary 
        /// store and <see cref="SaveChanges"/> must be called to add it to the persistent store.
        /// </remarks>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be added.</param>
        /// <param name="applyToStore">Whether the newly added entity should be persisted to the store immediately.</param>
        void Add<TEntity>(TEntity entity, bool applyToStore = false) where TEntity : class;
        #endregion .Add

        #region .Remove
        /// <summary>
        /// Removes an entity from the repository.
        /// <remarks>If 'applyToStore' is ommited or is specified as false, the entity will only be removed from the temporary 
        /// store and <see cref="SaveChanges"/> must be called to remove it from the persistent store.
        /// </remarks>
        /// Use to mark multiple entities for deletion before deleting themn all, as a batch, from the store.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be removed.</param>
        void Remove<TEntity>(TEntity entity, bool applyToStore = false) where TEntity : class;
        #endregion .Remove

		#region .Update
    	void Update<TEntity>(
			TEntity entity, 
			TEntity originalEntity,
			params Expression<Func<TEntity, object>>[] modifiedProperties
		) where TEntity : class;

		void Update<TEntity>(
			TEntity entity,
			params Expression<Func<TEntity, object>>[] modifiedProperties
		) where TEntity : class;
		#endregion .Update

		//#region .Attach

        ///// <summary>
        ///// Attaches the specified entity.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <param name="entity">The entity.</param>
        //void Attach<TEntity>(TEntity entity) where TEntity : class;
        //#endregion .Attach

        #region .SetOriginalValues
        void SetOriginalValues<TEntity>(TEntity entity, TEntity originalEntity) where TEntity : class;
        #endregion .SetOriginalValues

        #region .GetOriginalValues
        TEntity GetOriginalValues<TEntity>(TEntity entity) where TEntity : class;
        #endregion .GetOriginalValues

        #region .SetCurrentValues
        void SetCurrentValues<TEntity>(TEntity entity, TEntity currentEntity) where TEntity : class;
        #endregion .SetCurrentValues
        
        #region .GetCurrentValues
        TEntity GetCurrentValues<TEntity>(TEntity entity) where TEntity : class;
        #endregion .GetCurrentValues

        #region .GetStoreValues
        TEntity GetStoreValues<TEntity>(TEntity entity) where TEntity : class;
        #endregion .GetStoreValues
        
        #region .Reload
        void Reload<TEntity>(TEntity entity) where TEntity : class;
        #endregion .Reload

        #region .Delete
        /// <summary>
        /// Deletes one or many entities matching the specified criteria
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Deletes entities which satify specificatiion
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        #endregion .Delete

        #region .SaveChanges
        void SaveChanges();
        #endregion .SaveChanges

        #region .BeginTransaction
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion .BeginTransaction

        #region .CommitTransaction
        void CommitTransaction();
        #endregion .CommitTransaction

        #region .RollbackTransaction
        void RollbackTransaction();
        #endregion .RollbackTransaction

        #region .IsInTransaction
        bool IsInTransaction();
        #endregion .IsInTransaction
    }

    public interface IRepositoryConfiguration {
        bool AutoDetectChanges { get; set; }
    }
}
