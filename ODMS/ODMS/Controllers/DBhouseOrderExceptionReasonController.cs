using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class DBhouseOrderExceptionReasonController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: DBhouseOrderExceptionReason
        public ActionResult Index()
        {
            return View(db.tblm_notorder_reason.ToList());
        }

        // GET: DBhouseOrderExceptionReason/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblm_notorder_reason tblmNotorderReason = db.tblm_notorder_reason.Find(id);
            if (tblmNotorderReason == null)
            {
                return HttpNotFound();
            }
            return View(tblmNotorderReason);
        }

        // GET: DBhouseOrderExceptionReason/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DBhouseOrderExceptionReason/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,exception_name,exception_code,exception_description")] tblm_notorder_reason tblmNotorderReason)
        {
            if (ModelState.IsValid)
            {
                db.tblm_notorder_reason.Add(tblmNotorderReason);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblmNotorderReason);
        }

        // GET: DBhouseOrderExceptionReason/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblm_notorder_reason tblmNotorderReason = db.tblm_notorder_reason.Find(id);
            if (tblmNotorderReason == null)
            {
                return HttpNotFound();
            }
            return View(tblmNotorderReason);
        }

        // POST: DBhouseOrderExceptionReason/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,exception_name,exception_code,exception_description")] tblm_notorder_reason tblmNotorderReason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblmNotorderReason).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblmNotorderReason);
        }

        // GET: DBhouseOrderExceptionReason/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblm_notorder_reason tblmNotorderReason = db.tblm_notorder_reason.Find(id);
            if (tblmNotorderReason == null)
            {
                return HttpNotFound();
            }
            return View(tblmNotorderReason);
        }

        // POST: DBhouseOrderExceptionReason/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblm_notorder_reason tblmNotorderReason = db.tblm_notorder_reason.Find(id);
            if (tblmNotorderReason != null) db.tblm_notorder_reason.Remove(tblmNotorderReason);
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
