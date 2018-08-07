using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class ChallanController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

        // GET: Challan
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ShowAllChallan(int challanStatus, DateTime dateFrom)
        {
            var status = challanStatus == 0 ? new List<int> { 1, 2, 3 } : new List<int> { challanStatus };

            var data = from a in Db.tblt_Challan
                       where a.order_date == dateFrom.Date && status.Contains(a.challan_status)
                       select new ChallanVm
                       {
                           Id = a.id,
                           ChallanNumber = a.challan_number,
                           PsrName = a.psr_Name,
                           RouteName = a.route_Name,
                           OrderDate = a.order_date,
                           DeliveryDate = a.delivery_date,
                           NoOfMemo = a.No_of_memo,
                           GrandTotalCs = a.grand_total_CS,
                           DeliveryGrandTotalCs = a.delivery_grand_total_CS,
                           GrandTotal = a.grand_total,
                           DeliveryGrandTotal = a.delivery_grand_total,
                           ChallanStatus = a.challan_status

                       };

            return PartialView(data.ToList());
        }

        public ActionResult Createchallan()
        {
            int dbid = (int)Session["DBId"];
            ViewBag.PSRList =
                new SelectList(
                    Db.tbld_distribution_employee.Where(x => x.DistributionId == dbid && x.Emp_Type == 2).ToList(),
                    "id", "Name");
            return View();
        }

        [HttpGet]
        public ActionResult SubroutebyPsr(int? psrId)
        {

            //var date = (DateTime)Session["SystemDate"];
           
            var subroutebyPsr = (from a in Db.tbld_Route_Plan_Mapping
                                 join b in Db.tbld_distributor_Route on a.route_id equals b.RouteID
                                where a.db_emp_id == psrId
                                 select (new { b.RouteID, b.RouteName })).Distinct().ToList();



            return Json(subroutebyPsr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult ShowAllForCreateChallan(DateTime orderdate, int psrid, int routeid)
        {


            var dbid = (int)Session["DBId"];

            var noOfMemo = Db.tblt_Order.Count(a => a.db_id == dbid && a.psr_id == psrid && a.route_id == routeid &&
                                                    a.isProcess == 1 && a.so_status == 1 &&
                                                    a.planned_order_date == orderdate);

            //var memo = Db.tblt_Order.FirstOrDefault(a => a.db_id == dbid && a.psr_id == psrid &&
            //                                             a.route_id == routeid &&
            //                                             a.isProcess == 1 && a.so_status == 1 &&
            //                                             a.planned_order_date == orderdate);

            var noOfnotprocess = Db.tblt_Order.Count(a => a.db_id == dbid && a.psr_id == psrid &&
                                                          a.route_id == routeid &&
                                                          a.isProcess == 0 && a.so_status == 1 &&
                                                          a.planned_order_date == orderdate);

            if (noOfnotprocess == 0)
            {
                if (noOfMemo > 0)
                {
                    ChallaniVm civm = new ChallaniVm
                    {
                        OrderDate = orderdate,
                        PsrId = psrid,
                        PsrName = Db.tbld_distribution_employee.Where(x => x.id == psrid).Select(x => x.Name)
                            .SingleOrDefault(),
                        RouteId = routeid,
                        RouteName = Db.tbld_distributor_Route.Where(x => x.RouteID == routeid).Select(x => x.RouteName)
                            .SingleOrDefault(),
                        NoOfMemo = noOfMemo

                    };

                    List<ChallanlineVm> challanlineList = new List<ChallanlineVm>();

                    var skuList = from a in Db.tbld_distribution_house
                                  join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                                  join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                                  join d in Db.tbld_SKU_unit on c.SKUUnit equals d.id
                                  where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1
                                  orderby c.SKUbrand_id
                                  select new ChallanlineVm
                                  {
                                      SkuId = b.sku_id,
                                      SkuName = c.SKUName,
                                      BatchId = b.batch_id,
                                      Price = b.outlet_lifting_price,
                                      PackSize = d.qty
                                  };

                    foreach (var skuListitem in skuList)
                    {
                        int orderQty = 0;
                        int freeQty = 0;
                        var orderQtysum = (from a in Db.tblt_Order
                                           join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                           where a.db_id == dbid && a.psr_id == psrid && a.route_id == routeid &&
                                                 a.isProcess == 1 & b.sku_id == skuListitem.SkuId &&
                                                 b.Betch_id == skuListitem.BatchId && b.sku_order_type_id == 1 && a.so_status == 1 &&
                                                 a.planned_order_date == orderdate && a.Challan_no == 0
                                           select (int?)b.quantity_delivered).Sum();

                        var freeQtysum = (from a in Db.tblt_Order
                                          join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                          where a.db_id == dbid && a.psr_id == psrid && a.route_id == routeid &&
                                                a.isProcess == 1 & b.sku_id == skuListitem.SkuId &&
                                                b.Betch_id == skuListitem.BatchId && b.sku_order_type_id == 2 && a.so_status == 1 &&
                                                a.planned_order_date == orderdate && a.Challan_no == 0
                                          select (int?)b.quantity_delivered).Sum();

                        if (orderQtysum != null)
                        {
                            orderQty = (int)orderQtysum;
                        }

                        if (freeQtysum != null)
                        {
                            freeQty = (int)freeQtysum;
                        }

                        if ((orderQty + freeQty) > 0)
                        {
                            ChallanlineVm challanlineVm = new ChallanlineVm
                            {
                                SkuId = skuListitem.SkuId,
                                SkuName = skuListitem.SkuName,
                                BatchId = skuListitem.BatchId,
                                Price = skuListitem.Price,
                                PackSize = skuListitem.PackSize,
                                StockQty = Db.tblt_inventory
                                    .Where(x => x.dbId == dbid && x.skuId == skuListitem.SkuId &&
                                                x.batchNo == skuListitem.BatchId).Select(x => x.qtyPs)
                                    .SingleOrDefault(),
                                OrderCsQty = orderQty / skuListitem.PackSize,
                                OrderPsQty = orderQty % skuListitem.PackSize,
                                OrderQty = orderQty,
                                FreeCsQty = freeQty / skuListitem.PackSize,
                                FreePsQty = freeQty % skuListitem.PackSize,
                                FreeQty = freeQty,
                                TotalQty = orderQty + freeQty,
                                TotalCsQty = (orderQty + freeQty) / skuListitem.PackSize,
                                TotalPsQty = (orderQty + freeQty) % skuListitem.PackSize,
                                TotalQtyPrice = skuListitem.Price * orderQty

                            };

                            challanlineList.Add(challanlineVm);
                        }
                    }



                    civm.Challanline = challanlineList.OrderByDescending(x => x.OrderCsQty).ToList();

                    return PartialView(civm);
                }
                return PartialView("EmptyOrder");
            }
            return PartialView("OrdernotProcess");
            
        }

        [HttpPost]
        public ActionResult AddRow(int count, int[] skuList)
        {
            int dbid = (int)Session["DBId"];
            ViewBag.count = count;

            var skulist = from a in Db.tbld_distribution_house
                          join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                          join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                          where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1 && !skuList.Contains(b.sku_id)
                          select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });

            ViewBag.SKUList = new SelectList(skulist.ToList(), "bundleid", "SKUName");

            return PartialView();
        }

        [HttpPost]
        public ActionResult GetSkuDetailbyBundelId(int itemid, int[] skuids)
        {

            var dbid = (int)Session["DBId"];

            ChallanlineVm challanlineVm = new ChallanlineVm();

            var skuList = (from a in Db.tbld_distribution_house
                           join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                           join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                           join d in Db.tbld_SKU_unit on c.SKUUnit equals d.id
                           where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1 && b.id == itemid &&
                                 !skuids.Contains(c.SKU_id)
                           orderby c.SKUbrand_id
                           select new ChallanlineVm
                           {
                               SkuId = b.sku_id,
                               SkuName = c.SKUName,
                               BatchId = b.batch_id,
                               Price = b.outlet_lifting_price,
                               PackSize = d.qty

                           }).SingleOrDefault();


            if (skuList != null)
            {
                challanlineVm = new ChallanlineVm
                {
                    SkuId = skuList.SkuId,
                    SkuName = skuList.SkuName,
                    BatchId = skuList.BatchId,
                    Price = skuList.Price,
                    PackSize = skuList.PackSize,
                    StockQty = Db.tblt_inventory
                        .Where(x => x.dbId == dbid && x.skuId == skuList.SkuId &&
                                    x.batchNo == skuList.BatchId).Select(x => x.qtyPs).SingleOrDefault(),
                };
            }


            return Json(challanlineVm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Addchallan(ChallaniVm challaniVm)
        {
            int dbid = (int)Session["DBId"];
            var date = (DateTime)Session["SystemDate"];

            var deliverydate = Db.tbld_Route_Plan_Detail.SingleOrDefault(x => x.route_id == challaniVm.RouteId && x.planned_visit_date == challaniVm.OrderDate);
            if (deliverydate == null)
            {
                challaniVm.DeliveryDate = date;
            }
            else
            {
                if (deliverydate.delivery_date != null) challaniVm.DeliveryDate = (DateTime)deliverydate.delivery_date;
            }

            if (challaniVm != null)
            {
                int count = Db.tblt_Challan.Count(x => x.System_date == date && x.db_id == dbid) + 1;
                string challanNumber = "C-" + challaniVm.OrderDate.ToString("yyyymmdd") + "-" + dbid + "-" + challaniVm.PsrId + "-" + count;
                tblt_Challan tbltChallan = new tblt_Challan
                {
                    challan_number = challanNumber,
                    db_id = dbid,
                    psr_id = challaniVm.PsrId,
                    psr_Name = challaniVm.PsrName,
                    route_id = challaniVm.RouteId,
                    route_Name = challaniVm.RouteName,
                    challan_status = 1,
                    No_of_memo = challaniVm.NoOfMemo,
                    order_date = challaniVm.OrderDate,
                    delivery_date = challaniVm.DeliveryDate,
                    Create_date_time = DateTime.Now,
                    System_date = date,
                    grand_total_CS = challaniVm.GrandTotalCs,
                    delivery_grand_total_CS = 0,
                    grand_total = challaniVm.GrandTotal,
                    delivery_grand_total = 0

                };


                Db.tblt_Challan.Add(tbltChallan);
                Db.SaveChanges();
                int challanid = tbltChallan.id;

                foreach (var challanlineitem in challaniVm.Challanline)
                {
                    tblt_Challan_line tbltChallanLine = new tblt_Challan_line
                    {

                        challan_id = challanid,
                        sku_id = challanlineitem.SkuId,
                        batch_id = challanlineitem.BatchId,
                        price = challanlineitem.Price,
                        Pack_size = challanlineitem.PackSize,
                        Order_qty = challanlineitem.OrderQty,
                        Extra_qty = challanlineitem.ExtraQty,
                        Free_qty = challanlineitem.FreeQty,
                        Total_qty = challanlineitem.TotalQty,
                        Confirm_qty = 0,
                        Confirm_Free_qty = 0,
                        Order_qty_price = challanlineitem.OrderQty * challanlineitem.Price,
                        Extra_qty_price = challanlineitem.ExtraQty * challanlineitem.Price,
                        Total_qty_price = (challanlineitem.OrderQty + challanlineitem.ExtraQty) * challanlineitem.Price,
                        Confirm_qty_price = 0
                    };
                    Db.tblt_Challan_line.Add(tbltChallanLine);
                  


                    //Update Inventory
                    Db.tblt_inventory.Where(x => x.dbId == dbid && x.skuId == challanlineitem.SkuId &&
                                                 x.batchNo == challanlineitem.BatchId).ToList().ForEach(x =>
                    {
                        x.qtyPs = x.qtyPs - challanlineitem.TotalQty;
                    });

                    // Add inventory log
                    tbll_inventory_log tbllInventoryLog = new tbll_inventory_log
                    {
                        db_id = dbid,
                        sku_id = challanlineitem.SkuId ,
                        batch_id = challanlineitem.BatchId,
                        price=0,
                        tx_qty_ps = challanlineitem.TotalQty,
                        tx_type=2,
                        tx_System_date = date,
                        tx_date = DateTime.Now,
                        tx_challan = challanid
                    };

                    Db.tbll_inventory_log.Add(tbllInventoryLog);
                    Db.SaveChanges();


                }

                //Update Order status and add challan number
                Db.tblt_Order.Where(a => a.db_id == dbid && a.psr_id == challaniVm.PsrId &&
                                         a.route_id == challaniVm.RouteId &&
                                         a.isProcess == 1 && a.so_status == 1 && a.so_status != 9 &&
                                         a.planned_order_date == challaniVm.OrderDate && a.Challan_no == 0).ToList()
                    .ForEach(x =>
                    {
                        x.Challan_no = challanid;
                        x.so_status = 2;
                    });
                Db.SaveChanges();



                return Json("Success", JsonRequestBehavior.AllowGet);

            }

            return Json("Error", JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShowConfirmChallan(int id)
        {


            var tbltChallan = Db.tblt_Challan.Find(id);
            ViewBag.firstmemoid = 0;
            var firstOrDefault = Db.tblt_Order.FirstOrDefault(x => x.Challan_no == id);
            if (firstOrDefault != null)
            {
                ViewBag.firstmemoid = firstOrDefault.Orderid;
            }

            if (tbltChallan != null)
            {
                ChallaniVm civm = new ChallaniVm
                {
                    Id = id,
                    OrderDate = tbltChallan.order_date,
                    PsrId = tbltChallan.psr_id,
                    PsrName = tbltChallan.psr_Name,
                    RouteId = tbltChallan.route_id,
                    RouteName = tbltChallan.route_Name,
                    NoOfMemo = tbltChallan.No_of_memo,
                    DeliveryDate = tbltChallan.delivery_date

                };


                List<ChallanlineVm> challanlineVmList = new List<ChallanlineVm>();

                var skuList = from a in Db.tblt_Challan_line
                              join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                              where a.challan_id == id
                              select a;


                foreach (var skuListitem in skuList)
                {
                    int confirmQty = 0;
                    int freeQty = 0;
                    var orderQtysum = (from a in Db.tblt_Order
                                       join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                       where b.sku_id == skuListitem.sku_id &&
                                             b.Betch_id == skuListitem.batch_id && b.sku_order_type_id == 1 && a.so_status != 9 && a.Challan_no == id
                                       select (int?)b.quantity_delivered).Sum();

                    var freeQtysum = (from a in Db.tblt_Order
                                      join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                      where b.sku_id == skuListitem.sku_id &&
                                            b.Betch_id == skuListitem.batch_id && b.sku_order_type_id == 2 && a.so_status != 9 && a.Challan_no == id
                                      select (int?)b.quantity_delivered).Sum();

                    if (orderQtysum != null)
                    {
                        confirmQty = (int)orderQtysum;
                    }

                    if (freeQtysum != null)
                    {
                        freeQty = (int)freeQtysum;
                    }
                    ChallanlineVm challanlineVm = new ChallanlineVm
                    {
                        SkuId = skuListitem.sku_id,
                        SkuName = Db.tbld_SKU.Where(x => x.SKU_id == skuListitem.sku_id).Select(x => x.SKUName).SingleOrDefault(),
                        BatchId = skuListitem.batch_id,
                        Price = skuListitem.price,
                        PackSize = skuListitem.Pack_size,

                        FreeCsQty = freeQty / skuListitem.Pack_size,
                        FreePsQty = freeQty % skuListitem.Pack_size,
                        FreeQty = freeQty,

                        TotalCsQty = skuListitem.Total_qty / skuListitem.Pack_size,
                        TotalPsQty = skuListitem.Total_qty % skuListitem.Pack_size,
                        TotalQty = skuListitem.Total_qty,

                        ConfirmQty = confirmQty + freeQty,
                        ConfirmCsQty = (confirmQty + freeQty) / skuListitem.Pack_size,
                        ConfirmPsQty = (confirmQty + freeQty) % skuListitem.Pack_size,
                        ReturnQty = skuListitem.Total_qty - (confirmQty + freeQty)

                    };

                    challanlineVmList.Add(challanlineVm);
                    civm.GrandTotalCs = civm.GrandTotalCs + Convert.ToDouble((skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) / skuListitem.Pack_size);
                    civm.GrandTotal = civm.GrandTotal + Convert.ToDouble((skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) * skuListitem.price);
                    civm.DeliveryGrandTotalCs = civm.GrandTotalCs + Convert.ToDouble((confirmQty + freeQty) / skuListitem.Pack_size);
                    civm.DeliveryGrandTotal = civm.GrandTotal + Convert.ToDouble((confirmQty + freeQty) * skuListitem.price);
                }





                civm.Challanline = challanlineVmList.ToList();

                return View(civm);
            }
            return PartialView("EmptyOrder");
        }


        [HttpPost]
        public ActionResult ConfirmChallan(int id)
        {
            int confirmChallanFlag = 0;
            var date = (DateTime)Session["SystemDate"];
            var tbltChallan = Db.tblt_Challan.Find(id);


            List<ChallanlineVm> challanlineVmList = new List<ChallanlineVm>();
            if (tbltChallan != null)
            {
                var skuList = from a in Db.tblt_Challan_line
                              join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                              where a.challan_id == id
                              select a;

                foreach (var skuListitem in skuList)
                {
                    int confirmQty = 0;
                    int freeQty = 0;
                    var orderQtysum = (from a in Db.tblt_Order
                                       join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                       where b.sku_id == skuListitem.sku_id &&
                                             b.Betch_id == skuListitem.batch_id && b.sku_order_type_id == 1 && a.so_status != 9 && a.Challan_no == id
                                       select (int?)b.quantity_delivered).Sum();

                    var freeQtysum = (from a in Db.tblt_Order
                                      join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                      where b.sku_id == skuListitem.sku_id &&
                                            b.Betch_id == skuListitem.batch_id && b.sku_order_type_id == 2 && a.so_status != 9 && a.Challan_no == id
                                      select (int?)b.quantity_delivered).Sum();

                    if (orderQtysum != null)
                    {
                        confirmQty = (int)orderQtysum;
                    }

                    if (freeQtysum != null)
                    {
                        freeQty = (int)freeQtysum;
                    }

                    int challanQtyGap = skuListitem.Total_qty - (confirmQty + freeQty);

                    confirmChallanFlag = challanQtyGap < 0 ? 0 : 1;

                    ChallanlineVm challanlineVm = new ChallanlineVm
                    {
                        ChallanId = skuListitem.challan_id,
                        SkuId = skuListitem.sku_id,
                        BatchId = skuListitem.batch_id,
                        PackSize = skuListitem.Pack_size,
                        Price = skuListitem.price,
                        TotalQty = skuListitem.Total_qty,
                        ConfirmQty = confirmQty,
                        ConfirmFreeQty = freeQty,
                        ReturnQty = skuListitem.Total_qty - (confirmQty + freeQty)

                    };

                    challanlineVmList.Add(challanlineVm);

                }

                Double deliveryGrandTotalCs = 0;
                Double deliveryGrandTotal = 0;

                if (confirmChallanFlag == 1)
                {

                    foreach (var challanlineitem in challanlineVmList)
                    {
                        Db.tblt_Challan_line.Where(x => x.challan_id == challanlineitem.ChallanId && x.sku_id == challanlineitem.SkuId && x.batch_id == challanlineitem.BatchId).ToList()
                            .ForEach(x =>
                            {
                                x.Confirm_qty = challanlineitem.ConfirmQty;
                                x.Confirm_Free_qty = challanlineitem.ConfirmFreeQty;
                                x.Return_qty = challanlineitem.ReturnQty;
                                x.Confirm_qty_price = challanlineitem.ConfirmQty * x.price;
                            });
                   

                        //Update Inventory
                        Db.tblt_inventory.Where(x => x.dbId == tbltChallan.db_id && x.skuId == challanlineitem.SkuId &&
                                                     x.batchNo == challanlineitem.BatchId).ToList().ForEach(x =>
                        {
                            x.qtyPs = x.qtyPs + challanlineitem.ReturnQty;
                        });


                        // Add inventory log
                        tbll_inventory_log tbllInventoryLog = new tbll_inventory_log
                        {
                            db_id = tbltChallan.db_id,
                            sku_id = challanlineitem.SkuId,
                            batch_id = challanlineitem.BatchId,
                            price = 0,
                            tx_qty_ps = challanlineitem.ReturnQty,
                            tx_type = 3,
                            tx_System_date = date,
                            tx_date = DateTime.Now,
                            tx_challan = challanlineitem.ChallanId
                        };

                        Db.tbll_inventory_log.Add(tbllInventoryLog);

                        Db.SaveChanges();


                        
                        deliveryGrandTotalCs = deliveryGrandTotalCs + ((Double)challanlineitem.ConfirmQty / challanlineitem.PackSize);
                        deliveryGrandTotal = deliveryGrandTotal + (challanlineitem.ConfirmQty * challanlineitem.Price);
                    }

                    //Update Order status and add challan number
                    Db.tblt_Challan.Where(a => a.id == tbltChallan.id && a.challan_status == 1).ToList()
                        .ForEach(x =>
                        {
                            x.delivery_grand_total_CS = deliveryGrandTotalCs;
                            x.delivery_grand_total = deliveryGrandTotal;
                            x.challan_status = 2;
                        });
                    Db.SaveChanges();
                    //Update Order status and add challan number
                    Db.tblt_Order.Where(a => a.Challan_no == tbltChallan.id && a.so_status != 9).ToList()
                        .ForEach(x =>
                        {
                            x.delivery_date = tbltChallan.delivery_date;
                            x.so_status = 3;
                        });
                    Db.SaveChanges();






                    // return RedirectToAction("index", new { id });
                    return RedirectToAction("Index");
                }
                else if (confirmChallanFlag == 0)
                {

                    return RedirectToAction("ShowConfirmChallan", new { id });



                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DetailsChallan(int id)
        {


            var tbltChallan = Db.tblt_Challan.Find(id);

            if (tbltChallan != null)
            {
                ChallaniVm civm = new ChallaniVm
                {
                    Id = id,
                    OrderDate = tbltChallan.order_date,
                    PsrId = tbltChallan.psr_id,
                    PsrName = tbltChallan.psr_Name,
                    RouteId = tbltChallan.route_id,
                    RouteName = tbltChallan.route_Name,
                    NoOfMemo = tbltChallan.No_of_memo,
                    ChallanStatus = tbltChallan.challan_status

                };


                List<ChallanlineVm> challanlineVmList = new List<ChallanlineVm>();

                var skuList = from a in Db.tblt_Challan_line
                              join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                              where a.challan_id == id
                              select a;

                if (tbltChallan.challan_status != 2)
                {
                    foreach (var skuListitem in skuList)
                    {
                        ChallanlineVm challanlineVm = new ChallanlineVm
                        {
                            SkuId = skuListitem.sku_id,
                            SkuName = Db.tbld_SKU.Where(x => x.SKU_id == skuListitem.sku_id).Select(x => x.SKUName).SingleOrDefault(),
                            BatchId = skuListitem.batch_id,
                            Price = skuListitem.price,
                            PackSize = skuListitem.Pack_size,

                            FreeCsQty = skuListitem.Free_qty / skuListitem.Pack_size,
                            FreePsQty = skuListitem.Free_qty % skuListitem.Pack_size,
                            FreeQty = skuListitem.Free_qty,

                            OrderCsQty = skuListitem.Order_qty / skuListitem.Pack_size,
                            OrderPsQty = skuListitem.Order_qty % skuListitem.Pack_size,
                            OrderQty = skuListitem.Total_qty,
                            ExtraQty = skuListitem.Extra_qty,

                            TotalCsQty = (skuListitem.Total_qty - skuListitem.Free_qty) / skuListitem.Pack_size,
                            TotalPsQty = (skuListitem.Total_qty - skuListitem.Free_qty) % skuListitem.Pack_size,
                            TotalQty = skuListitem.Total_qty - skuListitem.Free_qty,
                            OrderQtyPrice = skuListitem.Order_qty_price,

                            ReturnQty = skuListitem.Total_qty - skuListitem.Free_qty,

                        };

                        challanlineVmList.Add(challanlineVm);
                        civm.GrandTotalCs = civm.GrandTotalCs + Convert.ToDouble((skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) / skuListitem.Pack_size);
                        civm.GrandTotal = civm.GrandTotal + Convert.ToDouble((skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) * skuListitem.price);
                        civm.DeliveryGrandTotalCs = 0;
                        civm.DeliveryGrandTotal = 0;
                    }
                }
                else
                {

                    foreach (var skuListitem in skuList)
                    {

                        ChallanlineVm challanlineVm = new ChallanlineVm
                        {
                            SkuId = skuListitem.sku_id,
                            SkuName = Db.tbld_SKU.Where(x => x.SKU_id == skuListitem.sku_id).Select(x => x.SKUName)
                                .SingleOrDefault(),
                            BatchId = skuListitem.batch_id,
                            Price = skuListitem.price,
                            PackSize = skuListitem.Pack_size,

                            FreeCsQty = skuListitem.Confirm_Free_qty / skuListitem.Pack_size,
                            FreePsQty = skuListitem.Confirm_Free_qty % skuListitem.Pack_size,
                            FreeQty = skuListitem.Confirm_Free_qty,

                            OrderCsQty = skuListitem.Order_qty / skuListitem.Pack_size,
                            OrderPsQty = skuListitem.Order_qty % skuListitem.Pack_size,
                            OrderQty = skuListitem.Total_qty,
                            ExtraQty = skuListitem.Extra_qty,

                            TotalCsQty = skuListitem.Order_qty / skuListitem.Pack_size,
                            TotalPsQty = skuListitem.Order_qty % skuListitem.Pack_size,
                            TotalQty = skuListitem.Order_qty,
                            OrderQtyPrice = skuListitem.Order_qty_price,

                            ConfirmQty = skuListitem.Confirm_qty,
                            ConfirmCsQty = skuListitem.Confirm_qty / skuListitem.Pack_size,
                            ConfirmPsQty = skuListitem.Confirm_qty % skuListitem.Pack_size,
                            ConfirmQtyPrice = skuListitem.Confirm_qty_price,
                            ReturnQty = skuListitem.Total_qty - (skuListitem.Confirm_qty + skuListitem.Confirm_Free_qty)

                        };

                        challanlineVmList.Add(challanlineVm);
                        civm.GrandTotalCs = civm.GrandTotalCs +
                                            Convert.ToDouble(
                                                (skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) /
                                                skuListitem.Pack_size);
                        civm.GrandTotal = civm.GrandTotal +
                                          Convert.ToDouble(
                                              (skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) *
                                              skuListitem.price);
                        civm.DeliveryGrandTotalCs = civm.GrandTotalCs +
                                                    Convert.ToDouble(
                                                        (skuListitem.Confirm_qty + skuListitem.Confirm_Free_qty) /
                                                        skuListitem.Pack_size);
                        civm.DeliveryGrandTotal = civm.GrandTotal +
                                                  Convert.ToDouble((skuListitem.Confirm_qty) * skuListitem.price);
                    }
                }

                var myList = Db.tblt_Order.Where(x => x.Challan_no == tbltChallan.id).Select(x => x.Orderid).ToList();


                civm.Momonumber = string.Join(",", myList.ToArray());

                civm.Challanline = challanlineVmList.ToList();

                return View(civm);
            }
            return PartialView("EmptyOrder");
        }
        public ActionResult PrintChallan(int id)
        {


            var tbltChallan = Db.tblt_Challan.Find(id);

            if (tbltChallan != null)
            {
                ChallaniVm civm = new ChallaniVm
                {
                    Id = id,
                    OrderDate = tbltChallan.order_date,
                    PsrId = tbltChallan.psr_id,
                    PsrName = tbltChallan.psr_Name,
                    RouteId = tbltChallan.route_id,
                    RouteName = tbltChallan.route_Name,
                    NoOfMemo = tbltChallan.No_of_memo,
                    ChallanStatus = tbltChallan.challan_status

                };


                List<ChallanlineVm> challanlineVmList = new List<ChallanlineVm>();

                var skuList = from a in Db.tblt_Challan_line
                              join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                              where a.challan_id == id
                              select a;

                if (tbltChallan.challan_status != 2)
                {
                    foreach (var skuListitem in skuList)
                    {
                        ChallanlineVm challanlineVm = new ChallanlineVm
                        {
                            SkuId = skuListitem.sku_id,
                            SkuName = Db.tbld_SKU.Where(x => x.SKU_id == skuListitem.sku_id).Select(x => x.SKUName).SingleOrDefault(),
                            BatchId = skuListitem.batch_id,
                            Price = skuListitem.price,
                            PackSize = skuListitem.Pack_size,

                            FreeCsQty = skuListitem.Free_qty / skuListitem.Pack_size,
                            FreePsQty = skuListitem.Free_qty % skuListitem.Pack_size,
                            FreeQty = skuListitem.Free_qty,

                            OrderCsQty = skuListitem.Order_qty / skuListitem.Pack_size,
                            OrderPsQty = skuListitem.Order_qty % skuListitem.Pack_size,
                            OrderQty = skuListitem.Total_qty,
                            ExtraQty = skuListitem.Extra_qty,

                            TotalCsQty = (skuListitem.Total_qty - skuListitem.Free_qty) / skuListitem.Pack_size,
                            TotalPsQty = (skuListitem.Total_qty - skuListitem.Free_qty) % skuListitem.Pack_size,
                            TotalQty = skuListitem.Total_qty - skuListitem.Free_qty,
                            OrderQtyPrice = skuListitem.Order_qty_price,

                            ReturnQty = skuListitem.Total_qty - skuListitem.Free_qty,

                        };

                        challanlineVmList.Add(challanlineVm);
                        civm.GrandTotalCs = civm.GrandTotalCs + Convert.ToDouble((skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) / skuListitem.Pack_size);
                        civm.GrandTotal = civm.GrandTotal + Convert.ToDouble((skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) * skuListitem.price);
                        civm.DeliveryGrandTotalCs = 0;
                        civm.DeliveryGrandTotal = 0;
                    }
                }
                else
                {

                    foreach (var skuListitem in skuList)
                    {

                        ChallanlineVm challanlineVm = new ChallanlineVm
                        {
                            SkuId = skuListitem.sku_id,
                            SkuName = Db.tbld_SKU.Where(x => x.SKU_id == skuListitem.sku_id).Select(x => x.SKUName)
                                .SingleOrDefault(),
                            BatchId = skuListitem.batch_id,
                            Price = skuListitem.price,
                            PackSize = skuListitem.Pack_size,

                            FreeCsQty = skuListitem.Confirm_Free_qty / skuListitem.Pack_size,
                            FreePsQty = skuListitem.Confirm_Free_qty % skuListitem.Pack_size,
                            FreeQty = skuListitem.Confirm_Free_qty,

                            OrderCsQty = skuListitem.Order_qty / skuListitem.Pack_size,
                            OrderPsQty = skuListitem.Order_qty % skuListitem.Pack_size,
                            OrderQty = skuListitem.Total_qty,
                            ExtraQty = skuListitem.Extra_qty,

                            TotalCsQty = skuListitem.Order_qty / skuListitem.Pack_size,
                            TotalPsQty = skuListitem.Order_qty % skuListitem.Pack_size,
                            TotalQty = skuListitem.Order_qty,
                            OrderQtyPrice = skuListitem.Order_qty_price,

                            ConfirmQty = skuListitem.Confirm_qty,
                            ConfirmCsQty = skuListitem.Confirm_qty / skuListitem.Pack_size,
                            ConfirmPsQty = skuListitem.Confirm_qty % skuListitem.Pack_size,
                            ConfirmQtyPrice = skuListitem.Confirm_qty_price,
                            ReturnQty = skuListitem.Total_qty - (skuListitem.Confirm_qty + skuListitem.Confirm_Free_qty)

                        };

                        challanlineVmList.Add(challanlineVm);
                        civm.GrandTotalCs = civm.GrandTotalCs +
                                            Convert.ToDouble(
                                                (skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) /
                                                skuListitem.Pack_size);
                        civm.GrandTotal = civm.GrandTotal +
                                          Convert.ToDouble(
                                              (skuListitem.Order_qty + skuListitem.Free_qty + skuListitem.Extra_qty) *
                                              skuListitem.price);
                        civm.DeliveryGrandTotalCs = civm.GrandTotalCs +
                                                    Convert.ToDouble(
                                                        (skuListitem.Confirm_qty + skuListitem.Confirm_Free_qty) /
                                                        skuListitem.Pack_size);
                        civm.DeliveryGrandTotal = civm.GrandTotal +
                                                  Convert.ToDouble((skuListitem.Confirm_qty) * skuListitem.price);
                    }
                }

                var myList = Db.tblt_Order.Where(x => x.Challan_no == tbltChallan.id).Select(x => x.Orderid).ToList();


                civm.Momonumber = string.Join(",", myList.ToArray());

                civm.Challanline = challanlineVmList.ToList();

                return PartialView(civm);
            }
            return PartialView("EmptyOrder");
        }
    }






}