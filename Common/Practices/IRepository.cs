using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Avantech.Common.Practices
{
    /// <summary>
    /// Interface to standardise basic functionality that should be implemented by all repositories
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Query();

        /// <summary>
        /// Gets an entity, having minimal property values that uniquely identify the entity, with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id entity to summerise.</param>
        /// <returns>An entity, in summerized form.</returns>
        TEntity Summerise(int id);

        /// <summary>
        /// Gets the entity specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <param name="includes">The additional value-type entitites to include in the retrieved set.</param>
        /// <returns>An entity.</returns>
        TEntity Get(
            int id,
            params Expression<Func<TEntity, object>>[] includes
        );

        /// <summary>
        /// Gets a single enity that matches the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate to match against a single entity.</param>
        /// <param name="includes">The additional value-type entitites to include in the retrieved set.</param>
        /// <remarks>Use when the predicate has to be guarenteed to match only one entity.</remarks>
        /// <returns>An entity.</returns>
        TEntity Get(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes
        );

        /// <summary>
        /// Adds the specified entity to the set.
        /// <remarks>The entity's id must be set to the default value of {int}/></remarks>
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Updates the <paramref name="entity"/> in the datastore. 
        /// Only updates the <paramref name="modifiedProperties" /> if specified.
        /// </summary>
        /// <param name="entity">The entity with values to be updated.
        /// <remarks>The id of the entity must be set to that of an existing entity in the set.</remarks>
        /// </param>
        /// <param name="modifiedProperties">The modified properties.</param>
        void Update(
            TEntity entity,
            params Expression<Func<TEntity, object>>[] modifiedProperties
        );

        /// <summary>
        /// Deletes the entity wthis the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the record to be deleted.</param>
        void Delete(int id);

        /// <summary>
        /// Gets a dictionary of entity ids and text descrptio;ns of all entities that satisfy the specified <paramref name="predicate"/>.
        /// <remarks>Useful when presenting options for selecting an entity, prior to retrieve full details of the entity.</remarks>
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A dictionary of entity id and text descriptions</returns>
        Dictionary<int, string> GetIndex(
            Expression<Func<TEntity, bool>> predicate = null
        );

        /// <summary>
        /// Gets all entities, having minimal property values that uniquely identify an entity, that satisfy the specified <paramref name="predicate"/>.
        /// <remarks>Useful when needing to display a summary list of entities.</remarks>
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>An enumeration of entities, in summerized form.</returns>
        IEnumerable<TEntity> SummariseAll(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Gets a paged enumeration of all entities, having minimal property values that uniquely identify an entity, that satisfy the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="recordCount">The total number of entites found to match the <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicate to match entities by.</param>
        /// <returns>A paged enumeration of entities, in summerized form.</returns>
        IEnumerable<TEntity> SummariseAll(
            int pageIndex,
            int pageSize,
            out int recordCount,
            Expression<Func<TEntity, bool>> predicate = null
        );

        /// <summary>
        /// Gets all entities that satisfy the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The additional value-type entitites to include in the retrieved set.</param>
        /// <returns>An enumeration of entities.</returns>
        IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        /// <summary>
        /// Gets a paged enumeration of all entities that satisfy the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="recordCount">The total number of entites found to match the <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicate to match entities by.</param>
        /// <param name="includes">The additional value-type entitites to include in the retrieved set.</param>
        /// <returns>An enumeration of entities.</returns>
        IEnumerable<TEntity> GetAll(
            int pageIndex,
            int pageSize,
            out int recordCount,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        /// <summary>
        /// Determines if all entities in the set satisfy the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate to be matched against ech record.</param>
        /// <returns>Whether or not all entities match.</returns>
        bool All(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Number of entities.</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Determines if any entity in the set satisfy the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate to be matched against ech record.</param>
        /// <returns>Whether or not any entities match.</returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);
    }
}
