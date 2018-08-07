using System;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class DayEndController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

        // GET: DayEnd
        public ActionResult Index()
        {
            string dateTime = Convert.ToDateTime(Session["SystemDate"]).ToString("yyyy-MM-dd h:mm tt");

            DateTime currentDate = Convert.ToDateTime(dateTime);
            DateTime prviousDate = currentDate.AddDays(-1);

            int dbid = (int)Session["DBId"];

            int previousChallanNotConfirmed = Db.tblt_Challan.Count(x => x.db_id == dbid && DateTime.Compare(x.delivery_date, currentDate) == 0 && x.challan_status == 1);

            int currentChallanNotCreated = Db.tblt_Order.Count(x => x.db_id == dbid && DateTime.Compare(x.planned_order_date, currentDate) == 0 && x.Challan_no == 0);
            int newOrder = Db.tblt_Order.Count(x => x.db_id == dbid && DateTime.Compare(x.planned_order_date, currentDate) == 0 && x.so_status == 1);


            ViewBag.PreviousChallanNotConfirmed = previousChallanNotConfirmed == 0 ? "Done" : "Not Done";

            ViewBag.CurrentChallanNotCreated = currentChallanNotCreated == 0 ? "Done" : "Not Done";
            ViewBag.NewOrder = newOrder == 0 ? "No" : "Yes[" + newOrder + "]";
            ViewBag.DayEndPossible = 1;

            string dayOfWeek=currentDate.DayOfWeek.ToString();

            

            if (previousChallanNotConfirmed != 0 || currentChallanNotCreated != 0 || newOrder != 0)
            {
                ViewBag.DayEndPossible = 0;
                TempData["alertbox"] = "error";
                TempData["alertboxMsg"] = "Day End Not Possible";
            }

            return View();
        }


        [HttpPost]
        public ActionResult Index(int dbid, DateTime dayEnd)
        {
            var currentSystem = Db.tblt_System.SingleOrDefault(x => x.DBid == dbid && x.CurrentDate == dayEnd);

            if (currentSystem != null)
            {
               Db.DayEnd_Process(currentSystem.DBid, currentSystem.CurrentDate); //Reporting data Generate using Stored Procedure & insert DayEndLog

               Db.tblt_System.Where(x => x.DBid == dbid && x.CurrentDate == dayEnd).ToList().ForEach(x =>
                {
                    x.CurrentDate = x.CurrentDate.AddDays(1);
                    x.NextDate = x.NextDate.AddDays(1);
                    x.PreviousDate = x.PreviousDate.AddDays(1);
                    x.UpdateDate = DateTime.Now;

                }); //Update System Date

                Db.SaveChanges();

                TempData["DayEnd"] = "DayEnd";
                TempData["DayEndalertbox"] = "Day End";
                return RedirectToAction("Logout", "Login");

            }
            return View();
        }
    }
}