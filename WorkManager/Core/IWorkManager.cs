using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Library.UnitOfWorkCore;

namespace WorkManager.Work
{
    public interface IWorkManager:IDisposable
    {
        IUnitOfWork<TContext> GetUnitOfWork<TContext>() where TContext : DbContext;
    }
}
