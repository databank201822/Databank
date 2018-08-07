using System;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class DynamicFilterController : Controller
    {
        private ODMSEntities _db = new ODMSEntities();
        // GET: DynamicFilter
       

        public ActionResult Zone_filter()
        {
            ViewBag.User_Id = Session["User_Id"];
            ViewBag.user_role_code = Session["user_role_code"];

            int userRoleId = Convert.ToInt32(Session["User_role_id"]);
            int bizZoneId = Convert.ToInt32(Session["biz_zone_id"]);
            int userBizRoleId = Convert.ToInt32(Session["User_biz_role_id"]);
            int dBid = Convert.ToInt32(Session["DBId"]);

            ViewBag.RSMZone = new SelectList(_db.tbld_business_zone.Where(x => false).ToList(), "id", "biz_zone_name");
            ViewBag.ASMZone = new SelectList(_db.tbld_business_zone.Where(x => false).ToList(), "id", "biz_zone_name");
            ViewBag.CEZone = new SelectList(_db.tbld_business_zone.Where(x => false).ToList(), "id", "biz_zone_name");
            ViewBag.DBhouse = new SelectList(_db.tbld_distribution_house.Where(x => false).ToList(), "DB_Id", "DBName");
            if (userRoleId < 7)
            {
                if (userBizRoleId == 1)
                {
                    ViewBag.RSMZone = new SelectList(_db.tbld_business_zone.Where(x => x.parent_biz_zone_id == bizZoneId).ToList(), "id", "biz_zone_name");

                }
                if (userBizRoleId == 2)
                {
                    ViewBag.ASMZone = new SelectList(_db.tbld_business_zone.Where(x => x.parent_biz_zone_id == bizZoneId).ToList(), "id", "biz_zone_name");

                }
                else if (userBizRoleId == 3)
                {
                    ViewBag.CEZone = new SelectList(_db.tbld_business_zone.Where(x => x.parent_biz_zone_id == bizZoneId).ToList(), "id", "biz_zone_name");

                }
                else if (userBizRoleId == 4)
                {
                    ViewBag.DBhouse = new SelectList(_db.tbld_distribution_house.Where(x => x.Zone_id == bizZoneId).ToList(), "DB_Id", "DBName");

                }
            }
            else if (userRoleId ==7)
            {
                ViewBag.DBhouse = new SelectList(_db.tbld_distribution_house.Where(x => x.DB_Id == dBid).ToList(), "DB_Id", "DBName");
            }
            return PartialView();

            //if (User_role_id < 7)
            //{
            //    if (User_biz_role_id == 1)
            //    {
            //        ViewBag.RSMZone = new SelectList(db.tbld_business_zone.Where(x => x.parent_biz_zone_id == biz_zone_id).ToList(), "id", "biz_zone_name");
            //        return PartialView();
            //    }
            //    if (User_biz_role_id == 2)
            //    {
            //        ViewBag.ASMZone = new SelectList(db.tbld_business_zone.Where(x => x.parent_biz_zone_id == biz_zone_id).ToList(), "id", "biz_zone_name");
            //        return PartialView("RSM_Zone_filter");
            //    }
            //    else if (User_biz_role_id == 3)
            //    {
            //        ViewBag.CEZone = new SelectList(db.tbld_business_zone.Where(x => x.parent_biz_zone_id == biz_zone_id).ToList(), "id", "biz_zone_name");
            //        return PartialView("ASM_Zone_filter");
            //    }
            //    else if (User_biz_role_id == 4)
            //    {
            //        ViewBag.DBhouse = new SelectList(db.tbld_distribution_house.Where(x => x.Zone_id == biz_zone_id).ToList(), "DB_Id", "DBName");
            //        return PartialView("CE_Zone_filter");
            //    }
            //}
            //ViewBag.DBhouse = new SelectList(db.tbld_distribution_house.Where(x => x.Zone_id == biz_zone_id).ToList(), "DB_Id", "DBName");
            //return PartialView("CE_Zone_filter");

        }

        [HttpPost]
        public ActionResult Getzoneidsbyparentid(int[] ids)
        {
            if (ids!=null)
            {
                var tbldBusinessZone = _db.tbld_business_zone
                    .Where(a => ids.Contains(a.parent_biz_zone_id) && a.biz_zone_category_id != 1)
                    .Select(a => new
                    {
                        a.id,
                        a.biz_zone_name
                    });
                return Json(tbldBusinessZone, JsonRequestBehavior.AllowGet); 
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Getdbidsbyzoneid(int[] ids)
        {
            var tbldDistributionHouse = _db.tbld_distribution_house.Where(a => ids.Contains(a.Zone_id) && a.Status == 1)
                .Select(a => new
                {
                    a.DB_Id,
                    DB_name = a.DBName
                });
            return Json(tbldDistributionHouse, JsonRequestBehavior.AllowGet);
        }

        



        public ActionResult Sku_filter()
        {
            ViewBag.skuList = new SelectList(_db.tbld_SKU.Where(x => x.SKUStatus == 1).ToList(), "SKU_id", "SKUName");
            return PartialView();
        }
        public ActionResult DateRange_filter()
        {
           
            return PartialView();
        }

       
    }
}