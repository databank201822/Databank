using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class OrderProcessController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        // GET: OrderProcess
        public ActionResult Index()
        {
            int dbid = (int)Session["DBId"];
            ViewBag.PSRList = new SelectList(Db.tbld_distribution_employee.Where(x => x.DistributionId == dbid && x.Emp_Type == 2).ToList(), "id", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult ShowAllProcessableOrder(DateTime date)
        {
            int dbid = (int)Session["DBId"];
            var data = (from a in Db.tbld_distribution_employee
                        where a.DistributionId == dbid && a.Emp_Type == 2
                        select new OrderProcessIvm
                        {
                            DBid = dbid,
                            Psrid = a.id,
                            PsrName = a.Name,
                            NoOfOutlet = Db.tblt_Order.Count(x => x.psr_id == a.id && x.isProcess == 0 && x.planned_order_date == date),
                            Date = date.Date
                        }).ToList();

            data.RemoveAll(x => x.NoOfOutlet == 0);

            return PartialView(data);
        }


        public ActionResult ProcessOrder(DateTime date, int psrid)
        {
            TradePromotionSupporting tps=new TradePromotionSupporting();

            int dbid = (int)Session["DBId"];
            DateTime orderDate = Convert.ToDateTime(date);

            var orderidList = Db.tblt_Order
                .Where(x => x.planned_order_date == date && x.db_id == dbid && x.psr_id == psrid && x.isProcess == 0)
                .Select(x => x.Orderid).ToList();


            HashSet<int> tpList = new HashSet<int>(from a in Db.tblt_TradePromotionDBhouseMapping 
                                                   join b in Db.tblt_TradePromotion on a.promo_id equals b.id 
                                                   where b.is_active==1 && a.db_id == dbid && DateTime.Compare((DateTime) b.start_date, orderDate) <= 0 && DateTime.Compare((DateTime) b.end_date, orderDate) >= 0 
                                                   select a.promo_id);

            foreach (var orderid in orderidList)
            {
                tps.ImpactTradepromotaton(orderid, tpList);
            }

            return RedirectToAction("Index", "OrderProcess");
        }


      
    }
}