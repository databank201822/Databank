using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class OrderController : Controller
    {

        public ODMSEntities Db = new ODMSEntities();

        TradePromotionSupporting tps = new TradePromotionSupporting();

        // GET: Order
        public ActionResult Index()
        {
            int dbid = (int)Session["DBId"];
            ViewBag.PSRList =
                new SelectList(
                    Db.tbld_distribution_employee.Where(x => x.DistributionId == dbid && x.Emp_Type == 2).ToList(),
                    "id", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult ShowAllorder(int orderStatus, int psr, DateTime dateTo, DateTime dateFrom)
        {
            List<int> status = new List<int>();

            if (orderStatus == 0)
            {
                status.Add(1);
                status.Add(2);
                status.Add(3);
                status.Add(9);
            }
            else
            {
                status.Add(orderStatus);
            }


            DateTime? blankTime = null;
            //int dbid = (int)Session["DBId"];

            var data = from a in Db.tblt_Order
                       where status.Contains(a.so_status) && DateTime.Compare(a.planned_order_date, dateTo) <= 0 &&
                             DateTime.Compare(a.planned_order_date, dateFrom) >= 0 && a.psr_id == psr && a.isProcess == 1
                       select new OrderiVm
                       {
                           Orderid = a.Orderid,
                           SoId = a.so_id,

                           RouteName = Db.tbld_distributor_Route.Where(x => x.RouteID == a.route_id).Select(x => x.RouteName)
                               .FirstOrDefault(),
                           OutletName = Db.tbld_Outlet.Where(x => x.OutletId == a.outlet_id).Select(x => x.OutletName)
                               .FirstOrDefault(),
                           ChallanNo = a.Challan_no,
                           PlannedOrderDate = a.planned_order_date,
                           DeliveryDate = a.so_status == 3 ? a.delivery_date : blankTime,
                           PsrName = Db.tbld_distribution_employee.Where(x => x.id == a.psr_id).Select(x => x.Name)
                               .FirstOrDefault(),
                           SoStatus = a.so_status,
                           OrderCs = Db.tblt_Order_line.Where(x => x.Orderid == a.Orderid && x.sku_order_type_id == 1)
                               .Select(x => x.quantity_confirmed / x.Pack_size).Sum().ToString(),
                           DeliveryCs = a.so_status == 3
                               ? Db.tblt_Order_line.Where(x => x.Orderid == a.Orderid && x.sku_order_type_id == 1)
                                   .Select(x => x.quantity_delivered / x.Pack_size).Sum().ToString()
                               : "",
                           TotalOrder = a.total_confirmed.ToString(),
                           TotalDelivered = a.so_status == 3 ? a.total_delivered.ToString() : "",
                           IsProcess = a.isProcess
                       };

            return PartialView(data.ToList());
        }

        /* Order Create Part start */
        public ActionResult CreateOrder()
        {
            int dbid = (int)Session["DBId"];

            ViewBag.Dbhouseid = dbid;
            ViewBag.OrderType = new SelectList(Db.tblt_OrderType.Where(x => x.Status == 1), "OrderTypeId",
                "OrderTypeName", 1);
            return View();
        }

        [HttpGet]
        public ActionResult CreateOrderbytype(int? orderType)
        {
            int dbid = (int)Session["DBId"];

            ViewBag.PSrList =
                new SelectList(
                    Db.tbld_distribution_employee.Where(
                        x => x.active == 1 && x.DistributionId == dbid && x.Emp_Type == 2), "id", "Name");
            // ViewBag.SubRoute = new SelectList(Db.tbld_distributor_Route.Where(x => x.IsActive == 1 && x.DistributorID == dbid && x.RouteType == 2), "RouteID", "RouteName");
            return PartialView();
        }

        [HttpGet]
        public ActionResult Outletlistbysubroute(int? subRouteid)
        {
            int dbid = (int)Session["DBId"];

            DateTime dateTime = (DateTime)Session["SystemDate"];
            List<int> orderedOutlet = new List<int>(Db.tblt_Order
                .Where(x => x.planned_order_date == dateTime && x.db_id == dbid && x.route_id == subRouteid)
                .Select(x => x.outlet_id));
            var outletlistbysubroute = Db.tbld_Outlet
                .Where(x => x.IsActive == 1 && x.parentid == subRouteid && !orderedOutlet.Contains(x.OutletId))
                .Select(x => new { x.OutletId, x.OutletName }).ToList();

            return Json(outletlistbysubroute, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SubroutebyPsr(int? orderType, int? psrId)
        {
            object subroutebyPsr;
            //int dbid = (int)Session["DBId"];
            DateTime dateTime = (DateTime)Session["SystemDate"];
            string dayOfWeek = dateTime.DayOfWeek.ToString();
            if (orderType == 1)
            {



                subroutebyPsr = (from a in Db.tbld_Route_Plan_Mapping
                                 join b in Db.tbld_distributor_Route on a.route_id equals b.RouteID
                                 where a.db_emp_id == psrId && a.day == dayOfWeek
                                 select (new { b.RouteID, b.RouteName })).Distinct().ToList();
            }
            else
            {
                //int dbid = (int)Session["DBId"];

                List<int?> current = Db.tbld_Route_Plan_Detail
                    .Where(x => x.db_emp_id == psrId && x.planned_visit_date == dateTime).Select(x => x.route_id)
                    .ToList();

                subroutebyPsr = (from a in Db.tbld_Route_Plan_Mapping
                                 join b in Db.tbld_distributor_Route on a.route_id equals b.RouteID
                                 where a.db_emp_id == psrId && !current.Contains(a.route_id)
                                 select (new { b.RouteID, b.RouteName })).Distinct().ToList();
            }


            return Json(subroutebyPsr, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOrderPart()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddRow(int count, int[] bundelitem, int[] skuList)
        {
            DateTime systemDate = (DateTime)Session["SystemDate"];
            DateTime dtFrom = Convert.ToDateTime("0001-01-01");
            int dbid = (int)Session["DBId"];
            ViewBag.count = count;

            var skulist = from a in Db.tbld_distribution_house
                          join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                          join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                          where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1 && !bundelitem.Contains(b.id) &&
                                !skuList.Contains(b.sku_id) && (b.end_date == dtFrom || b.end_date >= systemDate)

                          select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });

            ViewBag.SKUList = new SelectList(skulist.ToList(), "bundleid", "SKUName");

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
                                      TP_price = a.outlet_lifting_price,
                                      batch = a.batch_id,
                                      lpsc = b.SKUlpc
                                  }).ToList();


            return Json(skuitemDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRegularSaleOrder(OrderCreateVm orderinserVm)
        {

            int dbid = (int)Session["DBId"];
            var date = (DateTime)Session["SystemDate"];

            orderinserVm.OrderDateTime = DateTime.Now;
            orderinserVm.PlannedOrderDate = date;
            orderinserVm.ShippingDate = date.AddDays(1);
            orderinserVm.DeliveryDate = date.AddDays(1);
            orderinserVm.DbId = dbid;

            int countOrder = Db.tblt_Order.Count(x => x.psr_id == orderinserVm.PsrId &&
                                                      x.planned_order_date == orderinserVm.PlannedOrderDate);
            int nextOrder = countOrder + 1;
            var soId = "INV-[" + date.Date.ToString("yyyy-MM-dd") + "]-" + orderinserVm.PsrId + "-" + nextOrder;
            tblt_Order tbltOrder = new tblt_Order()
            {

                route_id = orderinserVm.RouteId,
                outlet_id = orderinserVm.OutletId,
                planned_order_date = orderinserVm.PlannedOrderDate,
                order_date_time = orderinserVm.OrderDateTime,
                shipping_date = orderinserVm.ShippingDate,
                delivery_date = orderinserVm.DeliveryDate,
                db_id = orderinserVm.DbId,
                psr_id = orderinserVm.PsrId,
                total_order = orderinserVm.TotalOrder,
                total_confirmed = orderinserVm.TotalOrder,
                total_delivered = orderinserVm.TotalOrder,
                sales_order_type_id = orderinserVm.SalesOrderTypeId,
                manual_discount = orderinserVm.ManualDiscount,
                so_id = soId,
                local_so_id = "0",
                Challan_no = orderinserVm.ChallanNo,
                so_status = 1,
                isProcess = 1

            };

            Db.tblt_Order.Add(tbltOrder);
            Db.SaveChanges();

            int orderId = tbltOrder.Orderid;



            foreach (var item in orderinserVm.OrderLine)
            {
                tblt_Order_line tbltOrderLine = new tblt_Order_line()
                {
                    sku_id = item.SkuId,
                    Betch_id = item.BetchId,
                    Bundelitmeid = item.Bundelitemid,
                    Orderid = orderId,
                    sku_order_type_id = 1,
                    unit_sale_price = item.UnitSalePrice,
                    Pack_size = item.PackSize,
                    lpec = (item.QuantityOrdered / item.Lpec >= 1 ? 1 : 0),
                    promotion_id = item.PromotionId,
                    quantity_confirmed = item.QuantityOrdered,
                    quantity_delivered = item.QuantityOrdered,
                    quantity_ordered = item.QuantityOrdered,
                    total_billed_amount = item.TotalSalePrice - item.TotalDiscountAmount,
                    total_discount_amount = item.TotalDiscountAmount,
                    total_sale_price = item.TotalSalePrice,

                };
                Db.tblt_Order_line.Add(tbltOrderLine);

            }



            Db.SaveChanges();

            //Trade Promtation Impact

            DateTime orderDate = Convert.ToDateTime(date);

            HashSet<int> tpList = new HashSet<int>(from a in Db.tblt_TradePromotionDBhouseMapping
                                                   join b in Db.tblt_TradePromotion on a.promo_id equals b.id
                                                   where b.is_active == 1 && a.db_id == dbid &&
                                                         DateTime.Compare((DateTime)b.start_date, orderDate) <= 0 &&
                                                         DateTime.Compare((DateTime)b.end_date, orderDate) >= 0
                                                   select a.promo_id);

            tps.ImpactTradepromotaton(orderId, tpList);
            //Trade Promtation Impact

            return Json("Success", JsonRequestBehavior.AllowGet);


        }

        /* Order Create Part End */

        public ActionResult OrderDetailByid(int id)
        {
            OrderDetailsVm orderDetailsVm = new OrderDetailsVm { Orderid = id };


            var orderInfo = Db.tblt_Order.SingleOrDefault(x => x.Orderid == id);
            var orderLine = from a in Db.tblt_Order_line
                            join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                            where a.Orderid == id && a.sku_order_type_id == 1
                            select new OrderLineDetilsVm
                            {
                                SkuName = b.SKUName,
                                BetchId = a.Betch_id,
                                PackSize = a.Pack_size,
                                UnitSalePrice = a.unit_sale_price,
                                SkuOrderTypeId = a.sku_order_type_id,
                                PromotionId = a.promotion_id,
                                PromotionName = Db.tblt_TradePromotion.Where(x => x.id == a.promotion_id).Select(x => x.name)
                                                    .FirstOrDefault() ?? "",
                                QuantityOrdered = a.quantity_ordered,
                                QuantityConfirmed = a.quantity_confirmed,
                                QuantityDelivered = a.quantity_delivered,
                                TotalSalePrice = a.total_sale_price,
                                TotalDiscountAmount = a.total_discount_amount,
                                TotalBilledAmount = a.total_billed_amount
                            };

            var tpIds = Db.tblt_Order_line.Where(x => x.Orderid == id).Select(x => x.promotion_id).ToList();

            var tplist = Db.tblt_TradePromotion.Where(x => tpIds.Contains(x.id)).Select(x => x.name);
            var tpliststring = "";
            foreach (var item in tplist)
            {
                tpliststring += item + "; \n";
            }

            if (orderInfo != null)
            {

                orderDetailsVm.Orderid = id;
                orderDetailsVm.SoId = orderInfo.so_id;
                orderDetailsVm.SubRoute = Db.tbld_distributor_Route.Where(x => x.RouteID == orderInfo.route_id)
                    .Select(x => x.RouteName).SingleOrDefault();
                orderDetailsVm.OutletName = Db.tbld_Outlet.Where(x => x.OutletId == orderInfo.outlet_id)
                    .Select(x => x.OutletName).SingleOrDefault();
                orderDetailsVm.OutletAddress = Db.tbld_Outlet.Where(x => x.OutletId == orderInfo.outlet_id)
                    .Select(x => x.Address).SingleOrDefault();
                orderDetailsVm.PlannedOrderDate = orderInfo.planned_order_date;
                orderDetailsVm.OrderDateTime = orderInfo.order_date_time;
                orderDetailsVm.ShippingDate = orderInfo.shipping_date;
                orderDetailsVm.DeliveryDate = orderInfo.delivery_date;
                orderDetailsVm.DbId = orderInfo.db_id;
                orderDetailsVm.PsrName = Db.tbld_distribution_employee.Where(x => x.id == orderInfo.psr_id)
                    .Select(x => x.Name).SingleOrDefault();
                orderDetailsVm.SoStatus = orderInfo.so_status;
                orderDetailsVm.TotalOrder = orderInfo.total_order ?? 0;
                orderDetailsVm.TotalConfirmed = orderInfo.total_confirmed ?? 0;
                orderDetailsVm.TotalDelivered = orderInfo.total_delivered ?? 0;
                orderDetailsVm.ManualDiscount = orderInfo.manual_discount ?? 0;
                orderDetailsVm.OrderLine = orderLine.ToList();
                orderDetailsVm.Promotation = tpliststring;
            }


            return View(orderDetailsVm);
        }

        public ActionResult OrderEditByid(int id)
        {

            OrderDetailsVm orderDetailsVm = new OrderDetailsVm();

            var orderInfo = Db.tblt_Order.SingleOrDefault(x => x.Orderid == id);

            if (orderInfo != null)
            {


                var orderLine = from a in Db.tblt_Order_line
                                join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                                where a.Orderid == id && a.sku_order_type_id == 1
                                select new OrderLineDetilsVm
                                {
                                    Bundelitemid = a.Bundelitmeid,
                                    SkuId = a.sku_id,
                                    SkuName = b.SKUName,
                                    BetchId = a.Betch_id,
                                    Lpec = b.SKUlpc,
                                    PackSize = a.Pack_size,
                                    UnitSalePrice = a.unit_sale_price,
                                    SkuOrderTypeId = a.sku_order_type_id,
                                    PromotionId = a.promotion_id,
                                    PromotionName = Db.tblt_TradePromotion.Where(x => x.id == a.promotion_id).Select(x => x.name)
                                                        .FirstOrDefault() ?? "",
                                    QuantityConfirmed = a.quantity_delivered,
                                    TotalSalePrice = a.total_sale_price,
                                    TotalDiscountAmount = a.total_discount_amount,
                                    TotalBilledAmount = a.total_billed_amount
                                };





                orderDetailsVm.Orderid = id;
                orderDetailsVm.ChallanId = orderInfo.Challan_no;
                orderDetailsVm.SoId = orderInfo.so_id;
                orderDetailsVm.SubRoute = Db.tbld_distributor_Route.Where(x => x.RouteID == orderInfo.route_id)
                    .Select(x => x.RouteName).SingleOrDefault();
                orderDetailsVm.OutletName = Db.tbld_Outlet.Where(x => x.OutletId == orderInfo.outlet_id)
                    .Select(x => x.OutletName).SingleOrDefault();
                orderDetailsVm.OutletAddress = Db.tbld_Outlet.Where(x => x.OutletId == orderInfo.outlet_id)
                    .Select(x => x.Address).SingleOrDefault();
                orderDetailsVm.PlannedOrderDate = orderInfo.planned_order_date;
                orderDetailsVm.OrderDateTime = orderInfo.order_date_time;
                orderDetailsVm.ShippingDate = orderInfo.shipping_date;
                orderDetailsVm.DeliveryDate = orderInfo.delivery_date;
                orderDetailsVm.DbId = orderInfo.db_id;
                orderDetailsVm.PsrName = Db.tbld_distribution_employee.Where(x => x.id == orderInfo.psr_id)
                    .Select(x => x.Name).SingleOrDefault();
                orderDetailsVm.SoStatus = orderInfo.so_status;
                orderDetailsVm.TotalOrder = orderInfo.total_order ?? 0;
                orderDetailsVm.TotalConfirmed = orderInfo.total_confirmed ?? 0;
                orderDetailsVm.TotalDelivered = orderInfo.total_delivered ?? 0;
                orderDetailsVm.ManualDiscount = orderInfo.manual_discount ?? 0;
                orderDetailsVm.OrderLine = orderLine.ToList();

                int dbid = (int)Session["DBId"];

                var skulist = from a in Db.tbld_distribution_house
                              join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                              join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                              where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1
                              select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });


                if (orderInfo.Challan_no != 0)
                {
                    var challansku = Db.tblt_Challan_line.Where(x => x.challan_id == orderInfo.Challan_no).Select(x => x.sku_id)
                        .ToList();

                    skulist = from a in Db.tbld_distribution_house
                              join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                              join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                              where a.DB_Id == dbid && b.status == 1 && challansku.Contains(c.SKU_id) && c.SKUStatus == 1
                              select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });
                }

                ViewBag.SKUList = skulist.ToList();


                return View(orderDetailsVm);

            }
            return View(orderDetailsVm);
        }

        [HttpPost]
        public ActionResult AddRowOnEdit(int count, int[] bundelitem, int[] skuList, int challanId)
        {
            int dbid = (int)Session["DBId"];

            ViewBag.count = count;
            var skulist = from a in Db.tbld_distribution_house
                          join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                          join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                          where a.DB_Id == dbid && b.status == 1 && c.SKUStatus == 1 && !bundelitem.Contains(b.id) && !skuList.Contains(b.sku_id)
                          select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });


            if (challanId != 0)
            {
                var challansku = Db.tblt_Challan_line.Where(x => x.challan_id == challanId).Select(x => x.sku_id)
                    .ToList();

                skulist = from a in Db.tbld_distribution_house
                          join b in Db.tbld_bundle_price_details on a.PriceBuandle_id equals b.bundle_price_id
                          join c in Db.tbld_SKU on b.sku_id equals c.SKU_id
                          where a.DB_Id == dbid && b.status == 1 && challansku.Contains(c.SKU_id) && c.SKUStatus == 1 &&
                                !bundelitem.Contains(b.id) &&
                                !skuList.Contains(b.sku_id)
                          select (new { bundleid = b.id, SKUName = c.SKUName + "[" + b.batch_id + "]" });
            }

            ViewBag.SKUList = new SelectList(skulist.ToList(), "bundleid", "SKUName");

            return PartialView();
        }

        [HttpPost]
        public ActionResult OrderEditByidSave(OrderCreateVm orderinserVm)
        {

            var orderid = orderinserVm.Orderid;
            var soStatus = orderinserVm.SoStatus;

            if (soStatus == 1)
            {
                Db.tblt_Order.Where(x => x.Orderid == orderid).ToList().ForEach(x =>
                {
                    x.total_confirmed = orderinserVm.TotalOrder;
                    x.total_delivered = orderinserVm.TotalOrder;
                    x.manual_discount = orderinserVm.ManualDiscount;

                });

                Db.SaveChanges();

                List<int> orderskus = orderinserVm.OrderLine.Select(x => x.SkuId).ToList();

                Db.tblt_Order_line.Where(x => x.Orderid == orderid && !orderskus.Contains(x.sku_id) && x.sku_order_type_id == 1).ToList().ForEach(x =>
                {
                    x.lpec = 0;
                    x.quantity_confirmed = 0;
                    x.quantity_delivered = 0;
                    x.total_billed_amount = 0;
                    x.total_discount_amount = 0;
                    x.total_sale_price = 0;
                }

                    );
                Db.SaveChanges();

                foreach (var item in orderinserVm.OrderLine)
                {

                    var soline = Db.tblt_Order_line.Count(x => x.Orderid == orderid && x.sku_id == item.SkuId && x.sku_order_type_id == 1);

                    if (soline > 0)
                    {
                        Db.tblt_Order_line.Where(x => x.Orderid == orderid && x.sku_id == item.SkuId && x.sku_order_type_id == 1).ToList()
                            .ForEach(x =>
                            {
                                x.sku_id = item.SkuId;
                                x.Betch_id = item.BetchId;
                                x.Bundelitmeid = item.Bundelitemid;
                                x.sku_order_type_id = 1;
                                x.unit_sale_price = item.UnitSalePrice;
                                x.Pack_size = item.PackSize;
                                x.lpec = (item.QuantityOrdered / item.Lpec >= 1 ? 1 : 0);
                                x.promotion_id = item.PromotionId;
                                x.quantity_confirmed = item.QuantityConfirmed;
                                x.quantity_delivered = item.QuantityConfirmed;
                                x.total_billed_amount = item.TotalSalePrice - item.TotalDiscountAmount;
                                x.total_discount_amount = item.TotalDiscountAmount;
                                x.total_sale_price = item.TotalSalePrice;
                            });

                        Db.SaveChanges();

                    }
                    else
                    {
                        tblt_Order_line tbltOrderLine = new tblt_Order_line()
                        {
                            sku_id = item.SkuId,
                            Betch_id = item.BetchId,
                            Bundelitmeid = item.Bundelitemid,
                            Orderid = orderid,
                            sku_order_type_id = 1,
                            unit_sale_price = item.UnitSalePrice,
                            Pack_size = item.PackSize,
                            lpec = (item.QuantityConfirmed / item.Lpec >= 1 ? 1 : 0),
                            promotion_id = item.PromotionId,
                            quantity_confirmed = item.QuantityConfirmed,
                            quantity_delivered = item.QuantityConfirmed,
                            quantity_ordered = 0,
                            total_billed_amount = item.TotalSalePrice - item.TotalDiscountAmount,
                            total_discount_amount = item.TotalDiscountAmount,
                            total_sale_price = item.TotalSalePrice
                        };
                        Db.tblt_Order_line.Add(tbltOrderLine);
                        Db.SaveChanges();
                    }
                }
            }
            else if (soStatus == 2)
            {
                Db.tblt_Order.Where(x => x.Orderid == orderid).ToList().ForEach(x =>
                {
                    x.total_delivered = orderinserVm.TotalOrder;
                    x.manual_discount = orderinserVm.ManualDiscount;

                });

                
                List<int> orderskus = orderinserVm.OrderLine.Select(x => x.SkuId).ToList();

                Db.tblt_Order_line.Where(x => x.Orderid == orderid && !orderskus.Contains(x.sku_id) && x.sku_order_type_id == 1).ToList().ForEach(x =>
                    {
                        x.lpec = 0;
                        x.quantity_delivered = 0;
                        x.total_billed_amount = 0;
                        x.total_discount_amount = 0;
                        x.total_sale_price = 0;
                    }

                );
                Db.SaveChanges();


                foreach (var item in orderinserVm.OrderLine)
                {

                    var soline = Db.tblt_Order_line.Count(x => x.Orderid == orderid && x.sku_id == item.SkuId &&
                                                               x.Betch_id == item.BetchId && x.sku_order_type_id == 1);

                    if (soline > 0)
                    {
                        Db.tblt_Order_line.Where(x => x.Orderid == orderid && x.sku_id == item.SkuId && x.sku_order_type_id == 1).ToList()
                            .ForEach(x =>
                            {
                                x.sku_id = item.SkuId;
                                x.Betch_id = item.BetchId;
                                x.Bundelitmeid = item.Bundelitemid;
                                x.sku_order_type_id = 1;
                                x.unit_sale_price = item.UnitSalePrice;
                                x.Pack_size = item.PackSize;
                                x.lpec = (item.QuantityConfirmed / item.Lpec >= 1 ? 1 : 0);
                                x.promotion_id = item.PromotionId;
                                x.quantity_delivered = item.QuantityConfirmed;
                                x.total_billed_amount = (item.QuantityConfirmed * item.UnitSalePrice) - item.TotalDiscountAmount;
                                x.total_discount_amount = item.TotalDiscountAmount;
                                x.total_sale_price = item.QuantityConfirmed * item.UnitSalePrice;
                            });

                    }
                    else
                    {
                        tblt_Order_line tbltOrderLine = new tblt_Order_line()
                        {
                            sku_id = item.SkuId,
                            Betch_id = item.BetchId,
                            Bundelitmeid = item.Bundelitemid,
                            Orderid = orderid,
                            sku_order_type_id = 1,
                            unit_sale_price = item.UnitSalePrice,
                            Pack_size = item.PackSize,
                            lpec = (item.QuantityConfirmed / item.Lpec >= 1 ? 1 : 0),
                            promotion_id = item.PromotionId,
                            quantity_confirmed = 0,
                            quantity_delivered = item.QuantityConfirmed,
                            quantity_ordered = 0,
                            total_billed_amount = item.TotalSalePrice - item.TotalDiscountAmount,
                            total_discount_amount = item.TotalDiscountAmount,
                            total_sale_price = item.TotalSalePrice
                        };
                        Db.tblt_Order_line.Add(tbltOrderLine);
                       
                    }
                    Db.SaveChanges();
                }
            }


            //Trade Promtation Impact
            var date = Db.tblt_Order.Where(x => x.Orderid == orderid).Select(x => x.planned_order_date)
                .SingleOrDefault();
            int dbid = (int)Session["DBId"];
            DateTime orderDate = Convert.ToDateTime(date);

            HashSet<int> tpList = new HashSet<int>(from a in Db.tblt_TradePromotionDBhouseMapping
                                                   join b in Db.tblt_TradePromotion on a.promo_id equals b.id
                                                   where b.is_active == 1 && a.db_id == dbid &&
                                                         DateTime.Compare((DateTime)b.start_date, orderDate) <= 0 &&
                                                         DateTime.Compare((DateTime)b.end_date, orderDate) >= 0
                                                   select a.promo_id);

            tps.ImpactTradepromotaton(orderid, tpList);
            //Trade Promtation Impact




            return Json("Success", JsonRequestBehavior.AllowGet);


        }


        public ActionResult NextOrderid(int id)
        {


            var orderInfo = Db.tblt_Order.SingleOrDefault(x => x.Orderid == id);
            var neworderid = Db.tblt_Order
                .Where(x => x.psr_id == orderInfo.psr_id && x.Orderid > id &&
                            x.planned_order_date == orderInfo.planned_order_date && x.so_status == orderInfo.so_status)
                .Select(x => x.Orderid).FirstOrDefault();

            return Json(neworderid, JsonRequestBehavior.AllowGet);



        }


        [HttpPost]
        public ActionResult Invoice(string ids)
        {

            int dbid = (int)Session["DBId"];
            int numOfLineInInvoice = 8;
            List<InvoiceVm> invoiceList = new List<InvoiceVm>();

            var orderid = ids.Split(',').Select(Int32.Parse).ToList();

            var inv = Db.tblt_Order.Where(x => orderid.Contains(x.Orderid));
            foreach (var invitem in inv)
            {
                int id = invitem.Orderid;


                InvoiceVm invoiceVm = new InvoiceVm();

                var invoiceLineItemSdata = (from a in Db.tblt_Order_line
                                            join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                                            where a.Orderid == id
                                            orderby a.id ascending
                                            select new InvoiceLineDetilsVm
                                            {
                                                SkuCode = b.SKUcode,
                                                SkuId = a.sku_id,
                                                SkuName = b.SKUName,
                                                BetchId = a.Betch_id,
                                                PackSize = a.Pack_size

                                            }).Distinct().ToList();
                var numberofmemo = (double)invoiceLineItemSdata.Count() / (double)numOfLineInInvoice;
                if (numberofmemo <= 1)
                {
                    List<InvoiceLineDetilsVm> invoiceLineItem = new List<InvoiceLineDetilsVm>();
                    var outlet = Db.tbld_Outlet.SingleOrDefault(x => x.OutletId == invitem.outlet_id);
                    var dbHouse = Db.tbld_distribution_house.SingleOrDefault(x => x.DB_Id == invitem.db_id);
                    var psrInfo = Db.tbld_distribution_employee.SingleOrDefault(x => x.id == invitem.psr_id);

                    var totalDelivered = (int)(invitem.total_delivered ?? 0);
                    var salesOrderTypeId = invitem.sales_order_type_id ?? 0;
                    var manualDiscount = (int)(invitem.manual_discount ?? 0);

                    if (outlet != null)
                        if (dbHouse != null)
                            if (psrInfo != null)
                                invoiceVm = new InvoiceVm
                                {
                                    Orderid = id,
                                    SoId = invitem.so_id,
                                    RouteName = Db.tbld_distributor_Route
                                        .Where(x => x.RouteID == invitem.route_id)
                                        .Select(x => x.RouteName).SingleOrDefault(),
                                    OutletName = outlet.OutletName,
                                    OutletAddress = outlet.Address,
                                    ChallanNo = invitem.Challan_no,
                                    PlannedOrderDate = invitem.planned_order_date,
                                    DeliveryDate = invitem.delivery_date,
                                    DbName = dbHouse.DBName ?? "",
                                    DbAddress = dbHouse.OfficeAddress,
                                    DbMobile = dbHouse.OwnerMoble,
                                    PsrName = psrInfo.Name,
                                    PsrMobile = psrInfo.contact_no,
                                    TotalDelivered = totalDelivered,
                                    SalesOrderTypeId = salesOrderTypeId,
                                    ManualDiscount = manualDiscount
                                };



                    foreach (var lineItem in invoiceLineItemSdata)
                    {
                        int confirmQty = 0;
                        int freeQty = 0;

                        double unitSalePrice = (from a in Db.tbld_bundle_price_details
                                                join b in Db.tbld_distribution_house on a.bundle_price_id equals b.PriceBuandle_id
                                                where a.sku_id == lineItem.SkuId && a.batch_id == lineItem.BetchId && b.DB_Id == dbid
                                                select a.outlet_lifting_price).SingleOrDefault();


                        var orderQtysum = (from b in Db.tblt_Order_line
                                           where b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                 b.Betch_id == lineItem.BetchId && b.sku_order_type_id == 1
                                           select (int?)b.quantity_delivered).Sum();

                        var freeQtysum = (from b in Db.tblt_Order_line
                                          where b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                b.Betch_id == lineItem.BetchId && b.sku_order_type_id == 2
                                          select (int?)b.quantity_delivered).Sum();

                        if (orderQtysum != null)
                        {
                            confirmQty = (int)orderQtysum;
                        }

                        if (freeQtysum != null)
                        {
                            freeQty = (int)freeQtysum;
                        }
                        var totalSalePrice = Db.tblt_Order_line
                                                 .Where(b => b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                             b.Betch_id == lineItem.BetchId &&
                                                             b.sku_order_type_id == 1)
                                                 .Sum(x => (double?)(x.total_sale_price)) ?? 0;
                        var totalDiscountAmount = Db.tblt_Order_line
                                                      .Where(b => b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                                  b.Betch_id == lineItem.BetchId &&
                                                                  b.sku_order_type_id == 1)
                                                      .Sum(x => (double?)(x.total_discount_amount)) ?? 0;
                        var totalBilledAmount = Db.tblt_Order_line
                                                    .Where(b => b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                                b.Betch_id == lineItem.BetchId &&
                                                                b.sku_order_type_id == 1)
                                                    .Sum(x => (double?)(x.total_billed_amount)) ?? 0;

                        InvoiceLineDetilsVm invoiceLineDetilsVm = new InvoiceLineDetilsVm
                        {
                            SkuCode = lineItem.SkuCode,
                            SkuName = lineItem.SkuName,
                            BetchId = lineItem.BetchId,
                            PackSize = lineItem.PackSize,
                            UnitSalePrice = confirmQty == 0 ? 0 : unitSalePrice,
                            QuantityDeliveredCs = confirmQty / lineItem.PackSize,
                            QuantityDeliveredPs = confirmQty % lineItem.PackSize,
                            QuantityFree = freeQty,
                            TotalSalePrice = Math.Round(totalSalePrice),
                            TotalDiscountAmount = Math.Round(totalDiscountAmount),
                            TotalBilledAmount = Math.Round(totalBilledAmount)



                        };
                        invoiceLineItem.Add(invoiceLineDetilsVm);

                    }


                    invoiceVm.InvoiceLine = invoiceLineItem;
                    invoiceList.Add(invoiceVm);
                }
                else
                {
                    double numberofInvoice = (double)invoiceLineItemSdata.Count() / numOfLineInInvoice;

                    for (int j = 0; j <= numberofInvoice; j++)
                    {
                        List<InvoiceLineDetilsVm> invoiceLineItem = new List<InvoiceLineDetilsVm>();


                        var outlet = Db.tbld_Outlet.SingleOrDefault(x => x.OutletId == invitem.outlet_id);
                        var dbHouse =
                            Db.tbld_distribution_house.SingleOrDefault(x => x.DB_Id == invitem.db_id);
                        var psrInfo =
                            Db.tbld_distribution_employee.SingleOrDefault(x => x.id == invitem.psr_id);

                        var totalDelivered = (int)(invitem.total_delivered ?? 0);
                        var salesOrderTypeId = invitem.sales_order_type_id ?? 0;
                        var manualDiscount = (int)(invitem.manual_discount ?? 0);

                        if (outlet != null) { }
                        if (dbHouse != null) { }
                        if (psrInfo != null) { }
                        invoiceVm = new InvoiceVm
                        {
                            Orderid = id,
                            SoId = invitem.so_id,
                            RouteName = Db.tbld_distributor_Route
                                .Where(x => x.RouteID == invitem.route_id)
                                .Select(x => x.RouteName).SingleOrDefault(),
                            OutletName = outlet.OutletName,
                            OutletAddress = outlet.Address,
                            ChallanNo = invitem.Challan_no,
                            PlannedOrderDate = invitem.planned_order_date,
                            DeliveryDate = invitem.delivery_date,
                            DbName = dbHouse.DBName ?? "",
                            DbAddress = dbHouse.OfficeAddress,
                            DbMobile = dbHouse.OwnerMoble,
                            PsrName = psrInfo.Name,
                            PsrMobile = psrInfo.contact_no,
                            TotalDelivered = totalDelivered,
                            SalesOrderTypeId = salesOrderTypeId,
                            ManualDiscount = manualDiscount
                        };



                        foreach (var lineItem in invoiceLineItemSdata.Take(numOfLineInInvoice))
                        {




                            int confirmQty = 0;
                            int freeQty = 0;

                            double unitSalePrice = (from a in Db.tbld_bundle_price_details
                                                    join b in Db.tbld_distribution_house on a.bundle_price_id equals b
                                                        .PriceBuandle_id
                                                    where a.sku_id == lineItem.SkuId && a.batch_id == lineItem.BetchId &&
                                                          b.DB_Id == dbid
                                                    select a.outlet_lifting_price).SingleOrDefault();


                            var orderQtysum = (from b in Db.tblt_Order_line
                                               where b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                     b.Betch_id == lineItem.BetchId && b.sku_order_type_id == 1
                                               select (int?)b.quantity_delivered).Sum();

                            var freeQtysum = (from b in Db.tblt_Order_line
                                              where b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                    b.Betch_id == lineItem.BetchId && b.sku_order_type_id == 2
                                              select (int?)b.quantity_delivered).Sum();

                            if (orderQtysum != null)
                            {
                                confirmQty = (int)orderQtysum;
                            }

                            if (freeQtysum != null)
                            {
                                freeQty = (int)freeQtysum;
                            }
                            var totalSalePrice = Db.tblt_Order_line
                                                     .Where(b => b.Orderid == id &&
                                                                 b.sku_id == lineItem.SkuId &&
                                                                 b.Betch_id == lineItem.BetchId &&
                                                                 b.sku_order_type_id == 1)
                                                     .Sum(x => (double?)(x.total_sale_price)) ?? 0;
                            var totalDiscountAmount = Db.tblt_Order_line
                                                          .Where(b => b.Orderid == id &&
                                                                      b.sku_id == lineItem.SkuId &&
                                                                      b.Betch_id == lineItem.BetchId &&
                                                                      b.sku_order_type_id == 1)
                                                          .Sum(x => (double?)(x.total_discount_amount)) ?? 0;
                            var totalBilledAmount = Db.tblt_Order_line
                                                        .Where(b => b.Orderid == id &&
                                                                    b.sku_id == lineItem.SkuId &&
                                                                    b.Betch_id == lineItem.BetchId &&
                                                                    b.sku_order_type_id == 1)
                                                        .Sum(x => (double?)(x.total_billed_amount)) ?? 0;

                            InvoiceLineDetilsVm invoiceLineDetilsVm = new InvoiceLineDetilsVm
                            {
                                SkuCode = lineItem.SkuCode,
                                SkuName = lineItem.SkuName,
                                BetchId = lineItem.BetchId,
                                PackSize = lineItem.PackSize,
                                UnitSalePrice = confirmQty == 0 ? 0 : unitSalePrice,
                                QuantityDeliveredCs = confirmQty / lineItem.PackSize,
                                QuantityDeliveredPs = confirmQty % lineItem.PackSize,
                                QuantityFree = freeQty,
                                TotalSalePrice = totalSalePrice,
                                TotalDiscountAmount = totalDiscountAmount,
                                TotalBilledAmount = totalBilledAmount



                            };
                            invoiceLineItem.Add(invoiceLineDetilsVm);


                        }


                        invoiceVm.InvoiceLine = invoiceLineItem;

                        invoiceList.Add(invoiceVm);
                        if (invoiceLineItemSdata.Count() >= numOfLineInInvoice)
                        {
                            invoiceLineItemSdata.RemoveRange(0, numOfLineInInvoice);
                        }


                    }



                }
            }

            return PartialView(invoiceList);

        }




        [HttpPost]
        public ActionResult PosInvoice(string ids)
        {

            int dbid = (int)Session["DBId"];

            List<InvoiceVm> invoiceList = new List<InvoiceVm>();

            var orderid = ids.Split(',').Select(Int32.Parse).ToList();

            var inv = Db.tblt_Order.Where(x => orderid.Contains(x.Orderid));

            foreach (var invitem in inv)
            {
                int id = invitem.Orderid;


                InvoiceVm invoiceVm = new InvoiceVm();

                var invoiceLineItemSdata = (from a in Db.tblt_Order_line
                                            join b in Db.tbld_SKU on a.sku_id equals b.SKU_id
                                            where a.Orderid == id
                                            orderby a.id ascending
                                            select new InvoiceLineDetilsVm
                                            {
                                                SkuCode = b.SKUcode,
                                                SkuId = a.sku_id,
                                                SkuName = b.SKUName,
                                                BetchId = a.Betch_id,
                                                PackSize = a.Pack_size

                                            }).Distinct().ToList();


                List<InvoiceLineDetilsVm> invoiceLineItem = new List<InvoiceLineDetilsVm>();
                var outlet = Db.tbld_Outlet.SingleOrDefault(x => x.OutletId == invitem.outlet_id);
                var dbHouse = Db.tbld_distribution_house.SingleOrDefault(x => x.DB_Id == invitem.db_id);
                var psrInfo = Db.tbld_distribution_employee.SingleOrDefault(x => x.id == invitem.psr_id);

                var totalDelivered = (int)(invitem.total_delivered ?? 0);
                var salesOrderTypeId = invitem.sales_order_type_id ?? 0;
                var manualDiscount = (int)(invitem.manual_discount ?? 0);

                if (outlet != null)
                    if (dbHouse != null)
                        if (psrInfo != null)
                            invoiceVm = new InvoiceVm
                            {
                                Orderid = id,
                                SoId = invitem.so_id,
                                RouteName = Db.tbld_distributor_Route
                                    .Where(x => x.RouteID == invitem.route_id)
                                    .Select(x => x.RouteName).SingleOrDefault(),
                                OutletName = outlet.OutletName,
                                OutletAddress = outlet.Address,
                                ChallanNo = invitem.Challan_no,
                                PlannedOrderDate = invitem.planned_order_date,
                                DeliveryDate = invitem.delivery_date,
                                DbName = dbHouse.DBName ?? "",
                                DbAddress = dbHouse.OfficeAddress,
                                DbMobile = dbHouse.OwnerMoble,
                                PsrName = psrInfo.Name,
                                PsrMobile = psrInfo.contact_no,
                                TotalDelivered = totalDelivered,
                                SalesOrderTypeId = salesOrderTypeId,
                                ManualDiscount = manualDiscount
                            };



                foreach (var lineItem in invoiceLineItemSdata)
                {
                    int confirmQty = 0;
                    int freeQty = 0;

                    double unitSalePrice = (from a in Db.tbld_bundle_price_details
                                            join b in Db.tbld_distribution_house on a.bundle_price_id equals b.PriceBuandle_id
                                            where a.sku_id == lineItem.SkuId && a.batch_id == lineItem.BetchId && b.DB_Id == dbid
                                            select a.outlet_lifting_price).SingleOrDefault();


                    var orderQtysum = (from b in Db.tblt_Order_line
                                       where b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                             b.Betch_id == lineItem.BetchId && b.sku_order_type_id == 1
                                       select (int?)b.quantity_delivered).Sum();

                    var freeQtysum = (from b in Db.tblt_Order_line
                                      where b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                            b.Betch_id == lineItem.BetchId && b.sku_order_type_id == 2
                                      select (int?)b.quantity_delivered).Sum();

                    if (orderQtysum != null)
                    {
                        confirmQty = (int)orderQtysum;
                    }

                    if (freeQtysum != null)
                    {
                        freeQty = (int)freeQtysum;
                    }
                    var totalSalePrice = Db.tblt_Order_line
                                             .Where(b => b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                         b.Betch_id == lineItem.BetchId &&
                                                         b.sku_order_type_id == 1)
                                             .Sum(x => (double?)(x.total_sale_price)) ?? 0;
                    var totalDiscountAmount = Db.tblt_Order_line
                                                  .Where(b => b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                              b.Betch_id == lineItem.BetchId &&
                                                              b.sku_order_type_id == 1)
                                                  .Sum(x => (double?)(x.total_discount_amount)) ?? 0;
                    var totalBilledAmount = Db.tblt_Order_line
                                                .Where(b => b.Orderid == id && b.sku_id == lineItem.SkuId &&
                                                            b.Betch_id == lineItem.BetchId &&
                                                            b.sku_order_type_id == 1)
                                                .Sum(x => (double?)(x.total_billed_amount)) ?? 0;

                    InvoiceLineDetilsVm invoiceLineDetilsVm = new InvoiceLineDetilsVm
                    {
                        SkuCode = lineItem.SkuCode,
                        SkuName = lineItem.SkuName,
                        BetchId = lineItem.BetchId,
                        PackSize = lineItem.PackSize,
                        UnitSalePrice = confirmQty == 0 ? 0 : unitSalePrice,
                        QuantityDeliveredCs = confirmQty / lineItem.PackSize,
                        QuantityDeliveredPs = confirmQty % lineItem.PackSize,
                        QuantityFree = freeQty,
                        TotalSalePrice = Math.Round(totalSalePrice),
                        TotalDiscountAmount = Math.Round(totalDiscountAmount),
                        TotalBilledAmount = Math.Round(totalBilledAmount)



                    };
                    invoiceLineItem.Add(invoiceLineDetilsVm);

                }


                invoiceVm.InvoiceLine = invoiceLineItem;
                invoiceList.Add(invoiceVm);



            }

            return PartialView(invoiceList);

        }
    }
}