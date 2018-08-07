using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class InventoryController : Controller
    {
        private ODMSEntities db = new ODMSEntities();
        // GET: Inventory

        public ActionResult Index()
        {
            int dbid = (int)Session["DBId"];

            var showInventoryByDbResult = (from a in db.RPT_CurrentStock(dbid)
                                           orderby a.sku_id ascending
                                           select new InventoryiVm
                                           {
                                               SkuId = a.sku_id,
                                               SkuName = a.SKUName,
                                               PackSize = a.packSize,
                                               TradePrice = a.outlet_lifting_price,
                                               BatchNo = a.batch_id,
                                               Currentqty = a.qtyPs,
                                               Bookedqty = a.bookedqty,
                                               Totalqty = a.totalqty ?? 0
                                           }).ToList();


            return View(showInventoryByDbResult);
        }

        public ActionResult StockAdjustment()
        {
            return View();

        }

        [HttpPost]
        public ActionResult StockAdjustmentFilter(int dbid)
        {
            int priceBuandleId = 0;
            var tbldDistributionHouse = db.tbld_distribution_house.SingleOrDefault(x => x.DB_Id == dbid);
            if (tbldDistributionHouse != null)
            {
                priceBuandleId = tbldDistributionHouse.PriceBuandle_id;
            }



            var skuList = from a in db.tbld_SKU
                          join d in db.tbld_bundle_price_details on a.SKU_id equals d.sku_id
                          join c in db.tbld_SKU_unit on a.SKUUnit equals c.id into qs
                          from q in qs.DefaultIfEmpty()
                          where a.SKUStatus == 1 && d.status == 1 && d.bundle_price_id == priceBuandleId
                          select new
                          {
                              SkuId = a.SKU_id,
                              SkuName = a.SKUName,
                              PackSize = q.qty,
                              BundleItemid = d.id,
                              d.batch_id
                          };
            List<InventoryAdjustmentVm> inventoryAdjustmentVm = new List<InventoryAdjustmentVm>();

            var showInventoryByDbResult = (from a in db.RPT_CurrentStock(dbid)
                                           orderby a.sku_id ascending
                                           select new InventoryiVm
                                           {
                                               SkuId = a.sku_id,
                                               SkuName = a.SKUName,
                                               PackSize = a.packSize,
                                               TradePrice = a.outlet_lifting_price,
                                               BatchNo = a.batch_id,
                                               Currentqty = a.qtyPs,
                                               Bookedqty = a.bookedqty,
                                               Totalqty = a.totalqty ?? 0
                                           }).ToList();
            foreach (var skuitem in skuList)
            {
                var singleOrDefault = showInventoryByDbResult
                    .SingleOrDefault(x => x.SkuId == skuitem.SkuId && x.BatchNo == skuitem.batch_id);
                int currentqty = 0;
                if (singleOrDefault != null)
                {
                    currentqty = singleOrDefault.Currentqty;
                }
                InventoryAdjustmentVm adjustmentItem = new InventoryAdjustmentVm
                {
                    DbId = dbid,
                    SkuId = skuitem.SkuId,
                    SkuName = skuitem.SkuName,
                    BatchNo = skuitem.batch_id,
                    BundleItemid = skuitem.BundleItemid,
                    PackSize = skuitem.PackSize,
                    Currentqty = currentqty,
                    CurrentCSqty = currentqty / skuitem.PackSize,
                    CurrentPSqty = currentqty % skuitem.PackSize,

                    AdjustmentCSqty = currentqty / skuitem.PackSize,
                    AdjustmentPSqty = currentqty % skuitem.PackSize

                };
                inventoryAdjustmentVm.Add(adjustmentItem);

            }




            return PartialView(inventoryAdjustmentVm);
        }

        [HttpPost]
        public ActionResult UpdateStock(List<InventoryAdjustmentVm> inventoryAdjustmentVm)
        {
            int userId = (int)Session["User_Id"];

            var item = inventoryAdjustmentVm.FirstOrDefault();
            DateTime syatemdate =db.tblt_System.Where(x => x.DBid == item.DbId).Select(x => x.CurrentDate).SingleOrDefault();

            if (item != null)
            {
                tbld_AdjustmentStock tbldAdjustmentStock = new tbld_AdjustmentStock
                {
                    date = DateTime.Now,
                    Syatemdate = syatemdate,
                    dbid = item.DbId,
                    userid = userId
                };

                db.tbld_AdjustmentStock.Add(tbldAdjustmentStock);
                db.SaveChanges();


                db.tblt_inventory.RemoveRange(db.tblt_inventory.Where(x => x.dbId == item.DbId).ToList());
                db.SaveChanges();

                foreach (var inventoryitem in inventoryAdjustmentVm)
                {


                    int previousQty = ((inventoryitem.CurrentCSqty * inventoryitem.PackSize) +
                                       inventoryitem.CurrentPSqty) ?? 0;
                    int currentQty = ((inventoryitem.AdjustmentCSqty * inventoryitem.PackSize) +
                                      inventoryitem.AdjustmentPSqty) ?? 0;

                    int adjustedQty = currentQty - previousQty;


                    tbld_AdjustmentStockItem tbldAdjustmentStockItem = new tbld_AdjustmentStockItem
                    {
                        AdjustmentStockID = tbldAdjustmentStock.id,
                        SKUID = inventoryitem.SkuId,
                        BatchNo = inventoryitem.BatchNo,
                        PreviousQty = previousQty,
                        CurrentQty = currentQty,
                        AdjustedQty = adjustedQty

                    };
                    db.tbld_AdjustmentStockItem.Add(tbldAdjustmentStockItem);
                    if (inventoryitem.PackSize != null)
                    {
                        tblt_inventory tbltInventory = new tblt_inventory
                        {
                            dbId = item.DbId,
                            skuId = inventoryitem.SkuId,
                            packSize = (int)inventoryitem.PackSize,
                            BundleItemid = inventoryitem.BundleItemid,
                            batchNo = inventoryitem.BatchNo,
                            qtyPs = currentQty
                        };
                        db.tblt_inventory.Add(tbltInventory);
                        db.SaveChanges();
                    }
                }
                //add  routeplan datils
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = " Adjustment Stock Successfully";

                return RedirectToAction("Index","Home");



            }
            return null;

        }
    }
}
