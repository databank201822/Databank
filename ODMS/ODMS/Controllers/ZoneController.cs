using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire, SuperAccess]
    public class ZoneController : Controller
    {
        public  ODMSEntities Db = new ODMSEntities();
        // GET: Zone
        public ActionResult Index()
        {
            ViewBag.title = "Zone";

            var data = from a in Db.tbld_business_zone
                       join b in Db.tbld_business_zone_hierarchy on a.biz_zone_category_id equals b.id

                       join c in Db.tbld_business_zone on a.parent_biz_zone_id equals c.id into tbldzone
                       from businessZone in tbldzone.DefaultIfEmpty()
                       orderby a.parent_biz_zone_id,b.parent_category_id
                       select new businessZoneVMindex{
                            Id = a.id,
                            BizZoneName = a.biz_zone_name,
                            BizZoneCode = a.biz_zone_code,
                            BizZoneDescription = a.biz_zone_description,
                            BizZoneCategory = b.biz_zone_category_name,
                            ParentBizZone = businessZone.biz_zone_name
                        };


           
            return View(data.ToList());
        }


        // GET: Zone/Details/5
        public ActionResult Details(int? id)
        {
            BusinessZoneVM bZvm = new BusinessZoneVM();

            ViewBag.title = "Zone Edit ";
            ViewBag.ZoneCatagory = Db.tbld_business_zone_hierarchy.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var businessZone = Db.tbld_business_zone.FirstOrDefault(x => x.id == id);

            if (businessZone == null)
            {
                return HttpNotFound();
            }
            bZvm.Id = businessZone.id;
            bZvm.BizZoneName = businessZone.biz_zone_name;
            bZvm.BizZoneCode = businessZone.biz_zone_code;
            bZvm.BizZoneDescription = businessZone.biz_zone_description;
            bZvm.BizZoneCategoryId = businessZone.biz_zone_category_id;
            bZvm.ParentBizZoneId = businessZone.parent_biz_zone_id;

            ViewBag.ZoneParent = Db.tbld_business_zone.Where(x => x.biz_zone_category_id == businessZone.biz_zone_category_id - 1).ToList();
            return View(bZvm);
        }

        // GET: Zone/Create
        public ActionResult Create()
        {
            ViewBag.title = "Create Zone";
            ViewBag.Zonecatagory = new SelectList(Db.tbld_business_zone_hierarchy.ToList(), "Id", "biz_zone_category_name");
            return View();
        }

        // POST: Zone/Create
        [HttpPost]
        public ActionResult Create(BusinessZoneVM businessZone)
        {
            
            if (ModelState.IsValid)
            {
                tbld_business_zone bZvm = new tbld_business_zone
                {
                    id = businessZone.Id,
                    biz_zone_name = businessZone.BizZoneName,
                    biz_zone_code = businessZone.BizZoneCode,
                    biz_zone_description = businessZone.BizZoneDescription,
                    biz_zone_category_id = businessZone.BizZoneCategoryId,
                    parent_biz_zone_id = businessZone.ParentBizZoneId
                };




                Db.tbld_business_zone.Add(bZvm);
                Db.SaveChanges();

                
                
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = bZvm.biz_zone_name + "Zone Create Successfully";
                return RedirectToAction("Index");
            }

            businessZone.BizZoneCategoryId = 0;
            return View(businessZone);
        }


        public ActionResult GetPerentbycategoryId(int? id)
        {
            var tbldBusinessZone = Db.tbld_business_zone.Where(x => x.biz_zone_category_id == id - 1).ToList();
            return Json(tbldBusinessZone, JsonRequestBehavior.AllowGet);
        }



        // GET: Zone/Edit/5
        public ActionResult Edit(int? id)
        {
            BusinessZoneVM bZvm = new BusinessZoneVM();

            ViewBag.title = "Zone Edit ";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var businessZone = Db.tbld_business_zone.FirstOrDefault(x => x.id==id);

            if (businessZone == null)
            {
                return HttpNotFound();
            }
            bZvm.Id = businessZone.id;           
            bZvm.BizZoneName = businessZone.biz_zone_name;
            bZvm.BizZoneCode = businessZone.biz_zone_code;
            bZvm.BizZoneDescription = businessZone.biz_zone_description;
            bZvm.BizZoneCategoryId = businessZone.biz_zone_category_id;
            bZvm.ParentBizZoneId = businessZone.parent_biz_zone_id;

            ViewBag.ZoneCatagory = new SelectList(Db.tbld_business_zone_hierarchy.ToList(), "Id", "biz_zone_category_name");
            ViewBag.ZoneParent = new SelectList(Db.tbld_business_zone.Where(x => x.biz_zone_category_id == businessZone.biz_zone_category_id - 1).ToList(), "id", "biz_zone_name");
            return View(bZvm);
           
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessZoneVM businessZone)
        {
            if (businessZone.BizZoneCategoryId == 1)
            {
                businessZone.ParentBizZoneId = 0;
            }

            if (ModelState.IsValid)
            {
                tbld_business_zone bZvm = new tbld_business_zone();

                bZvm.id = businessZone.Id;
                bZvm.biz_zone_name = businessZone.BizZoneName;
                bZvm.biz_zone_code = businessZone.BizZoneCode;
                bZvm.biz_zone_description = businessZone.BizZoneDescription;
                bZvm.biz_zone_category_id = businessZone.BizZoneCategoryId;
                bZvm.parent_biz_zone_id = businessZone.ParentBizZoneId;
                Db.Entry(bZvm).State = EntityState.Modified;
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = bZvm.biz_zone_name + " Zone Edit Successfully";

                return RedirectToAction("Index");
            }
            return View(businessZone);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_business_zone tbldBusinessZone = Db.tbld_business_zone.Find(id);
            if (tbldBusinessZone == null)
            {
                return HttpNotFound();
            }
            ViewBag.ZoneCatagory = Db.tbld_business_zone_hierarchy.ToList();
            ViewBag.ZoneParent = Db.tbld_business_zone.Where(x => x.biz_zone_category_id == tbldBusinessZone.biz_zone_category_id - 1).ToList();
            return View(tbldBusinessZone);
        }

        // POST: user_role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_business_zone tbldBusinessZone = Db.tbld_business_zone.Find(id);
            if (tbldBusinessZone != null)
            {
                Db.tbld_business_zone.Remove(tbldBusinessZone);
                Db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = tbldBusinessZone.biz_zone_name + " Zone Delete Successfully";
            }

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
