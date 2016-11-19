using FitnessViewer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace FitnessViewer.Infrastructure.Data
{
    public class GenericRepository : IRepository
    {

        private DbContext _context;

        /// <summary>
        /// Initializes a new instance of the GenericRepository class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GenericRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            _context = context;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<TEntity>().Add(entity);
        }

        public void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<TEntity>().AddOrUpdate(entity);
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<TEntity>().Attach(entity);
            Update<TEntity>(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<TEntity>().Remove(entity);
        }


        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public TEntity GetByKey<TEntity>(long keyValue, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : Entity<long>
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            
            foreach (var includeExpression in includeExpressions)
                query = query.Include(includeExpression);
            
            return query.Where(e => e.Id == keyValue).FirstOrDefault();
        }



        public TEntity GetByKey<TEntity>(int keyValue, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : Entity<int>
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var includeExpression in includeExpressions)
                query = query.Include(includeExpression);

            return query.Where(e => e.Id == keyValue).FirstOrDefault();
        }


        public IQueryable<TEntity> GetByUserId<TEntity>(string userId, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : class, IUserEntity
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var includeExpression in includeExpressions)
                query = query.Include(includeExpression);

            return query.Where(e => e.UserId == userId);
        }

        public IQueryable<TEntity> GetByActivityId<TEntity>(long activityId, params Expression<Func<TEntity, object>>[] includeExpressions) where TEntity : class, IActivityEntity
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var includeExpression in includeExpressions)
                query = query.Include(includeExpression);

            return query.Where(e => e.ActivityId == activityId);
        }


        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

       public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();

        }
    }
}
