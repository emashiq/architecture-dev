using Aggregates.Configurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitOfWork.Library.PreActionHandler;

namespace UnitOfWork.Library.RepositoryCore
{
    public class Repository : IRepository
    {
        private DbContext _context;
        private DbSet _dbSet;
        public List<IPreActionHandler> PreActionHandlers { get; set; }

        public Repository(DbContext context)
        {
            _context = context;
            PreActionHandlers = new List<IPreActionHandler>();
        }
        private void DbSetSetter(Type type) => _dbSet = _context.Set(type);
        public void Delete(Type type, params object[] keys)
        {
            if (PreActionHandlers.PreWorkRunner(type).ValidationResults.Count() == 0)
            {
                DbSetSetter(type);
                var objects = _dbSet.Find(keys);
                Delete(objects);
            }

        }
        public void Delete(List<object> entities) => entities.ForEach(x => Delete(x));
        public void Delete(object entity)
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count() == 0)
            {
                DbSetSetter(entity.GetType());
                _dbSet.Remove(entity);
            }
        }
        public void Insert(object entity)
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count() == 0)
            {
                DbSetSetter(entity.GetType());
                _dbSet.Add(entity);
            }
        }
        public void Insert(params object[] entities) => entities.ToList().ForEach(x => Insert(x));
        public void Insert(IEnumerable<object> entities) => entities.ToList().ForEach(x => Insert(x));
        public Task InsertAsync(object entity, CancellationToken cancellationToken = default(CancellationToken)) => Task.Run(() => Insert(entity), cancellationToken);
        public Task InsertAsync(params object[] entities) => Task.Run(() => entities.ToList().ForEach(x => Insert(x)));
        public Task InsertAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken)) => Task.Run(() => entities.ToList().ForEach(x => Insert(x)));
        public void Update(object entity)
        {
            if (PreActionHandlers.PreWorkRunner(entity).ValidationResults.Count() == 0)
            {
                DbSetSetter(entity.GetType());
                if (_context.Entry(entity).State == EntityState.Detached)
                    _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
        public void Update(params object[] entities) => entities.ToList().ForEach(x => Update(x));
        public void Update(IEnumerable<object> entities) => entities.ToList().ForEach(x => Update(x));
    }
}
