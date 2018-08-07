using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire, DBAccess]
    public class PurchaseController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRow(int count, int[] bundelitem, int[] skuList)
        {
            int dbid = (int)Session["DBId"];
            ViewBag.count = count;

            var dblist = from a in Db.tbld_distribution_house
                         join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                         join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                         where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1 && !bundelitem.Contains(b.id)
                         select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });
            ViewBag.SKUList = new SelectList(dblist.ToList(), "bundleid", "SKUName");
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetSkuDetailbyBundelId(int itemid)
        {
            var skuitemDetails = (from a in Db.tbld_bundle_price_details
                                  join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                                  join c in Db.tbld_SKU_unit on b.SKUUnit equals c.id
                                  where a.id == itemid
                                  select new
                                  {
                                      skuid = a.sku_id,
                                      unit = c.qty,
                                      db_price = a.db_lifting_price,
                                      batch = a.batch_id,
                                      lpsc = b.SKUlpc
                                  }).ToList();


            return Json(skuitemDetails, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PurchaseSave(PurchaseInsertVM purchaseInsertVm)
        {

          
            DateTime systemDate = (DateTime)Session["SystemDate"];
                int dbid = (int) Session["DBId"];
            tblt_PurchaseOrder purchaseOrder = new tblt_PurchaseOrder
            {
                ChallanNo = purchaseInsertVm.ChallanNo,
                ChallanDate = purchaseInsertVm.ChallanDate,
                ReceivedDate = systemDate,
                Timestamp = DateTime.Now,
                DbId = dbid
            };


                Db.tblt_PurchaseOrder.Add(purchaseOrder);
                int id = purchaseOrder.Id;

                foreach (var item in purchaseInsertVm.PurchaseLine)
                {

                    var tbltInventory = Db.tblt_inventory
                        .Where(x => x.dbId == dbid && x.skuId == item.SkuId && x.batchNo == item.BetchId)
                        .Select(x => new {x.id, x.dbId, x.skuId, x.BundleItemid, x.packSize, x.batchNo, x.qtyPs})
                        .SingleOrDefault();

                    tblt_inventory tbltinventory;
                    if (tbltInventory != null)
                    {
                        //Have Inventory



                        tbltinventory = new tblt_inventory
                        {
                            id = tbltInventory.id,
                            dbId = tbltInventory.dbId,
                            skuId = tbltInventory.skuId,
                            BundleItemid = tbltInventory.BundleItemid,
                            packSize = tbltInventory.packSize,
                            batchNo = tbltInventory.batchNo,
                            qtyPs = tbltInventory.qtyPs + item.ChallanQuantity,
                        };
                        Db.Entry(tbltinventory).State = EntityState.Modified;
                    }
                    else
                    {
                        //add new line
                        tbltinventory = new tblt_inventory
                        {

                            dbId = dbid,
                            skuId = item.SkuId,
                            BundleItemid = item.Bundelitemid,
                            packSize = item.PackSize,
                            batchNo = item.BetchId,
                            qtyPs = item.ChallanQuantity,
                        };
                        Db.tblt_inventory.Add(tbltinventory);
                    }



                    tblt_PurchaseOrderLine tbltPurchaseOrderLine = new tblt_PurchaseOrderLine()
                    {
                        POId = id,
                        sku_id = item.SkuId,
                        BundelItem = item.Bundelitemid,
                        BatchId = item.BetchId,
                        Price = item.UnitSalePrice,
                        PackSize = item.PackSize,
                        ChallanQty = item.ChallanQuantity,
                        ReciveQty = item.ChallanQuantity
                    };
                    Db.tblt_PurchaseOrderLine.Add(tbltPurchaseOrderLine);

                    // Add inventory log
                    tbll_inventory_log tbllInventoryLog = new tbll_inventory_log
                    {
                        db_id = dbid,
                        sku_id = item.SkuId,
                        batch_id = item.BetchId,
                        price = 0,
                        tx_qty_ps = item.ChallanQuantity,
                        tx_type = 1,
                        tx_System_date = systemDate,
                        tx_date = DateTime.Now,
                        tx_challan = purchaseOrder.ChallanNo
                    };

                    Db.tbll_inventory_log.Add(tbllInventoryLog);
                }
                Db.SaveChanges();


                return Json("Success", JsonRequestBehavior.AllowGet);
          
        }
    }



}