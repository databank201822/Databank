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
    public class DBhouseOutletController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

        // GET: DBhouseOutlet
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ShowAllbydbid(int[] rsMid, int[] asMid, int[] cEid, int[] id)
        {
            Supporting sp = new Supporting();

            HashSet<int> dbids = sp.Alldbids(rsMid, asMid, cEid, id);

            var data = from a in Db.tbld_Outlet
                       join b in Db.tbld_distribution_house on a.Distributorid equals b.DB_Id into dbHouse
                       from dBhouse in dbHouse.DefaultIfEmpty()
                       join c in Db.tbld_distributor_Route on a.parentid equals c.RouteID into dbRoute
                       from route in dbRoute.DefaultIfEmpty()
                       join e in Db.tbld_Outlet_category on a.outlet_category_id equals e.id into dboutletCategory
                       from outletCategory in dboutletCategory.DefaultIfEmpty()
                       join f in Db.tbld_Outlet_grade on a.grading equals f.id into outletGrading
                       from grading in outletGrading.DefaultIfEmpty()
                       join g in Db.tbld_Outlet_channel on a.channel equals g.id into outletChannel
                       from channel in outletChannel.DefaultIfEmpty()
                       where dbids.Contains(a.Distributorid)
                       orderby a.OutletId, a.parentid
                select new DBhouseoutletiVm
                       {
                           OutletId = a.OutletId,
                           OutletCode = a.OutletCode,
                           OutletName = a.OutletName,
                           OutletNameB = a.OutletName_b,
                           Location = a.Location,
                           Address = a.Address,
                           GpsLocation = a.GpsLocation,
                           OwnerName = a.OwnerName,
                           ContactNo = a.ContactNo,
                           Distributor = dBhouse.DBName,
                           HaveVisicooler = a.HaveVisicooler == 1 ? "YES" : "NO",
                           Parentid = route.RouteName,
                           Category = outletCategory.outlet_category_name,
                           Grading = grading.name,
                           Channel = channel.name,
                           Latitude = a.Latitude,
                           Longitude = a.Longitude,
                           Picture = a.picture,
                           IsActive = a.IsActive == 1 ? "Active" : "Inactive",
                           Createdate = a.createdate
                       };


            return PartialView(data.ToList());

        }


        // GET: DBhouseOutlet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBhouseoutletiVm dBhouseoutletiVm = new DBhouseoutletiVm();
            var data = from a in Db.tbld_Outlet
                       join b in Db.tbld_distribution_house on a.Distributorid equals b.DB_Id into dbHouse
                       from dBhouse in dbHouse.DefaultIfEmpty()
                       join c in Db.tbld_distributor_Route on a.parentid equals c.RouteID into dbRoute
                       from route in dbRoute.DefaultIfEmpty()
                       join e in Db.tbld_Outlet_category on a.outlet_category_id equals e.id into dboutletCategory
                       from outletCategory in dboutletCategory.DefaultIfEmpty()
                       join f in Db.tbld_Outlet_grade on a.grading equals f.id into outletGrading
                       from grading in outletGrading.DefaultIfEmpty()
                       join g in Db.tbld_Outlet_channel on a.channel equals g.id into outletChannel
                       from channel in outletChannel.DefaultIfEmpty()
                       where a.OutletId == id
                       select new
                       {
                           a.OutletId,
                           a.OutletCode,
                           a.OutletName,
                           a.OutletName_b,
                           a.Location,
                           a.Address,
                           a.GpsLocation,
                           a.OwnerName,
                           a.ContactNo,
                           Distributor = dBhouse.DBName,
                           HaveVisicooler = a.HaveVisicooler == 1 ? "YES" : "NO",
                           parentid = route.RouteName,
                           Category = outletCategory.outlet_category_name,
                           grading = grading.name,
                           channel = channel.name,
                           a.Latitude,
                           a.Longitude,
                           a.picture,
                           IsActive = a.IsActive == 1 ? "Active" : "Inactive",
                           a.createdate
                       };

            foreach (var a in data)
            {
                dBhouseoutletiVm = new DBhouseoutletiVm
                {
                    OutletId = a.OutletId,
                    OutletCode = a.OutletCode,
                    OutletName = a.OutletName,
                    OutletNameB = a.OutletName_b,
                    Location = a.Location,
                    Address = a.Address,
                    GpsLocation = a.GpsLocation,
                    OwnerName = a.OwnerName,
                    ContactNo = a.ContactNo,
                    Distributor = a.Distributor,
                    HaveVisicooler = a.HaveVisicooler,
                    Parentid = a.parentid,
                    Category = a.Category,
                    Grading = a.grading,
                    Channel = a.channel,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Picture = a.picture,
                    IsActive = a.IsActive,
                    Createdate = a.createdate
                };
            }
            return View(dBhouseoutletiVm);
        }

        // GET: DBhouseOutlet/Create
        public ActionResult Create()
        {
           
           

            if (Session["DBId"] == null)
            {
              
                ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
                ViewBag.parent = new SelectList("", "RouteID", "RouteName");
            }
            else
            {
                int dbid = (int)Session["DBId"];
                ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1 && x.DB_Id==dbid), "DB_Id", "DBName");
                ViewBag.parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.IsActive == 1 && x.RouteType == 2&&x.DistributorID==dbid), "RouteID", "RouteName");
            }
            
            ViewBag.outlet_category = new SelectList(Db.tbld_Outlet_category, "id", "outlet_category_name");
            ViewBag.gradinglist = new SelectList(Db.tbld_Outlet_grade, "id", "name");
            ViewBag.channellist = new SelectList(Db.tbld_Outlet_channel, "id", "name");
            ViewBag.IsActivelist = new SelectList(Db.status, "status_Id", "status_code");
            return View();
        }

        // POST: DBhouseOutlet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DBhouseoutletVm dBhouseoutletVm)
        {
            dBhouseoutletVm.Createdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                tbld_Outlet tbldOutlet = new tbld_Outlet
                {
                    OutletId = dBhouseoutletVm.OutletId,
                    OutletCode = dBhouseoutletVm.OutletCode,
                    OutletName = dBhouseoutletVm.OutletName,
                    OutletName_b = dBhouseoutletVm.OutletNameB,
                    Location = dBhouseoutletVm.Location,
                    Address = dBhouseoutletVm.Address,
                    GpsLocation = dBhouseoutletVm.GpsLocation,
                    OwnerName = dBhouseoutletVm.OwnerName,
                    ContactNo = dBhouseoutletVm.ContactNo,
                    Distributorid = dBhouseoutletVm.Distributorid,
                    HaveVisicooler = dBhouseoutletVm.HaveVisicooler,
                    parentid = dBhouseoutletVm.Parentid,
                    outlet_category_id = dBhouseoutletVm.OutletCategoryId,
                    grading = dBhouseoutletVm.Grading,
                    channel = dBhouseoutletVm.Channel,
                    Latitude = dBhouseoutletVm.Latitude,
                    Longitude = dBhouseoutletVm.Longitude,
                    picture = dBhouseoutletVm.Picture,
                    IsActive = dBhouseoutletVm.IsActive,
                    createdate = dBhouseoutletVm.Createdate
                };
                Db.tbld_Outlet.Add(tbldOutlet);
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseoutletVm.OutletName + "  Create Successfully";

                return RedirectToAction("Index");
            }

            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
            ViewBag.parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseoutletVm.Distributorid && x.IsActive == 1 && x.RouteType == 2), "RouteID", "RouteName");
            ViewBag.outlet_category = new SelectList(Db.tbld_Outlet_category, "id", "outlet_category_name");
            ViewBag.gradinglist = new SelectList(Db.tbld_Outlet_grade, "id", "name");
            ViewBag.channellist = new SelectList(Db.tbld_Outlet_channel, "id", "name");
            ViewBag.IsActivelist = new SelectList(Db.status, "status_Id", "status_code");
            return View(dBhouseoutletVm);
        }



        public ActionResult GetSubroutebydbid(int? id)
        {
            var getSubroutebydbid = Db.tbld_distributor_Route.Where(x => x.IsActive == 1 && x.DistributorID == id && x.RouteType == 2).ToList();
            return Json(getSubroutebydbid, JsonRequestBehavior.AllowGet);

        }



        // GET: DBhouseOutlet/Edit/5
        [EditAccess]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet tbldOutlet = Db.tbld_Outlet.Find(id);
            if (tbldOutlet == null)
            {
                return HttpNotFound();
            }
            DBhouseoutletVm dBhouseoutletVm = new DBhouseoutletVm
            {
                OutletId = tbldOutlet.OutletId,
                OutletCode = tbldOutlet.OutletCode,
                OutletName = tbldOutlet.OutletName,
                OutletNameB = tbldOutlet.OutletName_b,
                Location = tbldOutlet.Location,
                Address = tbldOutlet.Address,
                GpsLocation = tbldOutlet.GpsLocation,
                OwnerName = tbldOutlet.OwnerName,
                ContactNo = tbldOutlet.ContactNo,
                Distributorid = tbldOutlet.Distributorid,
                HaveVisicooler = tbldOutlet.HaveVisicooler,
                Parentid = tbldOutlet.parentid,
                OutletCategoryId = tbldOutlet.outlet_category_id,
                Grading = tbldOutlet.grading,
                Channel = tbldOutlet.channel,
                Latitude = tbldOutlet.Latitude,
                Longitude = tbldOutlet.Longitude,
                Picture = tbldOutlet.picture,
                IsActive = tbldOutlet.IsActive,
                Createdate = tbldOutlet.createdate
            };



            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
            ViewBag.parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseoutletVm.Distributorid && x.IsActive == 1 && x.RouteType == 2), "RouteID", "RouteName");
            ViewBag.outlet_category = new SelectList(Db.tbld_Outlet_category, "id", "outlet_category_name");
            ViewBag.gradinglist = new SelectList(Db.tbld_Outlet_grade, "id", "name");
            ViewBag.channellist = new SelectList(Db.tbld_Outlet_channel, "id", "name");
            ViewBag.IsActivelist = new SelectList(Db.status, "status_Id", "status_code");

            return View(dBhouseoutletVm);
        }

        // POST: DBhouseOutlet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DBhouseoutletVm dBhouseoutletVm)
        {
            if (ModelState.IsValid)
            {
                tbld_Outlet tbldOutlet = new tbld_Outlet
                {
                    OutletId = dBhouseoutletVm.OutletId,
                    OutletCode = dBhouseoutletVm.OutletCode,
                    OutletName = dBhouseoutletVm.OutletName,
                    OutletName_b = dBhouseoutletVm.OutletNameB,
                    Location = dBhouseoutletVm.Location,
                    Address = dBhouseoutletVm.Address,
                    GpsLocation = dBhouseoutletVm.GpsLocation,
                    OwnerName = dBhouseoutletVm.OwnerName,
                    ContactNo = dBhouseoutletVm.ContactNo,
                    Distributorid = dBhouseoutletVm.Distributorid,
                    HaveVisicooler = dBhouseoutletVm.HaveVisicooler,
                    parentid = dBhouseoutletVm.Parentid,
                    outlet_category_id = dBhouseoutletVm.OutletCategoryId,
                    grading = dBhouseoutletVm.Grading,
                    channel = dBhouseoutletVm.Channel,
                    Latitude = dBhouseoutletVm.Latitude,
                    Longitude = dBhouseoutletVm.Longitude,
                    picture = dBhouseoutletVm.Picture,
                    IsActive = dBhouseoutletVm.IsActive,
                    createdate = dBhouseoutletVm.Createdate
                };
                Db.Entry(tbldOutlet).State = EntityState.Modified;
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseoutletVm.OutletName + "  Edit Successfully";

                return RedirectToAction("Index");
            }
            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
            ViewBag.parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseoutletVm.Distributorid && x.IsActive == 1), "RouteID", "RouteName");
            ViewBag.outlet_category = new SelectList(Db.tbld_Outlet_category, "id", "outlet_category_name");
            ViewBag.gradinglist = new SelectList(Db.tbld_Outlet_grade, "id", "name");
            ViewBag.channellist = new SelectList(Db.tbld_Outlet_channel, "id", "name");
            ViewBag.IsActivelist = new SelectList(Db.status, "status_Id", "status_code");
            return View(dBhouseoutletVm);
        }

        // GET: DBhouseOutlet/Delete/5
        [DeleteAccess]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet tbldOutlet = Db.tbld_Outlet.Find(id);
            if (tbldOutlet == null)
            {
                return HttpNotFound();
            }
            return View(tbldOutlet);
        }

        // POST: DBhouseOutlet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_Outlet tbldOutlet = Db.tbld_Outlet.Find(id);
            if (tbldOutlet != null) Db.tbld_Outlet.Remove(tbldOutlet);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: DBhouseOutlet
        public ActionResult ApproveIndex()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ApproveOutletShowAllbydbid(int[] rsMid, int[] asMid, int[] cEid, int[] id)
        {
            Supporting sp = new Supporting();

            HashSet<int> dbids = sp.Alldbids(rsMid, asMid, cEid, id);

            var data = from a in Db.tbld_Outlet_new
                join b in Db.tbld_distribution_house on a.Distributorid equals b.DB_Id into dbHouse
                from dBhouse in dbHouse.DefaultIfEmpty()
                join c in Db.tbld_distributor_Route on a.parentid equals c.RouteID into dbRoute
                from route in dbRoute.DefaultIfEmpty()
                join e in Db.tbld_Outlet_category on a.outlet_category_id equals e.id into dboutletCategory
                from outletCategory in dboutletCategory.DefaultIfEmpty()
                join f in Db.tbld_Outlet_grade on a.grading equals f.id into outletGrading
                from grading in outletGrading.DefaultIfEmpty()
                join g in Db.tbld_Outlet_channel on a.channel equals g.id into outletChannel
                from channel in outletChannel.DefaultIfEmpty()
                where dbids.Contains(a.Distributorid) && a.verify_status==0
                orderby a.Id, a.parentid
                select new DBhouseoutletiVm
                {
                    OutletId = a.Id,
                    OutletCode = a.OutletCode,
                    OutletName = a.OutletName,
                    OutletNameB = a.OutletName_b,
                    Location = a.Location,
                    Address = a.Address,
                    GpsLocation = a.GpsLocation,
                    OwnerName = a.OwnerName,
                    ContactNo = a.ContactNo,
                    Distributor = dBhouse.DBName,
                    HaveVisicooler = a.HaveVisicooler == 1 ? "YES" : "NO",
                    Parentid = route.RouteName,
                    Category = outletCategory.outlet_category_name,
                    Grading = grading.name,
                    Channel = channel.name,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Picture = a.picture,
                    IsActive = a.IsActive == 1 ? "Active" : "Inactive",
                    Createdate = a.createdate
                };


            return PartialView(data.ToList());

        }

    
        public ActionResult ApproveOutlet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_new tbldOutlet = Db.tbld_Outlet_new.Find(id);
            if (tbldOutlet == null)
            {
                return HttpNotFound();
            }

            DBhouseoutletVm dBhouseoutletVm = new DBhouseoutletVm
            {
                OutletId = tbldOutlet.Id,
                OutletCode = tbldOutlet.OutletCode,
                OutletName = tbldOutlet.OutletName,
                OutletNameB = tbldOutlet.OutletName_b,
                Location = tbldOutlet.Location,
                Address = tbldOutlet.Address,
                GpsLocation = tbldOutlet.GpsLocation,
                OwnerName = tbldOutlet.OwnerName,
                ContactNo = tbldOutlet.ContactNo,
                Distributorid = tbldOutlet.Distributorid,
                HaveVisicooler = tbldOutlet.HaveVisicooler,
                Parentid = tbldOutlet.parentid,
                OutletCategoryId = tbldOutlet.outlet_category_id,
                Grading = tbldOutlet.grading,
                Channel = tbldOutlet.channel,
                Latitude = tbldOutlet.Latitude,
                Longitude = tbldOutlet.Longitude,
                Picture = tbldOutlet.picture,
                IsActive = tbldOutlet.IsActive,
                Createdate = tbldOutlet.createdate
            };



            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1 && x.DB_Id == tbldOutlet.Distributorid), "DB_Id", "DBName");
            ViewBag.parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseoutletVm.Distributorid && x.IsActive == 1 && x.RouteType == 2), "RouteID", "RouteName");
            ViewBag.outlet_category = new SelectList(Db.tbld_Outlet_category, "id", "outlet_category_name");
            ViewBag.gradinglist = new SelectList(Db.tbld_Outlet_grade, "id", "name");
            ViewBag.channellist = new SelectList(Db.tbld_Outlet_channel, "id", "name");
            ViewBag.IsActivelist = new SelectList(Db.status, "status_Id", "status_code");

            return View(dBhouseoutletVm);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveOutlet(DBhouseoutletVm dBhouseoutletVm)
        {
            if (ModelState.IsValid)
            {
                tbld_Outlet tbldOutlet = new tbld_Outlet
                {
                    OutletId = dBhouseoutletVm.OutletId,
                    OutletCode = dBhouseoutletVm.OutletCode,
                    OutletName = dBhouseoutletVm.OutletName,
                    OutletName_b = dBhouseoutletVm.OutletNameB,
                    Location = dBhouseoutletVm.Location,
                    Address = dBhouseoutletVm.Address,
                    GpsLocation = dBhouseoutletVm.GpsLocation,
                    OwnerName = dBhouseoutletVm.OwnerName,
                    ContactNo = dBhouseoutletVm.ContactNo,
                    Distributorid = dBhouseoutletVm.Distributorid,
                    HaveVisicooler = dBhouseoutletVm.HaveVisicooler,
                    parentid = dBhouseoutletVm.Parentid,
                    outlet_category_id = dBhouseoutletVm.OutletCategoryId,
                    grading = dBhouseoutletVm.Grading,
                    channel = dBhouseoutletVm.Channel,
                    Latitude = dBhouseoutletVm.Latitude,
                    Longitude = dBhouseoutletVm.Longitude,
                    picture = dBhouseoutletVm.Picture,
                    IsActive = dBhouseoutletVm.IsActive,
                    createdate = DateTime.Now
                };
                Db.tbld_Outlet.Add(tbldOutlet);

                Db.tbld_Outlet_new.Where(x => x.Id == tbldOutlet.OutletId).ToList().ForEach(x =>
                {
                    x.verify_status = 1;
                    x.verifydate = DateTime.Now;
                    x.verify_by = (int) Session["User_Id"];
                }); 


                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseoutletVm.OutletName + "  Approve Successfully";

                return RedirectToAction("ApproveIndex");
            }
            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
            ViewBag.parent = new SelectList(Db.tbld_distributor_Route.Where(x => x.DistributorID == dBhouseoutletVm.Distributorid && x.IsActive == 1), "RouteID", "RouteName");
            ViewBag.outlet_category = new SelectList(Db.tbld_Outlet_category, "id", "outlet_category_name");
            ViewBag.gradinglist = new SelectList(Db.tbld_Outlet_grade, "id", "name");
            ViewBag.channellist = new SelectList(Db.tbld_Outlet_channel, "id", "name");
            ViewBag.IsActivelist = new SelectList(Db.status, "status_Id", "status_code");
            return View(dBhouseoutletVm);
        }


        public ActionResult ApproveCancleOutlet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           
            tbld_Outlet_new tbldOutlet = Db.tbld_Outlet_new.Find(id);
            if (tbldOutlet == null)
            {
                TempData["alertbox"] = "error";
                TempData["alertboxMsg"] = "Sorry Not Found";

                return RedirectToAction("ApproveIndex");
            }

            Db.tbld_Outlet_new.Where(x => x.Id == tbldOutlet.Id).ToList().ForEach(x =>
            {
                x.verify_status = 3;
                x.verifydate = DateTime.Now;
                x.verify_by = (int)Session["User_Id"];
            });


            Db.SaveChanges();

            TempData["alertbox"] = "success";
            TempData["alertboxMsg"] = tbldOutlet.OutletName + "  Approve Cancle Successfully";

            return RedirectToAction("ApproveIndex");
          
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
