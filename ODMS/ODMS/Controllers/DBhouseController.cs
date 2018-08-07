using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SuperAccess]
    [SessionExpire]    
    public class DBhouseController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

        // GET: DBhouse
        public ActionResult Index()
        {
            var data = from a in Db.tbld_distribution_house
                       join b in Db.tbld_business_zone on a.Zone_id equals b.id into zoneHierarchy
                       from zone in zoneHierarchy.DefaultIfEmpty()
                       join c in Db.tbld_cluster on a.Cluster_id equals c.id into dbcluster
                       from cluster in dbcluster.DefaultIfEmpty()
                       join d in Db.status on a.Status equals d.status_Id
                       join e in Db.status on a.DeliveryModuleStatus equals e.status_Id
                       join f in Db.tbld_bundle_price on a.PriceBuandle_id equals f.Id
                       orderby a.Status,a.DB_Id ascending 
                select new DBhouseiMv
                       {
                           DbId = a.DB_Id,
                           DbName = a.DBName,
                           DbCode = a.DBCode,
                           OfficeAddress = a.OfficeAddress,
                           OwnerName = a.OwnerName,
                           CreateDate = a.CreateDate,
                           ModifiedDate = a.ModifiedDate,
                           ClusterId = cluster.name,
                           Zone = zone.biz_zone_name,
                           PriceBuandle = f.Name,
                           DeliveryModuleStatus = e.status_code,
                           Status = d.status_code

                       };

            return View(data.ToList());
        }

        // GET: DBhouse/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = from a in Db.tbld_distribution_house
                join b in Db.tbld_business_zone on a.Zone_id equals b.id into zoneHierarchy
                from zone in zoneHierarchy.DefaultIfEmpty()
                join c in Db.tbld_cluster on a.Cluster_id equals c.id into dbcluster
                from cluster in dbcluster.DefaultIfEmpty()
                join d in Db.status on a.Status equals d.status_Id
                join e in Db.status on a.DeliveryModuleStatus equals e.status_Id
                join f in Db.tbld_bundle_price on a.PriceBuandle_id equals f.Id
                where a.DB_Id==id
                select new DBhouseiMv
                {
                    DbId = a.DB_Id,
                    DbName = a.DBName,
                    DbCode = a.DBCode,
                    OfficeAddress = a.OfficeAddress,
                    OwnerName = a.OwnerName,
                    CreateDate = a.CreateDate,
                    ModifiedDate = a.ModifiedDate,
                    ClusterId = cluster.name,
                    Zone = zone.biz_zone_name,
                    PriceBuandle = f.Name,
                    DeliveryModuleStatus = e.status_code,
                    Status = d.status_code

                };

            if (data != null)
            {
                DBhouseiMv dBhouseiMv = new DBhouseiMv();
                foreach (var item in data)
                {
                    dBhouseiMv = new DBhouseiMv()
                    {
                        DbId = item.DbId,
                        DbName = item.DbName,
                        DbCode = item.DbCode,
                        OfficeAddress = item.OfficeAddress,
                        OwnerName = item.OwnerName,
                        CreateDate = item.CreateDate,
                        ModifiedDate = item.ModifiedDate,
                        ClusterId = item.ClusterId,
                        Zone = item.Zone,
                        PriceBuandle = item.PriceBuandle,
                        DeliveryModuleStatus = item.DeliveryModuleStatus,
                        Status = item.Status

                    };
                }

                return View(dBhouseiMv);
            }
            return RedirectToAction("Index");
        }

        // GET: DBhouse/Create
        public ActionResult Create()
        {

            ViewBag.Cluster = new SelectList(Db.tbld_cluster.ToList(), "id", "name");
            ViewBag.Zone = new SelectList(Db.tbld_business_zone.Where(x => x.biz_zone_category_id == 4).ToList(), "id", "biz_zone_name");
            ViewBag.PriceBuandle = new SelectList(Db.tbld_bundle_price.ToList(), "Id", "Name");
            ViewBag.isactive = new SelectList(Db.status.ToList(), "status_Id", "status_code");

            return View();
        }

        // POST: DBhouse/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DBhouseMv dBhouseMv)
        {
            if (ModelState.IsValid)
            {
                tbld_distribution_house tbldDistributionHouse = new tbld_distribution_house
                {
                    DB_Id = dBhouseMv.DbId,
                    DBName = dBhouseMv.DbName,
                    DBCode = dBhouseMv.DbCode,
                    DBDescription = dBhouseMv.DbDescription,
                    OfficeAddress = dBhouseMv.OfficeAddress,
                    WarehouseAddress = dBhouseMv.WarehouseAddress,
                    OwnerName = dBhouseMv.OwnerName,
                    OwnerMoble = dBhouseMv.OwnerMoble,
                    EmailAddress = dBhouseMv.EmailAddress,
                    CreateDate = dBhouseMv.CreateDate,
                    ModifiedDate = dBhouseMv.ModifiedDate,
                    Cluster_id = dBhouseMv.ClusterId,
                    Zone_id = dBhouseMv.ZoneId,
                    PriceBuandle_id = dBhouseMv.PriceBuandleId,
                    DeliveryModuleStatus = dBhouseMv.DeliveryModuleStatus,
                    Status = dBhouseMv.Status


                };

                Db.tbld_distribution_house.Add(tbldDistributionHouse);
                Db.SaveChanges();

                int dBid = tbldDistributionHouse.DB_Id;

                tblt_System tbltSystem = new tblt_System
                {
                    DBid = dBid,
                    CurrentDate = DateTime.Now.Date,
                    NextDate = DateTime.Now.Date.AddDays(1),
                    PreviousDate = DateTime.Now.Date.AddDays(-1),
                    UpdateDate = DateTime.Now
                };

                Db.tblt_System.Add(tbltSystem);
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseMv.DbName + "  Create Successfully";

                return RedirectToAction("Index");
            }

            ViewBag.Cluster = new SelectList(Db.tbld_cluster.ToList(), "id", "name");
            ViewBag.Zone = new SelectList(Db.tbld_business_zone.Where(x => x.biz_zone_category_id == 4).ToList(), "id", "biz_zone_name");
            ViewBag.PriceBuandle = new SelectList(Db.tbld_bundle_price.ToList(), "Id", "Name");
            ViewBag.isactive = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            return View(dBhouseMv);
        }

        // GET: DBhouse/Edit/5
       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distribution_house tbldDistributionHouse = Db.tbld_distribution_house.Find(id);

            if (tbldDistributionHouse != null)
            {
                DBhouseMv dBhouseMv = new DBhouseMv
                {
                    DbId = tbldDistributionHouse.DB_Id,
                    DbName = tbldDistributionHouse.DBName,
                    DbCode = tbldDistributionHouse.DBCode,
                    DbDescription = tbldDistributionHouse.DBDescription,
                    OfficeAddress = tbldDistributionHouse.OfficeAddress,
                    WarehouseAddress = tbldDistributionHouse.WarehouseAddress,
                    OwnerName = tbldDistributionHouse.OwnerName,
                    OwnerMoble = tbldDistributionHouse.OwnerMoble,
                    EmailAddress = tbldDistributionHouse.EmailAddress,
                    CreateDate = tbldDistributionHouse.CreateDate,
                    ModifiedDate = tbldDistributionHouse.ModifiedDate,
                    ClusterId = tbldDistributionHouse.Cluster_id,
                    ZoneId = tbldDistributionHouse.Zone_id,
                    PriceBuandleId = tbldDistributionHouse.PriceBuandle_id,
                    DeliveryModuleStatus = tbldDistributionHouse.DeliveryModuleStatus,
                    Status = tbldDistributionHouse.Status


                };
                dBhouseMv.ModifiedDate = DateTime.Now;
                ViewBag.Cluster = new SelectList(Db.tbld_cluster.ToList(), "id", "name");
                ViewBag.Zone = new SelectList(Db.tbld_business_zone.Where(x => x.biz_zone_category_id == 4).ToList(), "id", "biz_zone_name");
                ViewBag.PriceBuandle = new SelectList(Db.tbld_bundle_price.ToList(), "Id", "Name");
                ViewBag.isactive = new SelectList(Db.status.ToList(), "status_Id", "status_code");
                return View(dBhouseMv);
            }
            return RedirectToAction("Index");
        }

        // POST: DBhouse/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DBhouseMv dBhouseMv)
        {
            if (ModelState.IsValid)
            {
                tbld_distribution_house tbldDistributionHouse = new tbld_distribution_house
                {
                    DB_Id = dBhouseMv.DbId,
                    DBName = dBhouseMv.DbName,
                    DBCode = dBhouseMv.DbCode,
                    DBDescription = dBhouseMv.DbDescription,
                    OfficeAddress = dBhouseMv.OfficeAddress,
                    WarehouseAddress = dBhouseMv.WarehouseAddress,
                    OwnerName = dBhouseMv.OwnerName,
                    OwnerMoble = dBhouseMv.OwnerMoble,
                    EmailAddress = dBhouseMv.EmailAddress,
                    CreateDate = dBhouseMv.CreateDate,
                    ModifiedDate = dBhouseMv.ModifiedDate,
                    Cluster_id = dBhouseMv.ClusterId,
                    Zone_id = dBhouseMv.ZoneId,
                    PriceBuandle_id = dBhouseMv.PriceBuandleId,
                    DeliveryModuleStatus = dBhouseMv.DeliveryModuleStatus,
                    Status = dBhouseMv.Status


                };
                Db.Entry(tbldDistributionHouse).State = EntityState.Modified;
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseMv.DbName + "  Edit Successfully";

                return RedirectToAction("Index");
            }
            ViewBag.Cluster = new SelectList(Db.tbld_cluster.ToList(), "id", "name");
            ViewBag.Zone = new SelectList(Db.tbld_business_zone.Where(x => x.biz_zone_category_id == 4).ToList(), "id", "biz_zone_name");
            ViewBag.PriceBuandle = new SelectList(Db.tbld_bundle_price.ToList(), "Id", "Name");
            ViewBag.isactive = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            return View(dBhouseMv);
        }

        // GET: DBhouse/Delete/5
        [DeleteAccess]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distribution_house tbldDistributionHouse = Db.tbld_distribution_house.Find(id);
            if (tbldDistributionHouse == null)
            {
                return HttpNotFound();
            }
            return View(tbldDistributionHouse);
        }

        // POST: DBhouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_distribution_house tbldDistributionHouse = Db.tbld_distribution_house.Find(id);
            if (tbldDistributionHouse != null) Db.tbld_distribution_house.Remove(tbldDistributionHouse);
            Db.SaveChanges();
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
