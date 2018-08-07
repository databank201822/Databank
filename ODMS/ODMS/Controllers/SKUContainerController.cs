using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class SkuContainerController : Controller
    {
        private ODMSEntities _db = new ODMSEntities();

        // GET: SKUContainer
        public ActionResult Index()
        {
            return View(_db.tbld_SKUContainertype.ToList());
        }

        // GET: SKUContainer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKUContainertype tbldSkuContainertype = _db.tbld_SKUContainertype.Find(id);
            if (tbldSkuContainertype == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuContainertype);
        }

        // GET: SKUContainer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SKUContainer/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContainertypeId,Containertype")] tbld_SKUContainertype tbldSkuContainertype)
        {
            if (ModelState.IsValid)
            {
                _db.tbld_SKUContainertype.Add(tbldSkuContainertype);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldSkuContainertype);
        }

        // GET: SKUContainer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKUContainertype tbldSkuContainertype = _db.tbld_SKUContainertype.Find(id);
            if (tbldSkuContainertype == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuContainertype);
        }

        // POST: SKUContainer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContainertypeId,Containertype")] tbld_SKUContainertype tbldSkuContainertype)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tbldSkuContainertype).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbldSkuContainertype);
        }

        // GET: SKUContainer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKUContainertype tbldSkuContainertype = _db.tbld_SKUContainertype.Find(id);
            if (tbldSkuContainertype == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuContainertype);
        }

        // POST: SKUContainer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_SKUContainertype tbldSkuContainertype = _db.tbld_SKUContainertype.Find(id);
            if (tbldSkuContainertype != null) _db.tbld_SKUContainertype.Remove(tbldSkuContainertype);
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
