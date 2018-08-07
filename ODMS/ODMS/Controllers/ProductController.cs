using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class ProductController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

        // GET: Product
        public ActionResult Index()
        {
            var listItem = (from a in Db.tbld_SKU
                            join b in Db.tbld_SKU_Brand on a.SKUbrand_id equals b.id
                            join c in Db.tbld_SKU_unit on a.SKUUnit equals c.id
                            select new SkuiVM
                            {
                                SkuId = a.SKU_id,
                                SkuName = a.SKUName,
                                SkUcode = a.SKUcode,
                                SkUdescription = a.SKUdescription,
                                SkuStatus = a.SKUStatus == 1 ? "Active" : "InActive",
                                SkUlaunchdate = a.SKUcreationdate,
                                SkUbrand = b.element_name,
                                SkuVolumeMl = a.SKUVolume_ml,
                                SkuVolume8Oz = a.SKUVolume_8oz,
                                SkUlpc = a.SKUlpc,
                                SkuUnit = c.qty
                            }).ToList();

            return View(listItem);
        }

      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU tbldSku = Db.tbld_SKU.Find(id);
            if (tbldSku == null)
            {
                return HttpNotFound();
            }
            return View(tbldSku);
        }

        // GET: Product/Create
        public ActionResult Create()
        {

            ViewBag.SKUtype = new SelectList(Db.tbld_SKUtype, "SKUtypeId", "SKUtype");
            ViewBag.SKUContainer = new SelectList(Db.tbld_SKUContainertype, "ContainertypeId", "Containertype");
            ViewBag.SKUCategory = new SelectList(Db.tbld_sku_category, "Id", "sku_category_name");
            ViewBag.SKUbrand = new SelectList(Db.tbld_SKU_Brand.Where(x => x.element_category_id == 2).ToList(), "id", "element_name");
            ViewBag.SKUUnit = new SelectList(Db.tbld_SKU_unit.Where(x => x.qty > 1).ToList(), "id", "unit_name");
            ViewBag.SKUStatus = new SelectList(Db.status.ToList(), "status_Id", "status_code");

            return View();
        }

        // POST: Product/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Skuvm skuvm)
        {
           
            if (ModelState.IsValid)
            {

            
                    tbld_SKU tbldSku = new tbld_SKU
                    {
                            
                            SKUName = skuvm.SkuName,
                            SKUsl = skuvm.SkUsl,
                            SKUcode = skuvm.SkUcode,
                            SKUdescription = skuvm.SkUdescription,
                            SKUtype_id = skuvm.SkUtypeId,
                            SKUnodetype_id = skuvm.SkUnodetypeId,
                            SKUStatus = skuvm.SkuStatus,
                            SKUcreationdate = skuvm.SkUcreationdate,
                            SKUlaunchdate = skuvm.SkUlaunchdate,
                            SKUexpirydate = skuvm.SkUexpirydate,
                            SKUbrand_id = skuvm.SkUbrandId,
                            SKUcategoryid = skuvm.SkUcategoryid,
                            SKUVolume_ml = skuvm.SkuVolumeMl,
                            SKUVolume_8oz = skuvm.SkuVolume8Oz,
                            SKUName_B = skuvm.SkuNameB,
                            SKUContainertypeid = skuvm.SkuContainertypeid,
                            SKUlpc = skuvm.SkUlpc,
                            SKUUnit = skuvm.SkuUnit

                       };
                    Db.tbld_SKU.Add(tbldSku);
                    Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = skuvm.SkuName + "  Create Successfully";

                    return RedirectToAction("Index");
              }

          
            ViewBag.SKUtype = new SelectList(Db.tbld_SKUtype, "SKUtypeId", "SKUtype");
            ViewBag.SKUContainer = new SelectList(Db.tbld_SKUContainertype, "ContainertypeId", "Containertype");
            ViewBag.SKUCategory = new SelectList(Db.tbld_sku_category, "Id", "sku_category_name");
            ViewBag.SKUbrand = new SelectList(Db.tbld_SKU_Brand.Where(x => x.element_category_id == 2).ToList(), "id", "element_name");
            ViewBag.SKUUnit = new SelectList(Db.tbld_SKU_unit.Where(x => x.qty > 1).ToList(), "id", "unit_name");
            ViewBag.SKUStatus = new SelectList(Db.status.ToList(), "status_Id", "status_code");

            return View(skuvm);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU tbldSku = Db.tbld_SKU.Find(id);
            
            if (tbldSku == null)
            {
                return HttpNotFound();
            }


            Skuvm skuvm = new Skuvm
            {
                SkuId = tbldSku.SKU_id,
                SkuName = tbldSku.SKUName,
                SkUsl = tbldSku.SKUsl,
                SkUcode = tbldSku.SKUcode,
                SkUdescription = tbldSku.SKUdescription,
                SkUtypeId = tbldSku.SKUtype_id,
                SkUnodetypeId = tbldSku.SKUnodetype_id,
                SkuStatus = tbldSku.SKUStatus,
                SkUcreationdate = tbldSku.SKUcreationdate,
                SkUlaunchdate = tbldSku.SKUlaunchdate,
                SkUexpirydate = tbldSku.SKUexpirydate,
                SkUbrandId = tbldSku.SKUbrand_id,
                SkUcategoryid = tbldSku.SKUcategoryid,
                SkuVolumeMl = tbldSku.SKUVolume_ml,
                SkuVolume8Oz = tbldSku.SKUVolume_8oz,
                SkuNameB = tbldSku.SKUName_B,
                SkuContainertypeid = tbldSku.SKUContainertypeid,
                SkUlpc = tbldSku.SKUlpc,
                SkuUnit = tbldSku.SKUUnit

            };

            ViewBag.SKUtype = new SelectList(Db.tbld_SKUtype, "SKUtypeId", "SKUtype");
            ViewBag.SKUContainer = new SelectList(Db.tbld_SKUContainertype, "ContainertypeId", "Containertype");
            ViewBag.SKUCategory = new SelectList(Db.tbld_sku_category, "Id", "sku_category_name");
            ViewBag.brand = new SelectList(Db.tbld_SKU_Brand.Where(x => x.element_category_id == 2).ToList(), "id", "element_name");
            ViewBag.Unit = new SelectList(Db.tbld_SKU_unit.Where(x => x.qty > 1).ToList(), "id", "unit_name");
            ViewBag.Status = new SelectList(Db.status.ToList(), "status_Id", "status_code");

            return View(skuvm);
        }

        // POST: Product/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Skuvm skuvm)
        {
            if (ModelState.IsValid)
            {
                tbld_SKU tbldSku = new tbld_SKU
                {
                    SKU_id = skuvm.SkuId,
                    SKUName = skuvm.SkuName,
                    SKUsl = skuvm.SkUsl,
                    SKUcode = skuvm.SkUcode,
                    SKUdescription = skuvm.SkUdescription,
                    SKUtype_id = skuvm.SkUtypeId,
                    SKUnodetype_id = skuvm.SkUnodetypeId,
                    SKUStatus = skuvm.SkuStatus,
                    SKUcreationdate = skuvm.SkUcreationdate,
                    SKUlaunchdate = skuvm.SkUlaunchdate,
                    SKUexpirydate = skuvm.SkUexpirydate,
                    SKUbrand_id = skuvm.SkUbrandId,
                    SKUcategoryid = skuvm.SkUcategoryid,
                    SKUVolume_ml = skuvm.SkuVolumeMl,
                    SKUVolume_8oz = skuvm.SkuVolume8Oz,
                    SKUName_B = skuvm.SkuNameB,
                    SKUContainertypeid = skuvm.SkuContainertypeid,
                    SKUlpc = skuvm.SkUlpc,
                    SKUUnit = skuvm.SkuUnit

                };
                Db.Entry(tbldSku).State = EntityState.Modified;
                Db.SaveChanges();
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = skuvm.SkuName + "  Edit Successfully";

                return RedirectToAction("Index");
            }

            ViewBag.SKUtype = new SelectList(Db.tbld_SKUtype, "SKUtypeId", "SKUtype");
            ViewBag.SKUContainer = new SelectList(Db.tbld_SKUContainertype, "ContainertypeId", "Containertype");
            ViewBag.SKUCategory = new SelectList(Db.tbld_sku_category, "Id", "sku_category_name");
            ViewBag.brand = new SelectList(Db.tbld_SKU_Brand.Where(x => x.element_category_id == 2).ToList(), "id", "element_name");
            ViewBag.Unit = new SelectList(Db.tbld_SKU_unit.Where(x => x.qty > 1).ToList(), "id", "unit_name");
            ViewBag.Status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            return View(skuvm);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_SKU tbldSku = Db.tbld_SKU.Find(id);
            if (tbldSku == null)
            {
                return HttpNotFound();
            }
            return View(tbldSku);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_SKU tbldSku = Db.tbld_SKU.Find(id);
            if (tbldSku != null)
            {
                Db.tbld_SKU.Remove(tbldSku);
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = tbldSku.SKUName + "  Delete Successfully";
            }
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
