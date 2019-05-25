using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnitOfWork.Library.PagedList;
using UnitOfWork.Library.PreActionHandler;

namespace UnitOfWork.Library.RepositoryCore
{
    public interface IRepositoryGeneric<TEntity> where TEntity : class
    {
        List<IPreActionHandler> PreActionHandlers { get; set; } 
        IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true, params string[] includes);

        Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                    int pageIndex = 0,
                                                    int pageSize = 20,
                                                    bool disableTracking = true,
                                                    CancellationToken cancellationToken = default(CancellationToken), params string[] includes);

        IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  int pageIndex = 0,
                                                  int pageSize = 20,
                                                  bool disableTracking = true, params string[] includes) where TResult : class;


        Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                             Expression<Func<TEntity, bool>> predicate = null,
                                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                             int pageIndex = 0,
                                                             int pageSize = 20,
                                                             bool disableTracking = true,
                                                             CancellationToken cancellationToken = default(CancellationToken), params string[] includes) where TResult : class;

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                  bool disableTracking = true,params string[] includes);

        TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                           Expression<Func<TEntity, bool>> predicate = null,
                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                           bool disableTracking = true,params string[] includes);

        Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = true,params string[] includes);

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = true, params string[] includes);

        IQueryable<TEntity> FromSql(string sql, params object[] parameters);

        TEntity Find(params object[] keyValues);

        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);

        IQueryable<TEntity> GetAll();

        int Count(Expression<Func<TEntity, bool>> predicate = null);
        void Insert(TEntity entity);

        void Insert(params TEntity[] entities);

        void Insert(IEnumerable<TEntity> entities);

        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task InsertAsync(params TEntity[] entities);
        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));

        void Update(TEntity entity);
        void Update(params TEntity[] entities);

        void Update(IEnumerable<TEntity> entities);

        void Delete(params object[] keys);
        void Delete(TEntity entity);

        void Delete(params TEntity[] entities);

        void Delete(IEnumerable<TEntity> entities);
    }
}
