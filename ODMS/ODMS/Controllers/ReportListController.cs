using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class ReportListController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: ReportList
        public ActionResult Index()
        {
            return View(db.tbld_ReportList.ToList());
        }

        // GET: ReportList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_ReportList tbldReportList = db.tbld_ReportList.Find(id);
            if (tbldReportList == null)
            {
                return HttpNotFound();
            }
            return View(tbldReportList);
        }

        // GET: ReportList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportList/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ReportCode,ReportName,ReportController,ReportAction,ReportType")] tbld_ReportList tbldReportList)
        {
            if (ModelState.IsValid)
            {
                tbldReportList.id = Convert.ToInt32(tbldReportList.ReportCode);
                db.tbld_ReportList.Add(tbldReportList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldReportList);
        }

      
        // GET: ReportList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_ReportList tbldReportList = db.tbld_ReportList.Find(id);
            if (tbldReportList == null)
            {
                return HttpNotFound();
            }
            return View(tbldReportList);
        }

        // POST: ReportList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_ReportList tbldReportList = db.tbld_ReportList.Find(id);
            if (tbldReportList != null) db.tbld_ReportList.Remove(tbldReportList);
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
