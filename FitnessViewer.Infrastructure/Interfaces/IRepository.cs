using FitnessViewer.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface IRepository
    {
        /// <summary>
        /// Gets entity by key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keyValue">The key value.</param>
        /// <param name="includeExpressions">Related objects to include in the query results.  e.g. o=>o.ActivityType</param>
        /// <returns></returns>
        TEntity GetByKey<TEntity>(int keyValue, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : Entity<int>;

        /// <summary>
        /// Gets entity by key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keyValue">The key value.</param>
        /// <param name="includeExpressions">Related objects to include in the query results.  e.g. o=>o.ActivityType</param>
        /// <returns></returns>
        TEntity GetByKey<TEntity>(long keyValue, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : Entity<long>;


        /// <summary>
        /// Get entities for a given UserId
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="userId">ASP.NET userId</param>
        /// <param name="includeExpressions">Related objects to include in the query results.  e.g. o=>o.ActivityType</param>
        /// <returns></returns>
        IQueryable<TEntity> GetByUserId<TEntity>(string userId, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : class, IUserEntity;

        /// <summary>
        /// Get entities for a given activity
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="userId">ASP.NET userId</param>
        /// <param name="includeExpressions">Related objects to include in the query results.  e.g. o=>o.ActivityType</param>
        /// <returns></returns>
        IQueryable<TEntity> GetByActivityId<TEntity>(long activityId, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : class, IActivityEntity;

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Adds or Updates the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class;


        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Attach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Delete a range of the specified entity
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity</typeparam>
        /// <param name="entities">Collection of entities</param>
        void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        
        /// <summary>
        /// Updates changes of the existing entity. 
        /// The caller must later call SaveChanges() on the repository explicitly to save the entity to database
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Update<TEntity>(TEntity entity) where TEntity : class;
        
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
    }
}