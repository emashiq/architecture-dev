using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UnitOfWork.Library.PreActionHandler;
using UnitOfWork.Library.RepositoryCore;

namespace UnitOfWork.Library.UnitOfWorkCore
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        #region CTOR
        public readonly TContext _context;
        private bool disposed = false;
        private Dictionary<Type, object> repositoriesGeneric;
        private object repositories;
        public UnitOfWork(TContext context)
        {
            _context = context;
            repositoriesGeneric = new Dictionary<Type, object>();
        }
        public IUnitOfWork<TContext> PreAction<TEntity>(params IPreActionHandler[] preActionHandlers) where TEntity : class
        {
            var genericRepositories = GetGenericRepository<TEntity>();
            genericRepositories.PreActionHandlers = preActionHandlers.ToList();
            return this;
        }
        public IUnitOfWork<TContext> PreAction(params IPreActionHandler[] preActionHandlers)
        {
            var repository = GetRepository();
            repository.PreActionHandlers = preActionHandlers.ToList();
            return this;
        }
        #endregion
        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        public IRepositoryGeneric<TEntity> GetGenericRepository<TEntity>() where TEntity : class
        {
            
            var type = typeof(IRepositoryGeneric<TEntity>);
            if (!repositoriesGeneric.ContainsKey(type)) repositoriesGeneric.Add(type, new RepositoryGeneric<TEntity>(_context));
            return (IRepositoryGeneric<TEntity>)repositoriesGeneric[type];
        }
        /// <summary>
        /// Get Repository With Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IRepository GetRepository()
        {
            if (repositories == null) repositories = new Repository(_context);
            return (IRepository)repositories;
        }
        /// <summary>
        /// Begining Transaction of Operation
        /// </summary>
        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }
        /// <summary>
        /// Commiting Current Transaction
        /// </summary>
        public void Commit()
        {
            _context.Database.CurrentTransaction.Commit();
        }
        /// <summary>
        /// Rollback Current Transaction
        /// </summary>
        public void Rollback()
        {
            _context.Database.CurrentTransaction.Rollback();
        }

        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of state entities written to database.</returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlCommand(sql, parameters);

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{T}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Database.SqlQuery<TEntity>(sql, parameters).AsQueryable();

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Saves all changes made in this context to the database with distributed transaction.
        /// </summary>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync(params IUnitOfWork<TContext>[] unitOfWorks)
        {
            try
            {
                var count = 0;
                foreach (var uow in unitOfWorks)
                    uow.BeginTransaction();
                foreach (var uow in unitOfWorks)
                    count += await uow.SaveChangesAsync();
                foreach (var item in unitOfWorks)
                    item.Commit();
                return count;

            }
            catch (Exception ex)
            {
                foreach (var item in unitOfWorks)
                    item.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                        repositories = null;
                    // dispose the db context.
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        
    }
}
