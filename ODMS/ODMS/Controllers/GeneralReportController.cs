using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class GeneralReportController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        Supporting sp = new Supporting();


        // GET: GeneralReport
        public ActionResult CurrentRouteplan()
        {
            return View("CurrentRouteplan/CurrentRouteplan");
        }


        [HttpPost]
        public ActionResult CurrentRouteplanFilter(int[] rsMid, int[] asMid, int[] cEid, int[] id)
        {



            HashSet<int> dbids = sp.Alldbids(rsMid, asMid, cEid, id);
            var data = (from a in Db.tbld_Route_Plan_Mapping
                        join b in Db.tbld_db_zone_view on a.db_id equals b.DB_Id
                        join c in Db.tbld_distribution_employee on a.db_emp_id equals c.id
                        join d in Db.tbld_distributor_Route on a.route_id equals d.RouteID
                        where dbids.Contains(a.db_id)

                        select new CurrentRouteplanVm
                        {
                            Region = b.REGION_Name,
                            Area = b.AREA_Name,
                            CeArea = b.CEAREA_Name,
                            DbHouse = b.DB_Name,
                            PsrName = c.Name,
                            SubRouteCode = d.RouteCode,
                            SubRouteName = d.RouteName,
                            NumberOfOutlet = Db.tbld_Outlet.Count(x => x.parentid == d.RouteID),
                            Saturday = Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Saturday" && x.route_id == a.route_id) ==
                                       1
                                ? d.RouteName
                                : "X",
                            Sunday = Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Sunday" && x.route_id == a.route_id) == 1
                                ? d.RouteName
                                : "X",
                            Monday = Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Monday" && x.route_id == a.route_id) == 1
                                ? d.RouteName
                                : "X",
                            Tuesday = Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Tuesday" && x.route_id == a.route_id) == 1
                                ? d.RouteName
                                : "X",
                            Wednesday =
                                Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Wednesday" && x.route_id == a.route_id) == 1
                                    ? d.RouteName
                                    : "X",
                            Thursday = Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Thursday" && x.route_id == a.route_id) ==
                                       1
                                ? d.RouteName
                                : "X",
                            Friday = Db.tbld_Route_Plan_Mapping.Count(x => x.day == "Friday" && x.route_id == a.route_id) == 1
                                ? d.RouteName
                                : "X",


                        }
            ).Distinct();



            return PartialView("CurrentRouteplan/CurrentRouteplanFilter", data.ToList());

        }



        // GET: GeneralReport
        public ActionResult Visitplan()
        {
            return View("Visitplan/Visitplan");
        }


        [HttpPost]
        public ActionResult VisitplanFilter(int[] rsMid, int[] asMid, int[] cEid, int[] id, DateTime startDate, DateTime endDate)
        {

            HashSet<int> dbids = sp.Alldbids(rsMid, asMid, cEid, id);
            var data = from a in Db.tbld_Route_Plan_Detail
                       join b in Db.tbld_db_psr_zone_view on a.db_emp_id equals b.PSR_id
                       join d in Db.tbld_distributor_Route on a.route_id equals d.RouteID
                       where dbids.Contains(b.DB_Id) && a.planned_visit_date >= startDate && a.planned_visit_date <= endDate

                       select new CurrentVisitplanVm()
                       {
                           PsrId = b.PSR_id,
                           Date = (DateTime)a.planned_visit_date,
                           Region = b.REGION_Name,
                           Area = b.AREA_Name,
                           CeArea = b.CEAREA_Name,
                           DbHouse = b.DB_Name,
                           PsrName = b.Name,
                           SubRouteName = d.RouteName,
                           NumberOfOutlet = Db.tbld_Outlet.Count(x => x.parentid == d.RouteID && x.IsActive == 1),
                           NumberOforderedoutlet =
                               Db.tblm_visit_detail.Count(x => x.date == a.planned_visit_date && x.psr_id == b.PSR_id &&
                                                               x.route_id == d.RouteID && x.visit_type == 1),
                           NumberOfnotorderedoutlet =
                               Db.tblm_visit_detail.Count(x => x.date == a.planned_visit_date && x.psr_id == b.PSR_id &&
                                                               x.route_id == d.RouteID && x.visit_type == 2),

                       };




            return PartialView("Visitplan/VisitplanFilter", data.ToList());

        }


        // GET: GeneralReport
        [HttpGet]
        public string Visitplandetails(int? id, string date)
        {
            return " " + id + " ";
        }



        public ActionResult StockMovement()
        {
            return View("StockMovement/StockMovement");
        }


        [HttpPost]
        public ActionResult StockMovementFilter(int id, DateTime startDate, DateTime endDate)
        {


            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                Height = Unit.Pixel(600)

            };

            List<RPT_StockMovement_Result> stockMovement = Db.RPT_StockMovement(startDate, endDate, id).ToList();


            reportViewer.LocalReport.ReportPath = Server.MapPath(@"~\Reports\RPT_StockMovement.rdlc");
            ReportDataSource rdc = new ReportDataSource("StockMovement", stockMovement);
            reportViewer.LocalReport.DataSources.Add(rdc);
            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;



            return PartialView("StockMovement/StockMovementFilter");

        }

        public ActionResult OrderVsStock()
        {

            return View("OrderVsStock/OrderVsStock");
        }

        public ActionResult OrderVsStockFilter(int[] rsMid, int[] asMid, int[] cEid, int[] id, int[] skuList)
        {
            //HashSet<int> dbids = sp.Alldbids(rsMid, asMid, cEid, id);
            int dbid = id[0];
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                Height = Unit.Percentage(100)

            };
            List<RPT_OrderVsStock_Result> rptOrderVsStockResult = Db.RPT_OrderVsStock(dbid).ToList();

            // rptOrderVsStockResult = Db.RPT_OrderVsStock(1).Where(x => skuList.Contains(x.sku_id)).ToList();


            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) +
                                                  @"Reports\RPT_OrderVsStock.rdlc";

            ReportDataSource rdc = new ReportDataSource("ODDataSet", rptOrderVsStockResult);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;

            return PartialView("OrderVsStock/OrderVsStockFilter");
        }

        public ActionResult ChallanVsDelivery(int id)
        {

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                Height = Unit.Percentage(100)

            };

            List<RPT_ChallanVsDelivery_Result> rptOrderVsStockResult = Db.RPT_ChallanVsDelivery(id).ToList();


            rptOrderVsStockResult.RemoveAll(x => x.Delivery == 0 && x.FreeDelivery == 0);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) +
                                                  @"Reports\RPT_ChallanVsDelivery.rdlc";

            ReportDataSource rdc = new ReportDataSource("CDDataSet", rptOrderVsStockResult);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;
            reportViewer.SizeToReportContent = true;

            ViewBag.ReportViewer = reportViewer;



            return View("ChallanVsDelivery/ChallanVsDelivery");
        }




    }



}
