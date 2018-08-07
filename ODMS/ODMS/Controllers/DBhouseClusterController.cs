using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    [SessionExpire,SuperAccess]

    public class DBhouseClusterController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: DBhouseCluster
        public ActionResult Index()
        {
            return View(db.tbld_cluster.ToList());
        }

        // GET: DBhouseCluster/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_cluster tbldCluster = db.tbld_cluster.Find(id);
            if (tbldCluster == null)
            {
                return HttpNotFound();
            }
            return View(tbldCluster);
        }

        // GET: DBhouseCluster/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DBhouseCluster/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,code,description")] tbld_cluster tbldCluster)
        {
            if (ModelState.IsValid)
            {
                db.tbld_cluster.Add(tbldCluster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldCluster);
        }

        // GET: DBhouseCluster/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_cluster tbldCluster = db.tbld_cluster.Find(id);
            if (tbldCluster == null)
            {
                return HttpNotFound();
            }
            return View(tbldCluster);
        }

        // POST: DBhouseCluster/Edit/5
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,code,description")] tbld_cluster tbldCluster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbldCluster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbldCluster);
        }

        // GET: DBhouseCluster/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_cluster tbldCluster = db.tbld_cluster.Find(id);
            if (tbldCluster == null)
            {
                return HttpNotFound();
            }
            return View(tbldCluster);
        }

        // POST: DBhouseCluster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_cluster tbldCluster = db.tbld_cluster.Find(id);
            if (tbldCluster != null) db.tbld_cluster.Remove(tbldCluster);
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
