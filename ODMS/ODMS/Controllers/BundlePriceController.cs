using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class BundlePriceController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

       
        public ActionResult Index()
        {
            var listItem = (from a in Db.tbld_bundle_price
                            let skuCount =
                                (
                                  from c in Db.tbld_bundle_price_details
                                  where a.Id == c.bundle_price_id && c.status == 1
                                  select c
                                ).Count()

                            select new BundlePriceVm
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code,
                                Count = skuCount
                            }).ToList();


            return View(listItem.ToList());
        }

      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tbldBundlePrice = Db.tbld_bundle_price.Find(id);

            var bundlePriceItem = from a in Db.tbld_bundle_price_details
                join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                join c in Db.status on a.status equals c.status_Id
                where a.bundle_price_id == id && a.status==1
                                    orderby b.SKUbrand_id, b.SKU_id
                                    select new BundlePriceindexitemVm
                                      {
                                          Id = a.id,
                                          BundlePriceId = a.bundle_price_id,
                                          SkuName = b.SKUName,
                                          BatchId = a.batch_id,
                                          DbPrice = a.db_lifting_price,
                                          OutletPrice = a.outlet_lifting_price,
                                          Mrp = a.mrp,
                                          StartDate = a.start_date,
                                          EndDate = a.end_date,
                                          Status = c.status_code
                                      };

            BundlePriceindexVm bpiVm = new BundlePriceindexVm();

            if (tbldBundlePrice != null)
            {
                bpiVm.Id = tbldBundlePrice.Id;
                bpiVm.Name = tbldBundlePrice.Name;
                bpiVm.BundlePriceindexitemVm = bundlePriceItem.ToList();
               
            }
            return View(bpiVm);
        }

        public ActionResult Addnewproductprice(int id)
        {

            BundlePriceDetailsitemVm bundlePriceDetailsitemVm = new BundlePriceDetailsitemVm();
            var data = Db.tbld_bundle_price_details.Where(x => x.id == id);
            foreach (var item in data)
            {
                bundlePriceDetailsitemVm = new BundlePriceDetailsitemVm()
                {
                
                    BundlePriceId=item.bundle_price_id,
                    SkuId=item.sku_id,
                    BatchId=item.batch_id,
                    Quantity = 1,
                    StartDate = DateTime.Now

                };
            }

            var skuname = Db.tbld_SKU.Where(x =>x.SKU_id==bundlePriceDetailsitemVm.SkuId);

            foreach (var item in skuname)
            {
                ViewBag.SkuName = item.SKUName;
            }

            ViewBag.SKUStatus = new SelectList(Db.status.ToList(), "status_Id", "status_code");


            return PartialView(bundlePriceDetailsitemVm);
        }


        [HttpPost]
        public ActionResult Addnewproductprice(BundlePriceDetailsitemVm bundlePriceDetailsitemVm)
        {
            var bundlePriceDetails = Db.tbld_bundle_price_details.OrderByDescending(u => u.id).FirstOrDefault(x => x.sku_id == bundlePriceDetailsitemVm.SkuId);
            if (bundlePriceDetails != null)
            {
                int batchId = bundlePriceDetails.batch_id;

           //need previous betch inactive

                if (ModelState.IsValid)
                {
                    tbld_bundle_price_details tbldBundlePriceDetails = new tbld_bundle_price_details
                    {
                        quantity = bundlePriceDetailsitemVm.Quantity,
                        batch_id = batchId+1,
                        bundle_price_id = bundlePriceDetailsitemVm.BundlePriceId,
                        sku_id = bundlePriceDetailsitemVm.SkuId,
                        db_lifting_price = bundlePriceDetailsitemVm.DbLiftingPrice,
                        outlet_lifting_price = bundlePriceDetailsitemVm.OutletLiftingPrice,
                        mrp = bundlePriceDetailsitemVm.Mrp,
                        start_date = bundlePriceDetailsitemVm.StartDate,
                        end_date = bundlePriceDetailsitemVm.EndDate,
                        status = bundlePriceDetailsitemVm.Status

                    };

                    Db.tbld_bundle_price_details.Add(tbldBundlePriceDetails);

                    Db.tbld_bundle_price_details.Where(x => x.batch_id == batchId - 1 && x.sku_id == bundlePriceDetailsitemVm.SkuId && x.status!=2).ToList().ForEach(x =>
                    {
                        x.status = 2;
                        x.end_date = DateTime.Now;
                    });

                    Db.SaveChanges();
                    return Json("Save", JsonRequestBehavior.AllowGet);
                    //return Redirect("/BundlePrice/Details/"+BundlePriceDetailsitemVM.bundle_price_id);
                }
            }


            return Json("Error", JsonRequestBehavior.AllowGet);


        }
      

        public ActionResult Addnewproduct(int id)
        {
            BundlePriceDetailsitemVm bundlePriceDetailsitemVm = new BundlePriceDetailsitemVm
            {
                BundlePriceId = id,
                BatchId = 100,
                Quantity = 1,
                StartDate = DateTime.Now
            };


            HashSet<int> expectList = new HashSet<int>(Db.tbld_bundle_price_details.Where(x => x.bundle_price_id == id).Select(x => x.sku_id));

            var allskulist = Db.tbld_SKU.Where(x => !expectList.Contains(x.SKU_id)).ToList();
            ViewBag.SkuList = new SelectList(allskulist, "SKU_id", "SKUName");
            ViewBag.SKUStatus = new SelectList(Db.status.ToList(), "status_Id", "status_code");


            return PartialView(bundlePriceDetailsitemVm);
        }
        [HttpPost]
        public ActionResult AddnewProductinbundle(BundlePriceDetailsitemVm bundlePriceDetailsitemVm)
        {
            
          
            if (ModelState.IsValid)
            {
                tbld_bundle_price_details tbldBundlePriceDetails = new tbld_bundle_price_details
                {
                    quantity = bundlePriceDetailsitemVm.Quantity,
                    batch_id = bundlePriceDetailsitemVm.BatchId,
                    bundle_price_id = bundlePriceDetailsitemVm.BundlePriceId,
                    sku_id = bundlePriceDetailsitemVm.SkuId,
                    db_lifting_price = bundlePriceDetailsitemVm.DbLiftingPrice,
                    outlet_lifting_price = bundlePriceDetailsitemVm.OutletLiftingPrice,
                    mrp = bundlePriceDetailsitemVm.Mrp,
                    start_date =bundlePriceDetailsitemVm.StartDate ,
                    end_date = bundlePriceDetailsitemVm.EndDate,
                    status = bundlePriceDetailsitemVm.Status

                };
                Db.tbld_bundle_price_details.Add(tbldBundlePriceDetails);
                Db.SaveChanges();

                return Json("Save", JsonRequestBehavior.AllowGet);
                 //return Redirect("/BundlePrice/Details/"+BundlePriceDetailsitemVM.bundle_price_id);
            }

            return Json("Error", JsonRequestBehavior.AllowGet);


        }
      
        public ActionResult Create()
        {
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code")] tbld_bundle_price tbldBundlePrice)
        {
            tbldBundlePrice.Created_Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                Db.tbld_bundle_price.Add(tbldBundlePrice);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbldBundlePrice);
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
