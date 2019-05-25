using Aggregates.Models.Students;
using UnitOfWork.Library.PreActionHandler;

namespace WorkManager.Work.PreActions
{
    public class StudentPreAction : IPreActionHandler
    {
        public void Operate(object entity)
        {
            var dbObject = (Student)entity;
            if (dbObject.Age < 18)
                dbObject.ValidationResults.Add("Under18Age", "Age can not be under 18");
        }
    }
}
