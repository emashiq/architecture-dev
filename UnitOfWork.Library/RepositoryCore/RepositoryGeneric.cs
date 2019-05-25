using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnitOfWork.Library.PagedList;
using UnitOfWork.Library.PreActionHandler;

namespace UnitOfWork.Library.RepositoryCore
{
    public class RepositoryGeneric<TEntity> : IRepositoryGeneric<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public List<IPreActionHandler> PreActionHandlers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>

        public RepositoryGeneric(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
            PreActionHandlers = new List<IPreActionHandler>();
        }

        public IQueryable<TEntity> GetAll()
        {

            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
                return _dbSet;
            else
                throw new AccessViolationException("You are not permitted to access");

        }


        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true, params string[] includes)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return orderBy(query).ToPagedList(pageIndex, pageSize);
                else return query.ToPagedList(pageIndex, pageSize);
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default(CancellationToken), params string[] includes)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return orderBy(query).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
                else return query.ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TResult}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TResult}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                         Expression<Func<TEntity, bool>> predicate = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                         int pageIndex = 0,
                                                         int pageSize = 20,
                                                         bool disableTracking = true, params string[] includes)
            where TResult : class
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize);
                else return query.Select(selector).ToPagedList(pageIndex, pageSize);
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <summary>
        /// Gets the <see cref="IPagedList{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                                    Expression<Func<TEntity, bool>> predicate = null,
                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                    int pageIndex = 0,
                                                                    int pageSize = 20,
                                                                    bool disableTracking = true,
                                                                    CancellationToken cancellationToken = default(CancellationToken), params string[] includes)
            where TResult : class
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
                else return query.Select(selector).ToPagedListAsync(pageIndex, pageSize, 0, cancellationToken);
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                         bool disableTracking = true, params string[] includes)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return orderBy(query).FirstOrDefault();
                else return query.FirstOrDefault();
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }


        /// <inheritdoc />
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = true, params string[] includes)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return await orderBy(query).FirstOrDefaultAsync();
                else return await query.FirstOrDefaultAsync();
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <summary>
        /// Gets the first or default entity based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public virtual TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  bool disableTracking = true, params string[] includes)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return orderBy(query).Select(selector).FirstOrDefault();
                else return query.Select(selector).FirstOrDefault();
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <inheritdoc />
        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  bool disableTracking = true, params string[] includes)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
            {
                IQueryable<TEntity> query = _dbSet;
                if (disableTracking) query = query.AsNoTracking();
                foreach (var item in includes) query.Include(item);
                if (predicate != null) query = query.Where(predicate);
                if (orderBy != null) return await orderBy(query).Select(selector).FirstOrDefaultAsync();
                else return await query.Select(selector).FirstOrDefaultAsync();
            }
            else
                throw new AccessViolationException("You are not permitted to access");

        }

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters) => _dbContext.Database.SqlQuery<TEntity>(sql, parameters).AsQueryable();

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The found entity or null.</returns>
        public virtual TEntity Find(params object[] keyValues)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0) return _dbSet.Find(keyValues);
            else throw new AccessViolationException("You are not permitted to access");
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A <see cref="Task{TEntity}" /> that represents the asynchronous insert operation.</returns>
        public virtual Task<TEntity> FindAsync(params object[] keyValues)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0) return _dbSet.FindAsync(keyValues);
            else throw new AccessViolationException("You are not permitted to access");
            
        }

        /// <summary>
        /// Finds an entity with the given primary key values. If found, is attached to the context and returned. If no entity is found, then null is returned.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task{TEntity}"/> that represents the asynchronous find operation. The task result contains the found entity or null.</returns>
        public virtual Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0) return _dbSet.FindAsync(keyValues, cancellationToken);
            else throw new AccessViolationException("You are not permitted to access");
            
        }

        /// <summary>
        /// Gets the count based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count == 0)
                return (predicate == null ? _dbSet.Count() : _dbSet.Count(predicate));
            else throw new AccessViolationException("You are not permitted to access");
        }

        /// <summary>
        /// Inserts a new entity synchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public virtual void Insert(TEntity entity)
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count == 0)
                _dbSet.Add(entity);
        }

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Insert(params TEntity[] entities)
        {
            bool flag = true;
            foreach (var item in entities)
                if (PreActionHandlers.PreWorkRunner(item).ValidationResults.Count > 0)
                    flag = false;
            if (flag)
                _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Inserts a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            bool flag = true;
            foreach (var item in entities)
                if (PreActionHandlers.PreWorkRunner(item).ValidationResults.Count > 0)
                    flag = false;
            if (flag)
                _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Inserts a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count > 0)
                return Task.Run(() => _dbSet.Add(entity), cancellationToken);
            return Task.Run(() => { },cancellationToken);
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous insert operation.</returns>
        public virtual Task InsertAsync(params TEntity[] entities)
        {
            bool flag = true;
            foreach (var item in entities)
                if (PreActionHandlers.PreWorkRunner(item).ValidationResults.Count > 0)
                    flag = false;
            if (flag)
                Task.Run(() => _dbSet.AddRange(entities));
            return Task.Run(() => { });
        }

        /// <summary>
        /// Inserts a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        public virtual Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken)) => Task.Run(() => _dbSet.AddRange(entities), cancellationToken);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(TEntity entity)
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count > 0)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void UpdateAsync(TEntity entity) => Update(entity);

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>

        public virtual void Update(params TEntity[] entities) => entities.ToList().ForEach(x => Update(x));

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(IEnumerable<TEntity> entities) => entities.ToList().ForEach(x => Update(x));

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count > 0)
                _dbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public virtual void Delete(params object[] keys)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count > 0)
                throw new AccessViolationException("You dont have permission to access");

            var entity = _dbSet.Find(keys);
            Delete(entity);
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Delete(params TEntity[] entities)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count > 0)
                _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (PreActionHandlers.PreWorkRunner(typeof(TEntity)).ValidationResults.Count > 0)
                _dbSet.RemoveRange(entities);
        }
    }
}
