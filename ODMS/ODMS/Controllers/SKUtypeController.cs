using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class SkUtypeController : Controller
    {
        internal ODMSEntities Db = new ODMSEntities();

        // GET: SKUtype
        public ActionResult Index()
        {
            return View(Db.tbld_SKUtype.ToList());
        }

        // GET: SKUtype/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKUtype tbldSkUtype = Db.tbld_SKUtype.Find(id);
            if (tbldSkUtype == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkUtype);
        }

        // GET: SKUtype/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SKUtype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SKUtypeId,SKUtype")] tbld_SKUtype tbldSkUtype)
        {
            if (ModelState.IsValid)
            {
                Db.tbld_SKUtype.Add(tbldSkUtype);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldSkUtype);
        }

        // GET: SKUtype/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKUtype tbldSkUtype = Db.tbld_SKUtype.Find(id);
            if (tbldSkUtype == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkUtype);
        }

        // POST: SKUtype/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SKUtypeId,SKUtype")] tbld_SKUtype tbldSkUtype)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(tbldSkUtype).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbldSkUtype);
        }

        // GET: SKUtype/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKUtype tbldSkUtype = Db.tbld_SKUtype.Find(id);
            if (tbldSkUtype == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkUtype);
        }

        // POST: SKUtype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_SKUtype tbldSkUtype = Db.tbld_SKUtype.Find(id);
            if (tbldSkUtype != null) Db.tbld_SKUtype.Remove(tbldSkUtype);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
