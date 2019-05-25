using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Aggregates.Contexts;
using Aggregates.Models.Students;
using WorkManager.Work;
using WorkManager.Work.PreActions;

namespace WebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IWorkManager _workManager;
        public StudentsController()
        {
            _workManager = new WorkManager.Work.WorkManager();
        }
        // GET: Students
        public async Task<ActionResult> Index()
        {
            var students = _workManager.GetUnitOfWork<ApplicationContext>().GetGenericRepository<Student>().GetAll();
            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Student student = await _workManager.GetUnitOfWork<ApplicationContext>().GetGenericRepository<Student>().FindAsync(id);
            if (student == null)
                return HttpNotFound();
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _workManager.GetUnitOfWork<ApplicationContext>().PreAction(new StudentPreAction()).GetRepository().Insert(student);
                await _workManager.GetUnitOfWork<ApplicationContext>().SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Student student = await _workManager.GetUnitOfWork<ApplicationContext>().GetGenericRepository<Student>().FindAsync(id);
            if (student == null)
                return HttpNotFound();
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                _workManager.GetUnitOfWork<ApplicationContext>().GetRepository().Update(student);
                await _workManager.GetUnitOfWork<ApplicationContext>().SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(await _workManager.GetUnitOfWork<ApplicationContext>().GetGenericRepository<Student>().FindAsync(id));
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Student student = await _workManager.GetUnitOfWork<ApplicationContext>().GetGenericRepository<Student>().FindAsync(id);
            _workManager.GetUnitOfWork<ApplicationContext>().GetRepository().Delete(student);
            await _workManager.GetUnitOfWork<ApplicationContext>().SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _workManager.Dispose();
            base.Dispose(disposing);
        }
    }
}
