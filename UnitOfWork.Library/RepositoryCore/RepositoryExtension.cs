using System.Collections.Generic;
using Aggregates.Configurations;
using UnitOfWork.Library.PreActionHandler;

namespace UnitOfWork.Library.RepositoryCore
{
    public static class RepositoryExtensions
    {
        public static IValidationResult PreWorkRunner(this List<IPreActionHandler> handler,object entity)
        {
            ValidationResult validationResult = new ValidationResult();
            handler.ForEach(x => x.Operate(entity.GetType() == typeof(IEntity) ? (IValidationResult)entity : validationResult));
            return entity.GetType() == typeof(IEntity) ? (IValidationResult)entity : validationResult;
        }

        public static IValidationResult PreWorkRunner<TEntity>(this List<IPreActionHandler> handler,object entity)
        {
            handler.ForEach(x => x.Operate(entity));
            return entity.GetType() == typeof(TEntity) ? (IValidationResult)entity : new ValidationResult();
        }
    }
}
