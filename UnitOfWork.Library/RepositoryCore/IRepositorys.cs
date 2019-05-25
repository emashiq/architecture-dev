using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnitOfWork.Library.PreActionHandler;
namespace UnitOfWork.Library.RepositoryCore
{
    public interface IRepository
    {
        List<IPreActionHandler> PreActionHandlers { get; set; } 
        void Insert(object entity);
        void Insert(params object[] entities);
        void Insert(IEnumerable<object> entities);
        Task InsertAsync(object entity, CancellationToken cancellationToken = default(CancellationToken));
        Task InsertAsync(params object[] entities);
        Task InsertAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken));
        void Update(object entity);
        void Update(params object[] entities);
        void Update(IEnumerable<object> entities);
        void Delete(Type type,params object[] keys);
        void Delete(List<object> entity);
        void Delete(object entity);
    }
}
