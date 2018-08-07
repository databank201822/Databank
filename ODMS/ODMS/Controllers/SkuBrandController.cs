using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire,SuperAccess]
    public class SkuBrandController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: SkuBrand
        public ActionResult Index()
        {
            var data = from a in db.tbld_SKU_Brand
                select new SkuBrand
                {
                    Id = a.id,
                    BrandName = a.element_name,
                    BrandCode = a.element_code,
                    BrandDescription = a.element_description,
                    Category =a.element_category_id==1?"Brand":"Sub-Brand",
                    Parent = db.tbld_SKU_Brand.FirstOrDefault(x=>x.id==a.parent_element_id).element_name
                };

            return View(data.ToList());
        }

        // GET: SkuBrand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tbldSkuBrand = (from a in db.tbld_SKU_Brand
                                where a.id==id
                select new SkuBrand
                {
                    Id = a.id,
                    BrandName = a.element_name,
                    BrandCode = a.element_code,
                    BrandDescription = a.element_description,
                    Category =a.element_category_id==1?"Brand":"Sub-Brand",
                    Parent = db.tbld_SKU_Brand.FirstOrDefault(x=>x.id==a.parent_element_id).element_name
                }).FirstOrDefault();
            
            return View(tbldSkuBrand);
        }

        // GET: SkuBrand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SkuBrand/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,element_name,element_code,element_description,element_category_id,parent_element_id")] tbld_SKU_Brand tbldSkuBrand)
        {
            if (ModelState.IsValid)
            {
                db.tbld_SKU_Brand.Add(tbldSkuBrand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldSkuBrand);
        }

        // GET: SkuBrand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU_Brand tbldSkuBrand = db.tbld_SKU_Brand.Find(id);
            if (tbldSkuBrand == null)
            {
                return HttpNotFound();
            }
            ViewBag.category = new SelectList(db.tbld_SKU_Brand_category.ToList(), "id", "brand_category");
            ViewBag.parentelement = new SelectList(db.tbld_SKU_Brand.Where(x => x.element_category_id == 1).ToList(), "id", "element_name");
           
            return View(tbldSkuBrand);
        }

        // POST: SkuBrand/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbld_SKU_Brand tbldSkuBrand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbldSkuBrand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbldSkuBrand);
        }

        // GET: SkuBrand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU_Brand tbldSkuBrand = db.tbld_SKU_Brand.Find(id);
            if (tbldSkuBrand == null)
            {
                return HttpNotFound();
            }
            return View(tbldSkuBrand);
        }

        // POST: SkuBrand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_SKU_Brand tbldSkuBrand = db.tbld_SKU_Brand.Find(id);
            if (tbldSkuBrand != null) db.tbld_SKU_Brand.Remove(tbldSkuBrand);
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
