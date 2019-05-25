using System;
using System.Collections.Generic;
using System.Data.Entity;
using UnitOfWork.Library.UnitOfWorkCore;
namespace WorkManager.Work
{
    public class WorkManager : IWorkManager,IDisposable
    {
        private Dictionary<Type, object> objectDictionaries = new Dictionary<Type, object>();
        private bool disposed = false;
        public WorkManager()
        {
            objectDictionaries = new Dictionary<Type, object>();
        }
        public void Dispose()
        {
            var disposed = true;
            Dispose(disposed);
        }

        public IUnitOfWork<TContext> GetUnitOfWork<TContext>() where TContext : DbContext
        {
            var contextType = typeof(TContext);
            if (!objectDictionaries.ContainsKey(contextType)) objectDictionaries.Add(typeof(TContext), Activator.CreateInstance(contextType));
            var unitOfWorkType = typeof(IUnitOfWork<TContext>);
            if (!objectDictionaries.ContainsKey(unitOfWorkType)) objectDictionaries.Add(unitOfWorkType, new UnitOfWork<TContext>((TContext)objectDictionaries[contextType]));
            return (IUnitOfWork<TContext>)objectDictionaries[unitOfWorkType];
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    foreach (var item in objectDictionaries)
                        ((IDisposable)item.Value).Dispose();
        }
    }
}
