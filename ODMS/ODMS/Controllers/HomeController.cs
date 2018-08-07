using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class HomeController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        public ActionResult Index()
        {
            int userRoleId = (int)Session["User_role_id"];
            DateTime systemDate = DateTime.Today;

            if (userRoleId == 7)
            {
                int dbid = (int)Session["DBId"];
                systemDate = (DateTime)Session["SystemDate"];


                ViewBag.DeliveryPainding = (int?)Db.tblt_Order.Count(x => x.so_status != 9 && x.so_status != 3 && x.db_id == dbid);

                ViewBag.OrderCS = (from a in Db.tblt_Order
                                   join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                   where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                            a.db_id == dbid
                                   select new { qty = (double?)b.quantity_confirmed / (double?)b.Pack_size }).Sum(x => x.qty) ?? 0;

                ViewBag.ScheduleCall = (double?)(from a in Db.tbld_Route_Plan_Detail
                                                 join b in Db.tbld_Outlet on a.route_id equals b.parentid
                                                 where b.IsActive == 1 && a.planned_visit_date == systemDate && a.dbid == dbid
                                                 select b.OutletId).Count();

                ViewBag.ProductiveMemo = (double?)Db.tblt_Order.Count(x => x.planned_order_date == systemDate && x.so_status != 9 && x.db_id == dbid);


                ViewBag.TLSD = (double?)(from a in Db.tblt_Order
                    join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                    where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 && b.lpec == 1 &&
                          a.db_id == dbid
                    select b.quantity_confirmed / b.Pack_size).Count();
                ViewBag.Achievement = (from a in Db.tblt_Order
                                       join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                       where a.planned_order_date.Month == systemDate.Month && b.sku_order_type_id == 1 && a.so_status == 3 && a.db_id == dbid
                                       select new { qty = (double?)b.quantity_delivered / (double?)b.Pack_size }).Sum(x => x.qty) ?? 0;

                int tgtid = Db.tbld_Target.Where(x => x.end_date.Month == systemDate.Month && x.end_date.Year == systemDate.Year).Select(x => x.id).SingleOrDefault();

                var tbldTargetDetails = Db.tbld_Target_Details.Where(x => x.db_id == dbid && x.target_id == tgtid).Select(x => x.total_Qty / x.Pack_size);

                ViewBag.Target = 0;

                if (tbldTargetDetails.Any())
                {
                    ViewBag.Target = Math.Round(tbldTargetDetails.Sum(), 2);
                }
               
                var months = Db.tbl_calendar.SingleOrDefault(x => x.Date == systemDate);
                var days = Db.tbl_calendar.Where(x => x.MonthNo == months.MonthNo && x.Year == months.Year && x.id <= months.id).Select(x => new { x.DayNo, x.Date }).ToArray();

                ViewBag.days = string.Join(",", days.Select(x => x.DayNo));

                List<double> order = new List<double>();
                List<double> delevery = new List<double>();

                foreach (var day in days)
                {
                    double cs = (from a in Db.tblt_Order
                                 join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                 where a.planned_order_date == day.Date && b.sku_order_type_id == 1 &&
                                       a.so_status == 3 &&
                                       a.db_id == dbid
                                 select new { qty = (double?)b.quantity_confirmed / (double?)b.Pack_size })
                                .Sum(x => x.qty) ?? 0;
                    order.Add(cs);
                }

                foreach (var day in days)
                {
                    double cs = (from a in Db.tblt_Order
                                 join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                 where a.planned_order_date == day.Date && b.sku_order_type_id == 1 &&
                                       a.so_status == 3 &&
                                       a.db_id == dbid
                                 select new { qty = (double?)b.quantity_delivered / (double?)b.Pack_size })
                                .Sum(x => x.qty) ?? 0;
                    delevery.Add(cs);
                }

                ViewBag.order = string.Join(",", order.ToArray());
                ViewBag.delevery = string.Join(",", delevery.ToArray());


                ViewBag.Title = "Deshboard";
                return View();
            }

            int bizZoneId = (int)Session["biz_zone_id"];
            int userBizRoleId = (int)Session["User_biz_role_id"];

            HomeVm homeVm = new HomeVm();

            int totalscheduleCall = 0;
            int totaltlsd = 0;
            double totalorderCs = 0;
            int totalproductiveMemo = 0;
            double totalmtdTarget = 0;
            if (userBizRoleId == 1) //National
            {
                List<HomekpiVm> kpi = new List<HomekpiVm>();

                var performardata = Db.tbld_db_zone_view.Where(x => x.Status == 1).Select(x => new
                {
                    rsmid = x.REGION_id,
                    asmid = x.AREA_id,
                    x.AREA_Name
                }).Distinct().ToList().OrderBy(x => x.rsmid);

              
                foreach (var item in performardata)
                {
                    List<int> dbList = Db.tbld_db_zone_view.Where(x => x.AREA_id == item.asmid && x.Status == 1).Select(x => x.DB_Id)
                        .ToList();


                    int scheduleCall = (from a in Db.tbld_Route_Plan_Detail
                                        join b in Db.tbld_Outlet on a.route_id equals b.parentid
                                        where b.IsActive == 1 && dbList.Contains(b.Distributorid) && a.planned_visit_date == DateTime.Today
                                        select b.OutletId).Count();

                    totalscheduleCall = totalscheduleCall + scheduleCall;

                    int productiveMemo = (from a in Db.tblt_Order
                                          where
                                              a.planned_order_date == systemDate && a.so_status != 9 &&
                                                  dbList.Contains(a.db_id)
                                          select a.Orderid
                    ).Count();

                    totalproductiveMemo = totalproductiveMemo + productiveMemo;

                    int tlsd = (from a in Db.tblt_Order
                                join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                      b.lpec == 1 &&
                                      dbList.Contains(a.db_id)
                                select b.quantity_confirmed / b.Pack_size).Count();
                    totaltlsd = totaltlsd + tlsd;


                    Double orderCs = (from a in Db.tblt_Order
                                      join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                      where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                            dbList.Contains(a.db_id)
                                      select new { qty = (double?)b.quantity_confirmed / (double?)b.Pack_size }).Sum(x => x.qty) ?? 0;
                    totalorderCs = totalorderCs + orderCs;

                    int tgtid = Db.tbld_Target.Where(x => x.end_date.Month == systemDate.Month && x.end_date.Year == systemDate.Year).Select(x => x.id).SingleOrDefault();

                    var tbldTargetDetails = Db.tbld_Target_Details.Where(x =>dbList.Contains(x.db_id) && x.target_id == tgtid).Select(x => x.total_Qty / x.Pack_size);
                    double mtdTarget = 0;
                    if (tbldTargetDetails.Any())
                    {
                        mtdTarget = Math.Round(tbldTargetDetails.Sum(), 2);
                    }
                    totalmtdTarget = totalmtdTarget + mtdTarget;

                    HomekpiVm kpirow = new HomekpiVm
                    {
                        Performar = item.AREA_Name,
                        ScheduleCall = scheduleCall,
                        ProductiveMemo = productiveMemo,
                        Tlsd = tlsd,
                        MtdTarget = mtdTarget,
                        Order = orderCs,



                    };
                    kpi.Add(kpirow);
                }

                HomekpiVm kpitotal = new HomekpiVm
                {
                    Performar = "Grand Total",
                    ScheduleCall = totalscheduleCall,
                    ProductiveMemo = totalproductiveMemo,
                    Tlsd = totaltlsd,
                    MtdTarget = totalmtdTarget,
                    Order = totalorderCs,
                };
                kpi.Add(kpitotal);
                List<int> psrids = Db.tbld_db_psr_zone_view.Where(x => x.Status == 1).Select(x => x.DB_Id).ToList();
                homeVm.NoOfDb = Db.tbld_db_zone_view.Count(x => x.Status == 1);
                homeVm.NoOfPSr = psrids.Count();
                homeVm.NoOfLogin = Db.tblm_UserLogin.Count(x => psrids.Contains(x.PSR_id) && x.Date == systemDate);
               homeVm.Kpi = kpi;

            }
            else if (userBizRoleId == 2) //RSM
            {
                List<HomekpiVm> kpi = new List<HomekpiVm>();

                var performardata = Db.tbld_db_zone_view.Where(x => x.Status == 1 && x.REGION_id == bizZoneId).Select(x => new
                {
                    rsmid = x.REGION_id,
                    asmid = x.AREA_id,
                    x.AREA_Name
                }).Distinct().ToList().OrderBy(x => x.rsmid);

              
                foreach (var item in performardata)
                {
                    List<int> dbList = Db.tbld_db_zone_view.Where(x => x.AREA_id == item.asmid && x.Status == 1).Select(x => x.DB_Id)
                        .ToList();


                    int scheduleCall = (from a in Db.tbld_Route_Plan_Detail
                                        join b in Db.tbld_Outlet on a.route_id equals b.parentid
                                        where b.IsActive == 1 && dbList.Contains(b.Distributorid) && a.planned_visit_date == DateTime.Today
                                        select b.OutletId).Count();

                    totalscheduleCall = totalscheduleCall + scheduleCall;

                    int productiveMemo = (from a in Db.tblt_Order
                                          where
                                          a.planned_order_date == systemDate && a.so_status != 9 &&
                                          dbList.Contains(a.db_id)
                                          select a.Orderid
                    ).Count();

                    totalproductiveMemo = totalproductiveMemo + productiveMemo;

                    int tlsd = (from a in Db.tblt_Order
                                join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                      b.lpec == 1 &&
                                      dbList.Contains(a.db_id)
                                select b.quantity_confirmed / b.Pack_size).Count();
                    totaltlsd = totaltlsd + tlsd;


                    Double orderCs = (from a in Db.tblt_Order
                                      join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                      where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                            dbList.Contains(a.db_id)
                                      select new { qty = (double?)b.quantity_confirmed / (double?)b.Pack_size }).Sum(x => x.qty) ?? 0;
                    totalorderCs = totalorderCs + orderCs;

                    int tgtid = Db.tbld_Target.Where(x => x.end_date.Month == systemDate.Month && x.end_date.Year == systemDate.Year).Select(x => x.id).SingleOrDefault();

                    var tbldTargetDetails = Db.tbld_Target_Details.Where(x => dbList.Contains(x.db_id) && x.target_id == tgtid).Select(x => x.total_Qty / x.Pack_size);
                    double mtdTarget = 0;
                    if (tbldTargetDetails.Any())
                    {
                        mtdTarget = Math.Round(tbldTargetDetails.Sum(), 2);
                    }
                    totalmtdTarget = totalmtdTarget + mtdTarget;

                    HomekpiVm kpirow = new HomekpiVm
                    {
                        Performar = item.AREA_Name,
                        ScheduleCall = scheduleCall,
                        ProductiveMemo = productiveMemo,
                        Tlsd = tlsd,
                        MtdTarget = mtdTarget,
                        Order = orderCs
                    };
                    kpi.Add(kpirow);
                   
                }

                HomekpiVm kpitotal = new HomekpiVm
                {
                    Performar = "Grand Total",
                    ScheduleCall = totalscheduleCall,
                    ProductiveMemo = totalproductiveMemo,
                    Tlsd = totaltlsd,
                    MtdTarget = totalmtdTarget,
                    Order = totalorderCs,
                };
                kpi.Add(kpitotal);

                List<int> psrids = Db.tbld_db_psr_zone_view.Where(x => x.Status == 1 && x.REGION_id == bizZoneId).Select(x => x.DB_Id).ToList();
                homeVm.NoOfDb = Db.tbld_db_zone_view.Count(x => x.Status == 1 && x.REGION_id == bizZoneId);
                homeVm.NoOfPSr = psrids.Count();
                homeVm.NoOfLogin = Db.tblm_UserLogin.Count(x => psrids.Contains(x.PSR_id) && x.Date == systemDate);
                homeVm.Kpi = kpi;
            }
            else if (userBizRoleId == 3) //ASM
            {
                List<HomekpiVm> kpi = new List<HomekpiVm>();

                var performardata = Db.tbld_db_zone_view.Where(x => x.Status == 1 && x.AREA_id == bizZoneId).Select(x => new
                {
                    rsmid = x.REGION_id,
                    asmid = x.AREA_id,
                    ceid = x.CEAREA_id,
                    x.CEAREA_Name
                }).Distinct().ToList().OrderBy(x => x.rsmid);

            
                foreach (var item in performardata)
                {
                    List<int> dbList = Db.tbld_db_zone_view.Where(x => x.CEAREA_id == item.ceid && x.Status == 1).Select(x => x.DB_Id)
                        .ToList();


                    int scheduleCall = (from a in Db.tbld_Route_Plan_Detail
                                        join b in Db.tbld_Outlet on a.route_id equals b.parentid
                                        where b.IsActive == 1 && dbList.Contains(b.Distributorid) && a.planned_visit_date == DateTime.Today
                                        select b.OutletId).Count();

                    totalscheduleCall = totalscheduleCall + scheduleCall;

                    int productiveMemo = (from a in Db.tblt_Order
                                          where
                                          a.planned_order_date == systemDate && a.so_status != 9 &&
                                          dbList.Contains(a.db_id)
                                          select a.Orderid
                    ).Count();

                    totalproductiveMemo = totalproductiveMemo + productiveMemo;

                    int tlsd = (from a in Db.tblt_Order
                                join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                      b.lpec == 1 &&
                                      dbList.Contains(a.db_id)
                                select b.quantity_confirmed / b.Pack_size).Count();
                    totaltlsd = totaltlsd + tlsd;


                    Double orderCs = (from a in Db.tblt_Order
                                      join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                      where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                            dbList.Contains(a.db_id)
                                      select new { qty = (double?)b.quantity_confirmed / (double?)b.Pack_size }).Sum(x => x.qty) ?? 0;
                    totalorderCs = totalorderCs + orderCs;

                    int tgtid = Db.tbld_Target.Where(x => x.end_date.Month == systemDate.Month && x.end_date.Year == systemDate.Year).Select(x => x.id).SingleOrDefault();

                    var tbldTargetDetails = Db.tbld_Target_Details.Where(x => dbList.Contains(x.db_id) && x.target_id == tgtid).Select(x => x.total_Qty / x.Pack_size);
                    double mtdTarget = 0;
                    if (tbldTargetDetails.Any())
                    {
                        mtdTarget = Math.Round(tbldTargetDetails.Sum(), 2);
                    }
                    totalmtdTarget = totalmtdTarget + mtdTarget;

                    HomekpiVm kpirow = new HomekpiVm
                    {
                        Performar = item.CEAREA_Name,
                        ScheduleCall = scheduleCall,
                        ProductiveMemo = productiveMemo,
                        Tlsd = tlsd,
                        MtdTarget = mtdTarget,
                        Order = orderCs,



                    };
                    kpi.Add(kpirow);
                }

                HomekpiVm kpitotal = new HomekpiVm
                {
                    Performar = "Grand Total",
                    ScheduleCall = totalscheduleCall,
                    ProductiveMemo = totalproductiveMemo,
                    Tlsd = totaltlsd,
                    MtdTarget = totalmtdTarget,
                    Order = totalorderCs,
                };
                List<int> psrids = Db.tbld_db_psr_zone_view.Where(x => x.Status == 1 && x.AREA_id == bizZoneId).Select(x => x.DB_Id).ToList();
                homeVm.NoOfDb = Db.tbld_db_zone_view.Count(x => x.Status == 1 && x.AREA_id == bizZoneId);
                homeVm.NoOfPSr = psrids.Count();
                homeVm.NoOfLogin = Db.tblm_UserLogin.Count(x => psrids.Contains(x.PSR_id) && x.Date == systemDate);
                kpi.Add(kpitotal);
                homeVm.Kpi = kpi;
            }
            else if (userBizRoleId == 4) //CE
            {
                List<HomekpiVm> kpi = new List<HomekpiVm>();

                var performardata = Db.tbld_db_zone_view.Where(x => x.Status == 1 && x.CEAREA_id == bizZoneId).Select(x => new
                {
                    ceid = x.CEAREA_id,
                    dbid = x.DB_Id,
                    x.DB_Name
                }).Distinct().ToList().OrderBy(x => x.ceid);

                
                foreach (var item in performardata)
                {
                    List<int> dbList = Db.tbld_db_zone_view.Where(x => x.DB_Id == item.dbid && x.Status == 1).Select(x => x.DB_Id)
                        .ToList();


                    int scheduleCall = (from a in Db.tbld_Route_Plan_Detail
                                        join b in Db.tbld_Outlet on a.route_id equals b.parentid
                                        where b.IsActive == 1 && dbList.Contains(b.Distributorid) && a.planned_visit_date == DateTime.Today
                                        select b.OutletId).Count();

                    totalscheduleCall = totalscheduleCall + scheduleCall;

                    int productiveMemo = (from a in Db.tblt_Order
                                          where
                                          a.planned_order_date == systemDate && a.so_status != 9 &&
                                          dbList.Contains(a.db_id)
                                          select a.Orderid
                    ).Count();

                    totalproductiveMemo = totalproductiveMemo + productiveMemo;

                    int tlsd = (from a in Db.tblt_Order
                                join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                      b.lpec == 1 &&
                                      dbList.Contains(a.db_id)
                                select b.quantity_confirmed / b.Pack_size).Count();
                    totaltlsd = totaltlsd + tlsd;


                    Double orderCs = (from a in Db.tblt_Order
                                      join b in Db.tblt_Order_line on a.Orderid equals b.Orderid
                                      where a.planned_order_date == systemDate && b.sku_order_type_id == 1 && a.so_status != 9 &&
                                            dbList.Contains(a.db_id)
                                      select new { qty = (double?)b.quantity_confirmed / (double?)b.Pack_size }).Sum(x => x.qty) ?? 0;
                    totalorderCs = totalorderCs + orderCs;
                    int tgtid = Db.tbld_Target.Where(x => x.end_date.Month == systemDate.Month && x.end_date.Year == systemDate.Year).Select(x => x.id).SingleOrDefault();
                    var tbldTargetDetails = Db.tbld_Target_Details.Where(x => dbList.Contains(x.db_id) && x.target_id == tgtid).Select(x => x.total_Qty / x.Pack_size);
                    double mtdTarget = 0;
                    if (tbldTargetDetails.Any())
                    {
                        mtdTarget = Math.Round(tbldTargetDetails.Sum(), 2);
                    }
                    totalmtdTarget = totalmtdTarget + mtdTarget;
                    HomekpiVm kpirow = new HomekpiVm
                    {
                        Performar = item.DB_Name,
                        ScheduleCall = scheduleCall,
                        ProductiveMemo = productiveMemo,
                        Tlsd = tlsd,
                        MtdTarget = mtdTarget,
                        Order = orderCs,



                    };
                    kpi.Add(kpirow);
                }

                HomekpiVm kpitotal = new HomekpiVm
                {
                    Performar = "Grand Total",
                    ScheduleCall = totalscheduleCall,
                    ProductiveMemo = totalproductiveMemo,
                    Tlsd = totaltlsd,
                    MtdTarget = totalmtdTarget,
                    Order = totalorderCs,
                };
                List<int> psrids = Db.tbld_db_psr_zone_view.Where(x => x.Status == 1 && x.CEAREA_id == bizZoneId).Select(x => x.DB_Id).ToList();
                homeVm.NoOfDb = Db.tbld_db_zone_view.Count(x => x.Status == 1 && x.CEAREA_id == bizZoneId);
                homeVm.NoOfPSr = psrids.Count();
                homeVm.NoOfLogin = Db.tblm_UserLogin.Count(x => psrids.Contains(x.PSR_id) && x.Date == systemDate);
                kpi.Add(kpitotal);
             
                homeVm.Kpi = kpi;

            }



            ViewBag.Title = "Deshboard";
            return View("Deshboard", homeVm);
        }


    }
}