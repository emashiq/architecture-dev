using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Library.PreActionHandler;
using UnitOfWork.Library.RepositoryCore;

namespace UnitOfWork.Library.UnitOfWorkCore
{
    public interface IUnitOfWork<TContext> :IDisposable where TContext:DbContext
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        IUnitOfWork<TContext> PreAction<TEntity>(params IPreActionHandler[] preActionHandlers) where TEntity : class;
        IUnitOfWork<TContext> PreAction(params IPreActionHandler[] preActionHandlers);
        IRepositoryGeneric<TEntity> GetGenericRepository<TEntity>() where TEntity : class;
        IRepository GetRepository();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}
