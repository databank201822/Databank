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
    public class TradePromotionController : Controller
    {
        private ODMSEntities Db = new ODMSEntities();

        Supporting sp = new Supporting();

        // GET: TradePromotion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShowAllTradePromotion(DateTime dateTo, DateTime dateFrom)
        {
            var data = (from a in Db.tblt_TradePromotion
                where DateTime.Compare((DateTime) a.start_date, dateTo) <= 0 &&
                      DateTime.Compare((DateTime) a.start_date, dateFrom) >= 0
                select new TradePromotionVm
                {
                    Id = a.id,
                    Name = a.name,
                    Code = a.code,
                    Description = a.description,
                    StartDate = (DateTime) a.start_date,
                    EndDate = (DateTime) a.end_date,
                    IsActive = (int) a.is_active



                }
            ).ToList();

            return PartialView(data);
        }



        // GET: TradePromotion/Create
        public ActionResult Create()
        {
            int bizZoneId = Convert.ToInt32(Session["biz_zone_id"]);
            ViewBag.RSMZone =
                new SelectList(Db.tbld_business_zone.Where(x => x.parent_biz_zone_id == bizZoneId).ToList(), "id",
                    "biz_zone_name");
            ViewBag.Cluster = new SelectList(Db.tbld_cluster.ToList(), "id", "name");

            ViewBag.OfferType = new SelectList(Db.tblt_TradePromotionOfferType.ToList(), "id", "offertype");
            ViewBag.ConditionType = new SelectList(Db.tblt_TradePromotionConditionType.ToList(), "id", "ConditionType");

            ViewBag.PromotionType = new SelectList(Db.tblt_TradePromotionType.ToList(), "promo_type_id",
                "promo_type_name");

            var tPCode = Db.tblt_TradePromotion.Count(x => x.start_date == DateTime.Now)+1;
            ViewBag.Promotioncode = "TP-" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + tPCode;

            return View();
        }

        // POST: TradePromotion/SaveTradePromotation

        [HttpPost]

        public ActionResult SaveTradePromotation(TradePromotionCreateVm tradePromotionCreateVm)
        {

            tblt_TradePromotion tbltTradePromotion = new tblt_TradePromotion
            {
                name = sp.Remove_Special_Characters(tradePromotionCreateVm.Name),
                code = tradePromotionCreateVm.Code,
                description = sp.Remove_Special_Characters(tradePromotionCreateVm.Description),
                TP_type = 1,
                TPOffer_type = 1,
                promotion_unit_id = 1,
                promotion_sub_unit_id = 1,
                start_date = tradePromotionCreateVm.StartDate,
                end_date = tradePromotionCreateVm.EndDate,
                expiry_date = tradePromotionCreateVm.EndDate,
                create_date = DateTime.Now,
                is_active = 1

            };
            Db.tblt_TradePromotion.Add(tbltTradePromotion);
            Db.SaveChanges();

            int tpId = tbltTradePromotion.id;


            if (tradePromotionCreateVm.OfferType == 1)
            {
                foreach (var orderskulistitem in tradePromotionCreateVm.Orderskulist)
                {
                    tblt_TradePromotionDefinition tbltTradePromotionOrderDefinition = new tblt_TradePromotionDefinition
                    {
                        promo_id = tpId,
                        rule_type = tradePromotionCreateVm.RuleType,
                        promo_line_type = 1,
                        condition_type = tradePromotionCreateVm.ConditionType,
                        offer_type = tradePromotionCreateVm.OfferType,
                        condition_sku_id = orderskulistitem.SkuId,
                        condition_sku_Batch = orderskulistitem.BetchId,
                        condition_sku_pack_size = orderskulistitem.PackSize,
                        condition_sku_amount = orderskulistitem.Qty,
                        offer_sku_id = 0,
                        offer_sku_pack_size = 0,
                        offer_sku_Batch = 0,
                        offer_sku_amount = 0,
                        condition_bundle_qty_CS = orderskulistitem.SlabCsQty,
                        condition_sku_group = 1
                    };
                    Db.tblt_TradePromotionDefinition.Add(tbltTradePromotionOrderDefinition);

                }
                foreach (var freeskulistitem in tradePromotionCreateVm.Freeskulist)
                {
                    tblt_TradePromotionDefinition tbltTradePromotionfreeDefinition = new tblt_TradePromotionDefinition
                    {
                        promo_id = tpId,
                        rule_type = 1,
                        promo_line_type = 2,
                        condition_type = 1,
                        offer_type = tradePromotionCreateVm.OfferType,
                        condition_sku_id = 0,
                        condition_sku_Batch = 0,
                        condition_sku_pack_size = 0,
                        condition_sku_amount = 0,
                        offer_sku_id = freeskulistitem.SkuId,
                        offer_sku_pack_size = freeskulistitem.PackSize,
                        offer_sku_Batch = freeskulistitem.BetchId,
                        offer_sku_amount = freeskulistitem.Qty,
                        condition_bundle_qty_CS = freeskulistitem.SlabCsQty,
                        condition_sku_group = 1
                    };
                    Db.tblt_TradePromotionDefinition.Add(tbltTradePromotionfreeDefinition);

                }
            }
            else
            {
                foreach (var orderskulistitem in tradePromotionCreateVm.Orderskulist)
                {
                    tblt_TradePromotionDefinition tbltTradePromotionOrderDefinition = new tblt_TradePromotionDefinition
                    {
                        promo_id = tpId,
                        rule_type = 1,
                        promo_line_type = 3,
                        condition_type = 1,
                        offer_type = 2,
                        condition_sku_id = orderskulistitem.SkuId,
                        condition_sku_Batch = orderskulistitem.BetchId,
                        condition_sku_pack_size = orderskulistitem.PackSize,
                        condition_sku_amount = orderskulistitem.Qty,
                        offer_sku_id = 0,
                        offer_sku_pack_size = 0,
                        offer_sku_Batch = 0,
                        offer_sku_amount = tradePromotionCreateVm.ValueDiscount,
                        condition_bundle_qty_CS = 0,
                        condition_sku_group = 1
                    };
                    Db.tblt_TradePromotionDefinition.Add(tbltTradePromotionOrderDefinition);

                }   
            }

            HashSet<int> dbids = sp.Alldbids(tradePromotionCreateVm.RsmId, tradePromotionCreateVm.AsmId,
                tradePromotionCreateVm.CeId, tradePromotionCreateVm.DbId);

            var dbidList = Db.tbld_distribution_house.Where(
                x => dbids.Contains(x.DB_Id) && tradePromotionCreateVm.ClusterList.Contains(x.Cluster_id));


            foreach (var dbid in dbidList)
            {
                tblt_TradePromotionDBhouseMapping tbltTradePromotionDBhouseMapping =
                    new tblt_TradePromotionDBhouseMapping
                    {
                        promo_id = tpId,
                        db_id = dbid.DB_Id,
                        status = 1
                    };
                Db.tblt_TradePromotionDBhouseMapping.Add(tbltTradePromotionDBhouseMapping);
            }
            Db.SaveChanges();



            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOrderConditionPart(int offerType, int promotionType)
        {
            if (offerType == 1)
            {
                var fullskulist = (from a in Db.tbld_bundle_price_details
                    join b in Db.tbld_SKU on a.sku_id equals b.SKU_id into sku
                    from c in sku.DefaultIfEmpty()
                    where a.status == 1 && c.SKUStatus == 1 && a.bundle_price_id == 1
                    select (new {bundleid = a.id, SKUName = c.SKUName + "[" + a.batch_id + "]"}));



                ViewBag.FreeSKUList = new SelectList(fullskulist.ToList(), "bundleid", "SKUName");

                if (promotionType != 3)
                {
                    return PartialView("OrderConditionPart");
                }
                return PartialView("OrderConditionPart_slab");
            }

            if (offerType == 2 && promotionType != 3)
            {
                return PartialView("OrderConditionPart_Value");
            }
            return null;
        }


        [HttpPost]
        public ActionResult AddSlabRow(int count, int[] bundelitem, int[] skuList)
        {

            ViewBag.count = count;

            var fullskulist = (from a in Db.tbld_bundle_price_details
                join b in Db.tbld_SKU on a.sku_id equals b.SKU_id into sku
                from c in sku.DefaultIfEmpty()
                where a.status == 1 && c.SKUStatus == 1 && !bundelitem.Contains(a.id) && a.bundle_price_id == 1
                select (new {bundleid = a.id, SKUName = c.SKUName + "[" + a.batch_id + "]"}));



            ViewBag.SKUList = new SelectList(fullskulist.ToList(), "bundleid", "SKUName");

            return PartialView();
        }


        [HttpPost]
        public ActionResult AddRow(int count, int[] bundelitem, int[] skuList)
        {

            ViewBag.count = count;

            var fullskulist = (from a in Db.tbld_bundle_price_details
                join b in Db.tbld_SKU on a.sku_id equals b.SKU_id into sku
                from c in sku.DefaultIfEmpty()
                where a.status == 1 && c.SKUStatus == 1 && !bundelitem.Contains(a.id) && a.bundle_price_id == 1
                select (new {bundleid = a.id, SKUName = c.SKUName + "[" + a.batch_id + "]"}));



            ViewBag.SKUList = new SelectList(fullskulist.ToList(), "bundleid", "SKUName");

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
                    batch = a.batch_id
                }).ToList();


            return Json(skuitemDetails, JsonRequestBehavior.AllowGet);
        }





        // GET: TradePromotion/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblt_TradePromotion tbltTradePromotion = Db.tblt_TradePromotion.Find(id);



            if (tbltTradePromotion != null)
            {
                List<int> dbId = Db.tblt_TradePromotionDBhouseMapping.Where(x => x.promo_id == id).Select(x => x.db_id).ToList();

                TradePromotioninfoVm tradePromotioninfoVm = new TradePromotioninfoVm
                {
                    Name = tbltTradePromotion.name,
                    Code = tbltTradePromotion.code,
                    Description = tbltTradePromotion.code,
                    CreateDate = tbltTradePromotion.create_date,
                    StartDate = tbltTradePromotion.start_date,
                    EndDate = tbltTradePromotion.end_date,
                    IsActive=tbltTradePromotion.is_active,
                    ConditionType = Db.tblt_TradePromotionConditionType.Where(y=>y.id==Db.tblt_TradePromotionDefinition.Where(x=>x.promo_id==id && x.promo_line_type!=2).Select(x=>x.condition_type).FirstOrDefault()).Select(y=>y.ConditionType).FirstOrDefault(),
                    RuleType = Db.tblt_TradePromotionType.Where(y => y.promo_type_id == Db.tblt_TradePromotionDefinition.Where(x => x.promo_id == id && x.promo_line_type != 2).Select(x => x.rule_type).FirstOrDefault()).Select(y => y.promo_type_name).FirstOrDefault(),
                    OfferType = Db.tblt_TradePromotionOfferType.Where(y => y.id == Db.tblt_TradePromotionDefinition.Where(x => x.promo_id == id && x.promo_line_type != 2).Select(x => x.offer_type).FirstOrDefault()).Select(y => y.offertype).FirstOrDefault(),
                    Orderskulist = Db.tblt_TradePromotionDefinition.Where(x => x.promo_id == id && x.promo_line_type != 2).Select(x => new TpOrderskuDetailslistVm{Sku = Db.tbld_SKU.FirstOrDefault(y => y.SKU_id == x.condition_sku_id).SKUName,BetchId =x.condition_sku_Batch,PackSize = x.condition_sku_pack_size,Qty = x.condition_sku_amount,SlabCsQty = x.condition_bundle_qty_CS}).ToList(),
                    Freeskulist = Db.tblt_TradePromotionDefinition.Where(x => x.promo_id == id && x.promo_line_type == 2).Select(x => new TpOrderskuDetailslistVm { Sku =Db.tbld_SKU.FirstOrDefault(y => y.SKU_id == x.offer_sku_id).SKUName, BetchId = x.offer_sku_Batch, PackSize = x.offer_sku_pack_size, Qty = x.offer_sku_amount }).ToList()
                  };


                var dbList = from a in Db.tbld_db_zone_view 
                             where a.Status == 1 && dbId.Contains(a.DB_Id)
                             orderby  a.REGION_id,a.AREA_id,a.CEAREA_id,a.DB_Id
                             select (new { a.DB_Id, DBName = "[" + a.REGION_Name + "][" + a.AREA_Name + "]  [" + a.CEAREA_Name + "][" + a.DB_Name + "]" });
                                  

                ViewBag.DB = new SelectList(dbList.ToList(), "DB_Id", "DBName");

                var cluster =( from a in Db.tbld_distribution_house
                    join b in Db.tbld_cluster on a.Cluster_id equals b.id
                    where dbId.Contains(a.DB_Id)
                    select b).Distinct();

                ViewBag.Cluster = new SelectList(cluster.ToList(), "id", "name");

                return View(tradePromotioninfoVm);

            }

            return null;
        }


        public ActionResult StatusChange(int? id)
        {
            tblt_TradePromotion tbltTradePromotion = Db.tblt_TradePromotion.SingleOrDefault(x=>x.id==id);

            if (tbltTradePromotion != null)
            {
                tbltTradePromotion.is_active = tbltTradePromotion.is_active == 1 ? 2 : 1;
                Db.Entry(tbltTradePromotion).State = EntityState.Modified;
                Db.SaveChanges();
                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ExtendTradePromotion(int tpid, DateTime extenddate)
        {
            tblt_TradePromotion tbltTradePromotion = Db.tblt_TradePromotion.SingleOrDefault(x => x.id == tpid);

            if (tbltTradePromotion != null)
            {
                tbltTradePromotion.end_date = extenddate;
                tbltTradePromotion.expiry_date = extenddate;
                Db.Entry(tbltTradePromotion).State = EntityState.Modified;
                Db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
    
    }
}
