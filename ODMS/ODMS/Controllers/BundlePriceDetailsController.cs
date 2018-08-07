using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class BundlePriceDetailsController : Controller
    {
        private ODMSEntities _db = new ODMSEntities();

      

        // GET: BundlePriceDetails/Details/5
        public ActionResult Details(int? id)
        {
            tbld_bundle_price_details tbldBundlePriceDetails = _db.tbld_bundle_price_details.Find(id);

            if (tbldBundlePriceDetails == null)
            {
                return HttpNotFound();
            }

            var firstOrDefault = _db.tbld_SKU.SingleOrDefault(x => x.SKU_id == tbldBundlePriceDetails.sku_id);
            if (firstOrDefault != null)
            {
                BundlePriceindexitemVm bundlePriceindexitemVm = new BundlePriceindexitemVm
                {
                    Id =tbldBundlePriceDetails.id,
                    BundlePriceId =tbldBundlePriceDetails.bundle_price_id,
                    SkuName = firstOrDefault.SKUName,
                    BatchId =tbldBundlePriceDetails.batch_id,
                    DbPrice =tbldBundlePriceDetails.db_lifting_price,
                    OutletPrice =tbldBundlePriceDetails.outlet_lifting_price,
                    Mrp =tbldBundlePriceDetails.mrp,
                    StartDate =tbldBundlePriceDetails.start_date,
                    EndDate = tbldBundlePriceDetails.end_date,
                    Status = tbldBundlePriceDetails.status == 1 ? "Active" : "InActive "
                
                };

                return View(bundlePriceindexitemVm);
            }
            return null;
        }

       

        // GET: BundlePriceDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_bundle_price_details tbldBundlePriceDetails = _db.tbld_bundle_price_details.Find(id);
            var skuName = _db.tbld_SKU.SingleOrDefault(x => x.SKU_id == tbldBundlePriceDetails.sku_id);
            if (tbldBundlePriceDetails == null)
            {
                return HttpNotFound();
            }

            if (skuName != null)
           
                ViewBag.SKuName = skuName.SKUName;

            ViewBag.IsActive = new SelectList(_db.status.ToList(), "status_Id", "status_code");
            
            return View(tbldBundlePriceDetails);
        }

        // POST: BundlePriceDetails/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tbld_bundle_price_details tbldBundlePriceDetails)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tbldBundlePriceDetails).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Details", "BundlePrice",new{id=tbldBundlePriceDetails.bundle_price_id});
            }
            ViewBag.IsActive = new SelectList(_db.status.ToList(), "status_Id", "status_code");
            return View(tbldBundlePriceDetails);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
