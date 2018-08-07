using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class DBhouseOutletGradeController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: DBhouseOutletGrade
        public ActionResult Index()
        {
            return View(db.tbld_Outlet_grade.ToList());
        }

        // GET: DBhouseOutletGrade/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_grade tbld_Outlet_grade = db.tbld_Outlet_grade.Find(id);
            if (tbld_Outlet_grade == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_grade);
        }

        // GET: DBhouseOutletGrade/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DBhouseOutletGrade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,code,description")] tbld_Outlet_grade tbld_Outlet_grade)
        {
            if (ModelState.IsValid)
            {
                db.tbld_Outlet_grade.Add(tbld_Outlet_grade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbld_Outlet_grade);
        }

        // GET: DBhouseOutletGrade/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_grade tbld_Outlet_grade = db.tbld_Outlet_grade.Find(id);
            if (tbld_Outlet_grade == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_grade);
        }

        // POST: DBhouseOutletGrade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,code,description")] tbld_Outlet_grade tbld_Outlet_grade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbld_Outlet_grade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbld_Outlet_grade);
        }

        // GET: DBhouseOutletGrade/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_grade tbld_Outlet_grade = db.tbld_Outlet_grade.Find(id);
            if (tbld_Outlet_grade == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_grade);
        }

        // POST: DBhouseOutletGrade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_Outlet_grade tbld_Outlet_grade = db.tbld_Outlet_grade.Find(id);
            db.tbld_Outlet_grade.Remove(tbld_Outlet_grade);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
