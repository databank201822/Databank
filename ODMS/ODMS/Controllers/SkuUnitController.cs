using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class SkuUnitController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: SkuUnit
        public ActionResult Index()
        {
            return View(db.tbld_SKU_unit.ToList());
        }

        // GET: SkuUnit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU_unit tbld_SKU_unit = db.tbld_SKU_unit.Find(id);
            if (tbld_SKU_unit == null)
            {
                return HttpNotFound();
            }
            return View(tbld_SKU_unit);
        }

        // GET: SkuUnit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SkuUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,unit_name,unit_short_name,unit_code,unit_description,qty,height,width,length,weight")] tbld_SKU_unit tbld_SKU_unit)
        {
            if (ModelState.IsValid)
            {
                db.tbld_SKU_unit.Add(tbld_SKU_unit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbld_SKU_unit);
        }

        // GET: SkuUnit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU_unit tbld_SKU_unit = db.tbld_SKU_unit.Find(id);
            if (tbld_SKU_unit == null)
            {
                return HttpNotFound();
            }
            return View(tbld_SKU_unit);
        }

        // POST: SkuUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,unit_name,unit_short_name,unit_code,unit_description,qty,height,width,length,weight")] tbld_SKU_unit tbld_SKU_unit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbld_SKU_unit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbld_SKU_unit);
        }

        // GET: SkuUnit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU_unit tbld_SKU_unit = db.tbld_SKU_unit.Find(id);
            if (tbld_SKU_unit == null)
            {
                return HttpNotFound();
            }
            return View(tbld_SKU_unit);
        }

        // POST: SkuUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_SKU_unit tbld_SKU_unit = db.tbld_SKU_unit.Find(id);
            db.tbld_SKU_unit.Remove(tbld_SKU_unit);
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
