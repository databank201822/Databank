using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class SkUcategoryController : Controller
    {
        private ODMSEntities _db = new ODMSEntities();

        // GET: SKUcategory
        public ActionResult Index()
        {
            return View(_db.tbld_sku_category.ToList());
        }

        // GET: SKUcategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_sku_category tbldSkuCategory = _db.tbld_sku_category.Find(id);
            if (tbldSkuCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuCategory);
        }

        // GET: SKUcategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SKUcategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,sku_category_name")] tbld_sku_category tbldSkuCategory)
        {
            if (ModelState.IsValid)
            {
                _db.tbld_sku_category.Add(tbldSkuCategory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldSkuCategory);
        }

        // GET: SKUcategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_sku_category tbldSkuCategory = _db.tbld_sku_category.Find(id);
            if (tbldSkuCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuCategory);
        }

        // POST: SKUcategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,sku_category_name")] tbld_sku_category tbldSkuCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tbldSkuCategory).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbldSkuCategory);
        }

        // GET: SKUcategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_sku_category tbldSkuCategory = _db.tbld_sku_category.Find(id);
            if (tbldSkuCategory == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuCategory);
        }

        // POST: SKUcategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_sku_category tbldSkuCategory = _db.tbld_sku_category.Find(id);
            if (tbldSkuCategory != null) _db.tbld_sku_category.Remove(tbldSkuCategory);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
