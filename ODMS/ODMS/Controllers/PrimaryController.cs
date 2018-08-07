using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class PrimaryController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        // GET: Primary
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrimaryChallan(List<PrimaryReceiveVm> primaryVm, int challanid, DateTime challanDate)
        {
            int dbid = (int)Session["DBId"];

            var tbldDistributionHouse = Db.tbld_distribution_house.SingleOrDefault(y => y.DB_Id == dbid);

            if (tbldDistributionHouse != null)
            {
               

                var primaryReceiveLineList = (from b in primaryVm
                                              let skusingleOrDefault = Db.tbld_SKU.SingleOrDefault(x => x.SKUcode == b.ProductID)
                                              where skusingleOrDefault != null
                                              let tbldSkuUnit = Db.tbld_SKU_unit.SingleOrDefault(x => x.id == skusingleOrDefault.SKUUnit)
                                              where tbldSkuUnit != null

                                              select new PrimaryReceiveLineVm
                                              {
                                                  PrimaryChallanno = challanid,
                                                  PrimaryChallanDate=challanDate,
                                                  SkuName = skusingleOrDefault.SKUName,
                                                  SkuId = skusingleOrDefault.SKU_id,
                                                  PackSize = tbldSkuUnit.qty,
                                                  ChallanQuantityCs = (int)b.Quantitycs,
                                                  ChallanQuantityPs = ((int)(b.Quantitycs * tbldSkuUnit.qty) % tbldSkuUnit.qty),
                                                  ChallanQuantity = (int)(b.Quantitycs * tbldSkuUnit.qty),
                                                  ReceiveQuantityCs = (int)b.Quantitycs,
                                                  ReceiveQuantityPs = ((int)(b.Quantitycs * tbldSkuUnit.qty) % tbldSkuUnit.qty),
                                                  ReceiveQuantity = (int)(b.Quantitycs * tbldSkuUnit.qty)
                                              }).ToList();



                return PartialView(primaryReceiveLineList);
            }
            return null;
        }

        public ActionResult SavePrimaryChallan(List<PrimaryReceiveLineVm> primaryReceiveLineVm)
        {
            tblt_PurchaseOrder purchaseOrder = new tblt_PurchaseOrder();

            int dbid = (int)Session["DBId"];

            DateTime systemDate = (DateTime)Session["SystemDate"];
            var singleOrDefault = primaryReceiveLineVm.FirstOrDefault();

            var primaryChallanExist = Db.tblt_PurchaseOrder.FirstOrDefault(x => x.ChallanNo == singleOrDefault.PrimaryChallanno);
            if (primaryChallanExist == null)
            {
                if (singleOrDefault != null)
                {
                    purchaseOrder.ChallanNo = singleOrDefault.PrimaryChallanno;
                    purchaseOrder.ChallanDate = singleOrDefault.PrimaryChallanDate;
                }
                purchaseOrder.ReceivedDate = systemDate;
                purchaseOrder.Timestamp=DateTime.Now;
                purchaseOrder.DbId = dbid;

                Db.tblt_PurchaseOrder.Add(purchaseOrder);
                Db.SaveChanges();

                int id = purchaseOrder.Id;
                

                foreach (var item in primaryReceiveLineVm)
                {
                    var latestBatch = (from a in Db.tbld_distribution_house
                                       join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                                       where a.DB_Id == dbid && b.sku_id == item.SkuId && b.status == 1
                                       orderby b.batch_id descending
                                       select new { b.batch_id, b.db_lifting_price }).FirstOrDefault();

                    var tbltInventory = Db.tblt_inventory
                        .Where(x => x.dbId == dbid && x.skuId == item.SkuId && x.batchNo == latestBatch.batch_id)
                        .Select(x => new { x.id, x.dbId, x.skuId, x.BundleItemid, x.packSize, x.batchNo, x.qtyPs })
                        .SingleOrDefault();

                    if (latestBatch != null)
                    {
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
                                batchNo = latestBatch.batch_id,
                                qtyPs = tbltInventory.qtyPs + item.ReceiveQuantity,
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
                                BundleItemid = 0,
                                packSize = item.PackSize,
                                batchNo = latestBatch.batch_id,
                                qtyPs = item.ReceiveQuantity,
                            };
                            Db.tblt_inventory.Add(tbltinventory);
                        }

                        tblt_PurchaseOrderLine tbltPurchaseOrderLine = new tblt_PurchaseOrderLine()
                        {
                            POId = id,
                            sku_id = item.SkuId,
                            BundelItem = 0,
                            BatchId = latestBatch.batch_id,
                            Price = latestBatch.db_lifting_price,
                            PackSize = item.PackSize,
                            ChallanQty = item.ChallanQuantity,
                            ReciveQty = item.ReceiveQuantity
                        };
                        Db.tblt_PurchaseOrderLine.Add(tbltPurchaseOrderLine);
                        // Add inventory log
                        tbll_inventory_log tbllInventoryLog = new tbll_inventory_log
                        {
                            db_id = dbid,
                            sku_id = item.SkuId,
                            batch_id = latestBatch.batch_id,
                            price = 0,
                            tx_qty_ps = item.ReceiveQuantity,
                            tx_type = 1,
                            tx_System_date = systemDate,
                            tx_date = DateTime.Now,
                            tx_challan = purchaseOrder.ChallanNo
                        };

                        Db.tbll_inventory_log.Add(tbllInventoryLog);
                        
                    }
                    Db.SaveChanges();
                }
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = "Challan Receive successfully";
                return RedirectToAction("Index");
            }
            TempData["alertbox"] = "error";
            TempData["alertboxMsg"] = "Challan Already Received";
            return RedirectToAction("Index");
        }
    }
}