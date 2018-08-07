using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class DBhouseRouteController : Controller
    {
        public  ODMSEntities Db = new ODMSEntities();

        // GET: DBhouseRoute
        public ActionResult Index()
        {

            return View();
        }

        //POST DBhouseRoute ditails data
        [HttpPost]
        public ActionResult ShowAllbydbid(int[] rsMid, int[] asMid, int[] cEid, int[] id)
        {
            
            Supporting sp = new Supporting();

            HashSet<int> dbids = sp.Alldbids(rsMid, asMid, cEid, id);


            var data = from a in Db.tbld_distributor_Route
                       join b in Db.status on a.IsActive equals b.status_Id
                       join c in Db.tbld_distributor_Route on a.ParentID equals c.RouteID into routeparent
                       from parent in routeparent.DefaultIfEmpty()
                       join d in Db.tbld_distribution_house on a.DistributorID equals d.DB_Id
                       join e in Db.tbld_distributor_RouteType on a.RouteType equals e.RouteType
                       where dbids.Contains(a.DistributorID)
                       orderby a.IsActive
                select new DBhouseRouteiVm
                       {
                           RouteId = a.RouteID,
                           RouteCode = a.RouteCode,
                           RouteName = a.RouteName,
                           RouteType = e.RouteTypeCode,
                           IsActive = b.status_code,
                           Distributor = d.DBName,
                           Parent = parent.RouteName
                       };

            return PartialView(data.ToList());

        }

        // GET: DBhouseRoute/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distributor_Route tbldDistributorRoute = Db.tbld_distributor_Route.Find(id);
            if (tbldDistributorRoute == null)
            {
                return HttpNotFound();
            }
            return View(tbldDistributorRoute);
        }

        // GET: DBhouseRoute/Create
        [CreateAccess]
        public ActionResult Create()
        {
            ViewBag.DBHouse = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
            ViewBag.Type = new SelectList(Db.tbld_distributor_RouteType.ToList(), "RouteType", "RouteTypeCode");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            ViewBag.Parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == 0 && x.RouteType == 1).ToList(), "RouteID", "RouteName");
            return View();
        }


        public ActionResult Routebydbid(int? id)
        {
            var distributorRoute = Db.tbld_distributor_Route.Where(x => x.DistributorID == id && x.RouteType == 1).ToList();
            return Json(distributorRoute, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DBhouseRouteVm dBhouseRouteVm)
        {
            if (ModelState.IsValid)
            {
                tbld_distributor_Route tbldDistributorRoute = new tbld_distributor_Route
                {
                    
                    RouteCode = dBhouseRouteVm.RouteCode,
                    RouteName = dBhouseRouteVm.RouteName,
                    RouteType = dBhouseRouteVm.RouteType,
                    CreateDate = dBhouseRouteVm.CreateDate,
                    ModifiedDate = dBhouseRouteVm.ModifiedDate,
                    IsActive = dBhouseRouteVm.IsActive,
                    DistributorID = dBhouseRouteVm.DistributorId,
                    ParentID = dBhouseRouteVm.ParentId
                };

                Db.tbld_distributor_Route.Add(tbldDistributorRoute);
                Db.SaveChanges();
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseRouteVm.RouteName + "  Create Successfully";
                return RedirectToAction("Create");
                
            }
            ViewBag.DBHouse = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
            ViewBag.Type = new SelectList(Db.tbld_distributor_RouteType.ToList(), "RouteType", "RouteTypeCode");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            ViewBag.Parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseRouteVm.DistributorId && x.RouteType == 1).ToList(), "RouteID", "RouteName", dBhouseRouteVm.ParentId);
            return View(dBhouseRouteVm);
        }

        // GET: DBhouseRoute/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distributor_Route tbldDistributorRoute = Db.tbld_distributor_Route.Find(id);
            if (tbldDistributorRoute == null)
            {
                return HttpNotFound();
            }

            DBhouseRouteVm dBhouseRouteVm = new DBhouseRouteVm
            {
                RouteId = tbldDistributorRoute.RouteID,
                RouteCode = tbldDistributorRoute.RouteCode,
                RouteName = tbldDistributorRoute.RouteName,
                RouteType = tbldDistributorRoute.RouteType,
                CreateDate = tbldDistributorRoute.CreateDate,
                ModifiedDate = tbldDistributorRoute.ModifiedDate??DateTime.Now,
                IsActive = tbldDistributorRoute.IsActive,
                DistributorId = tbldDistributorRoute.DistributorID,
                ParentId = tbldDistributorRoute.ParentID
            };

            ViewBag.DBHouse = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
            ViewBag.Type = new SelectList(Db.tbld_distributor_RouteType.ToList(), "RouteType", "RouteTypeCode");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            ViewBag.Parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseRouteVm.DistributorId && x.RouteType == 1).ToList(), "RouteID", "RouteName", dBhouseRouteVm.ParentId);
            return View(dBhouseRouteVm);
        }

        // POST: DBhouseRoute/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DBhouseRouteVm dBhouseRouteVm)
        {
            if (ModelState.IsValid)
            {
                tbld_distributor_Route tbldDistributorRoute = new tbld_distributor_Route
                {
                    RouteID = dBhouseRouteVm.RouteId,
                    RouteCode = dBhouseRouteVm.RouteCode,
                    RouteName = dBhouseRouteVm.RouteName,
                    RouteType = dBhouseRouteVm.RouteType,
                    CreateDate = dBhouseRouteVm.CreateDate,
                    ModifiedDate = DateTime.Now,
                    IsActive = dBhouseRouteVm.IsActive,
                    DistributorID = dBhouseRouteVm.DistributorId,
                    ParentID = dBhouseRouteVm.ParentId
                };

                Db.Entry(tbldDistributorRoute).State = EntityState.Modified;
                Db.SaveChanges();
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseRouteVm.RouteName + "  Update Successfully";
                return RedirectToAction("Index");
            }
            ViewBag.DBHouse = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
            ViewBag.Type = new SelectList(Db.tbld_distributor_RouteType.ToList(), "RouteType", "RouteTypeCode");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            ViewBag.Parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseRouteVm.DistributorId && x.RouteType == 1).ToList(), "RouteID", "RouteName", dBhouseRouteVm.ParentId);
            
            return View(dBhouseRouteVm);
        }

        // GET: DBhouseRoute/Delete/5
        [DeleteAccess]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distributor_Route tbldDistributorRoute = Db.tbld_distributor_Route.Find(id);
            if (tbldDistributorRoute == null)
            {
                return HttpNotFound();
            }
            return View(tbldDistributorRoute);
        }

        // POST: DBhouseRoute/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_distributor_Route tbldDistributorRoute = Db.tbld_distributor_Route.Find(id);
            if (tbldDistributorRoute != null) Db.tbld_distributor_Route.Remove(tbldDistributorRoute);
            Db.SaveChanges();
            TempData["alertbox"] = "error";
            if (tbldDistributorRoute != null)
                TempData["alertboxMsg"] = tbldDistributorRoute.RouteName + "Delete Successfully";
            return RedirectToAction("Index");
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
