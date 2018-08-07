using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class DBhouseOutletChannelController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: DBhouseOutletChannel
        public ActionResult Index()
        {
            return View(db.tbld_Outlet_channel.ToList());
        }

        // GET: DBhouseOutletChannel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_channel tbld_Outlet_channel = db.tbld_Outlet_channel.Find(id);
            if (tbld_Outlet_channel == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_channel);
        }

        // GET: DBhouseOutletChannel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DBhouseOutletChannel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,code,description")] tbld_Outlet_channel tbld_Outlet_channel)
        {
            if (ModelState.IsValid)
            {
                db.tbld_Outlet_channel.Add(tbld_Outlet_channel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbld_Outlet_channel);
        }

        // GET: DBhouseOutletChannel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_channel tbld_Outlet_channel = db.tbld_Outlet_channel.Find(id);
            if (tbld_Outlet_channel == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_channel);
        }

        // POST: DBhouseOutletChannel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,code,description")] tbld_Outlet_channel tbld_Outlet_channel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbld_Outlet_channel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbld_Outlet_channel);
        }

        // GET: DBhouseOutletChannel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_channel tbld_Outlet_channel = db.tbld_Outlet_channel.Find(id);
            if (tbld_Outlet_channel == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_channel);
        }

        // POST: DBhouseOutletChannel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_Outlet_channel tbld_Outlet_channel = db.tbld_Outlet_channel.Find(id);
            db.tbld_Outlet_channel.Remove(tbld_Outlet_channel);
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
