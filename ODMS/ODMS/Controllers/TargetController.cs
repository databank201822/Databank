using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToExcel;
using ODMS.Models;
using ODMS.Models.ViewModel;


namespace ODMS.Controllers
{
    [SessionExpire]
    public class TargetController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        // GET: Target
        public ActionResult Index()
        {
            var tgt = Db.tbld_Target.OrderByDescending(x=>x.id).ToList();
            return View(tgt);
        }


        // GET: tbld_Target/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TargetVm targetVm)
        {
            if (ModelState.IsValid)
            {
                tbld_Target tbldTarget = new tbld_Target
                {
                    name = targetVm.Name,
                    start_date = targetVm.StartDate,
                    end_date = targetVm.EndDate
                };

                Db.tbld_Target.Add(tbldTarget);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(targetVm);
        }

        public ActionResult UploadTarget()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadTargetbyDb(HttpPostedFileBase fileUpload)
        {
            int targetid=0 ;
            List<tbld_Target_Details> tbldTargetDetails = new List<tbld_Target_Details>();

            if (fileUpload == null)
                return View("UploadTarget");
            if (fileUpload.ContentType == "application/vnd.ms-excel" || fileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                string filename = fileUpload.FileName;

                if (filename.EndsWith(".xlsx"))
                {
                    string targetpath = Server.MapPath("~/Upload/");
                    fileUpload.SaveAs(targetpath + filename);
                    var pathToExcelFile = targetpath + filename;

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var worksheetNames = excelFile.GetWorksheetNames();

                    string sheetName = worksheetNames.First();


                    var tgtList = from a in excelFile.Worksheet<TargetDetailsVm>(sheetName) select a;
                   
                    foreach (var tgtListitem in tgtList)
                    {
                        tbld_Target_Details targetitem = new tbld_Target_Details()
                        {
                            target_id = tgtListitem.TargetId,
                            db_id = tgtListitem.DbId,
                            sku_id = tgtListitem.SkuId,
                            Pack_size = tgtListitem.PackSize,
                            qtyinCS = tgtListitem.QtyinCs,
                            qtyinPS = tgtListitem.QtyinPs,
                            total_Qty = (tgtListitem.QtyinCs * tgtListitem.PackSize) + tgtListitem.QtyinPs

                        };
                        targetid = tgtListitem.TargetId;

                        tbldTargetDetails.Add(targetitem);
                        Db.tbld_Target_Details.Add(targetitem);
                    }

                   
                    int tergetCount = Db.tbld_Target_Details.Count(x => x.target_id == targetid);

                    if (tergetCount <= 0)
                    {
                        if (tgtList.Count() == tbldTargetDetails.Count())
                        {

                            Db.SaveChanges();
                            TempData["alertbox"] = "success";
                            TempData["alertboxMsg"] = "  Target Upload Successfully";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["alertbox"] = "error";
                        TempData["alertboxMsg"] =" Target Already uploaded";

                        return RedirectToAction("Index");
                    }

                    

                }
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UploadTargetbyPsr(HttpPostedFileBase fileUpload)
        {
            int targetid = 0;
            List<tbld_Target_PSR_Details> tbldTargetDetails = new List<tbld_Target_PSR_Details>();

            if (fileUpload == null)
            {
                TempData["alertbox"] = "error";
                TempData["alertboxMsg"] = " Target not uploaded";

                return RedirectToAction("Index");
            }
            if (fileUpload.ContentType == "application/vnd.ms-excel" || fileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                string filename = fileUpload.FileName;

                if (filename.EndsWith(".xlsx"))
                {
                    string targetpath = Server.MapPath("~/Upload/");
                    fileUpload.SaveAs(targetpath + filename);
                    var pathToExcelFile = targetpath + filename;

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var worksheetNames = excelFile.GetWorksheetNames();

                    string sheetName = worksheetNames.First();


                    var tgtList = from a in excelFile.Worksheet<TargetPsrDetailsVm>(sheetName) select a;

                    foreach (var tgtListitem in tgtList)
                    {
                        tbld_Target_PSR_Details targetitem = new tbld_Target_PSR_Details()
                        {
                            target_id = tgtListitem.TargetId,
                            db_id = tgtListitem.DbId,
                            psr_id = tgtListitem.PsrId,
                            sku_id = tgtListitem.SkuId,
                            Pack_size = tgtListitem.PackSize,
                            qtyinCS = tgtListitem.QtyinCs,
                            qtyinPS = tgtListitem.QtyinPs,
                            total_Qty = (tgtListitem.QtyinCs * tgtListitem.PackSize) + tgtListitem.QtyinPs

                        };
                        targetid = tgtListitem.TargetId;

                        tbldTargetDetails.Add(targetitem);
                        Db.tbld_Target_PSR_Details.Add(targetitem);
                    }

                    var dbTargetDetails = (from a in tbldTargetDetails
                        group a by new
                        {
                            a.target_id,
                            a.db_id,
                            a.sku_id,
                            a. Pack_size,
                        }
                        into g
                        select new tbld_Target_Details
                        {
                            target_id = g.Key.target_id,
                            db_id = (int)g.Key.db_id,
                            sku_id = (int)g.Key.sku_id,
                            Pack_size=g.Key.Pack_size,
                            qtyinCS = g.Sum(p=>p.total_Qty)/g.Key.Pack_size,
                            qtyinPS = g.Sum(p => p.total_Qty) % g.Key.Pack_size,
                            total_Qty = g.Sum(p=>p.total_Qty)
                            
                        }).ToList();
                  
                    
                    int tergetCount = Db.tbld_Target_PSR_Details.Count(x => x.target_id == targetid);

                    if (tergetCount <= 0)
                    {
                        Db.tbld_Target_Details.AddRange(dbTargetDetails);

                     
                        if (tgtList.Count() == tbldTargetDetails.Count())
                        {

                            Db.SaveChanges();
                            TempData["alertbox"] = "success";
                            TempData["alertboxMsg"] = "  Target Upload Successfully";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["alertbox"] = "error";
                        TempData["alertboxMsg"] = " Target Already uploaded";

                        return RedirectToAction("Index");
                    }



                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult DownloadDbTargetFormat(int id)
        {

            var tgt = Db.tbld_Target.SingleOrDefault(x => x.id == id);

            if (tgt != null)
            {
                List<TargetDetailsVm> targetDetailsList = new List<TargetDetailsVm>();


                var skuList = from a in Db.tbld_SKU
                              join b in Db.tbld_SKU_unit on a.SKUUnit equals b.id
                              where a.SKUStatus == 1
                              orderby a.SKU_id, a.SKUbrand_id
                              select new
                              {
                                  sku_id = a.SKU_id,
                                  sku_name = a.SKUName,
                                  pack_size = b.qty
                              };

                var dblist = Db.tbld_db_zone_view.Where(x => x.Status == 1);


                foreach (var dbitem in dblist)
                {
                    foreach (var skuitem in skuList)
                    {
                        TargetDetailsVm targetDetailsitem = new TargetDetailsVm
                        {
                            TargetId = tgt.id,
                            TargetName = tgt.name,
                            RegionName = dbitem.REGION_Name,
                            AreaName = dbitem.AREA_Name,
                            CeAreaName = dbitem.CEAREA_Name,
                            DbId = dbitem.DB_Id,
                            DbName = dbitem.DB_Name,
                            SkuId = skuitem.sku_id,
                            SkuName = skuitem.sku_name,
                            PackSize = skuitem.pack_size
                        };
                        targetDetailsList.Add(targetDetailsitem);
                    }
                }



                var gv = new GridView { DataSource = targetDetailsList };

                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + tgt.name + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());

                Response.Flush();
                Response.End();
            }
            return View("Index");
        }


        public ActionResult DownloadpsrTargetFormat(int id)
        {

            var tgt = Db.tbld_Target.SingleOrDefault(x => x.id == id);

            if (tgt != null)
            {
                List<TargetPsrDetailsVm> targetDetailsList = new List<TargetPsrDetailsVm>();


                var skuList = from a in Db.tbld_SKU
                    join b in Db.tbld_SKU_unit on a.SKUUnit equals b.id
                    where a.SKUStatus == 1
                    orderby a.SKU_id, a.SKUbrand_id
                    select new
                    {
                        sku_id = a.SKU_id,
                        sku_name = a.SKUName,
                        pack_size = b.qty
                    };

                var dblist = from a in Db.tbld_db_zone_view
                    join b in Db.tbld_distribution_employee on a.DB_Id equals b.DistributionId
                    where a.Status == 1 && b.Emp_Type==2
                    select new
                    {
                        a.REGION_Name,
                        a.AREA_Name,
                        a.CEAREA_Name,
                        a.DB_Id,
                        a.DB_Name,
                        PsrId = b.id,
                        PsrName = b.Name
                    };


                foreach (var dbitem in dblist)
                {
                    foreach (var skuitem in skuList)
                    {
                        TargetPsrDetailsVm targetDetailsitem = new TargetPsrDetailsVm
                        {
                            TargetId = tgt.id,
                            TargetName = tgt.name,
                            RegionName = dbitem.REGION_Name,
                            AreaName = dbitem.AREA_Name,
                            CeAreaName = dbitem.CEAREA_Name,
                            DbId = dbitem.DB_Id,
                            DbName = dbitem.DB_Name,
                            PsrId = dbitem.PsrId,
                            PsrName = dbitem.PsrName,
                            SkuId = skuitem.sku_id,
                            SkuName = skuitem.sku_name,
                            PackSize = skuitem.pack_size
                        };
                        targetDetailsList.Add(targetDetailsitem);
                    }
                }



                var gv = new GridView { DataSource = targetDetailsList };

                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + tgt.name + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());

                Response.Flush();
                Response.End();
            }
            return View("Index");
        }
        public ActionResult Details(int id)
        {
            List<int>dbList=new List<int>();
            var tgt = Db.tbld_Target.SingleOrDefault(x => x.id == id);

           
            if (Session["DBId"] != null)
            {
                dbList.Add((int) Session["DBId"]);

            }
            else
            {
                dbList = Db.tbld_distribution_house.Where(x => x.Status == 1).Select(x => x.DB_Id).ToList();

            }

            if (tgt != null)
            {


                var tgtdetails = from a in Db.tbld_Target_Details
                                 join b in Db.tbld_db_zone_view on a.db_id equals b.DB_Id
                                 join c in Db.tbld_SKU on a.sku_id equals c.SKU_id
                                 where a.target_id == id && dbList.Contains(b.DB_Id)
                                 select new TargetDetailsVm
                             {
                                 TargetId = tgt.id,
                                 TargetName = tgt.name,
                                 RegionName = b.REGION_Name,
                                 AreaName = b.AREA_Name,
                                 CeAreaName = b.CEAREA_Name,
                                 DbId = b.DB_Id,
                                 DbName = b.DB_Name,
                                 SkuId = a.sku_id,
                                 SkuName = c.SKUName,
                                 PackSize = a.Pack_size,
                                 QtyinCs = a.qtyinCS,
                                 QtyinPs = a.qtyinPS,
                                 TotalQty = a.total_Qty
                             };


                var gv = new GridView { DataSource = tgtdetails.ToList() };

                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + tgt.name + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());

                Response.Flush();
                Response.End();
            }
            return View("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.targetid = id;
            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
            return View();
        }


        public ActionResult EditFilter(int id,int dbId)
        {
            List<TargetDetailsVm> targetDetailsVm=new List<TargetDetailsVm>();

            var skuList = from a in Db.tbld_SKU
                join c in Db.tbld_SKU_unit on a.SKUUnit equals c.id into qs
                from q in qs.DefaultIfEmpty()
                where a.SKUStatus == 1 
                select new {SkuId = a.SKU_id,
                    SkuName = a.SKUName,
                    PackSize = q.qty,
                };

            foreach (var skuitem in skuList)
            {
                int tgtqty = 0;
                var tbldTargetDetails = Db.tbld_Target_Details
                    .SingleOrDefault(x => x.target_id == id && x.db_id == dbId && x.sku_id == skuitem.SkuId);
                if (tbldTargetDetails != null)
                {
                    tgtqty = (int) tbldTargetDetails.total_Qty;
                }
               
                TargetDetailsVm targetDetailsitem = new TargetDetailsVm
                {
                    TargetId = id,
                    DbId = dbId,
                    SkuId = skuitem.SkuId,
                    SkuName = skuitem.SkuName,
                    PackSize = skuitem.PackSize,
                    QtyinCs =  Convert.ToInt32(tgtqty / skuitem.PackSize),
                    QtyinPs =  Convert.ToInt32(tgtqty % skuitem.PackSize),
                    
                };
                targetDetailsVm.Add(targetDetailsitem);
            }


          //  var data = Db.tbld_Target_Details.Where(x => x.target_id == id && x.db_id == dbId);
            return PartialView(targetDetailsVm);
        }

        [HttpPost]
        public ActionResult TargerEditbyDb(List<TargetDetailsVm> targetDetailsVm)
        {
            var firstOrDefault = targetDetailsVm.FirstOrDefault();


            if (firstOrDefault != null)
            {
                int targetid = firstOrDefault.TargetId;
                int dbId = firstOrDefault.DbId;

                Db.tbld_Target_Details.RemoveRange(
                    Db.tbld_Target_Details.Where(x => x.target_id == targetid && x.db_id == dbId));
                Db.SaveChanges();
                foreach (var targetDetailsVmitem in targetDetailsVm)
                {
                    int total = (int) ((targetDetailsVmitem.QtyinCs * targetDetailsVmitem.PackSize) +targetDetailsVmitem.QtyinPs);
                    if (total > 0)
                    {
                        tbld_Target_Details tbldTargetDetails = new tbld_Target_Details
                        {
                            target_id = targetDetailsVmitem.TargetId,
                            db_id = targetDetailsVmitem.DbId,
                            sku_id = targetDetailsVmitem.SkuId,
                            Pack_size = targetDetailsVmitem.PackSize,
                            qtyinCS = targetDetailsVmitem.QtyinCs,
                            qtyinPS = targetDetailsVmitem.QtyinPs,
                            total_Qty = total

                        };
                        Db.tbld_Target_Details.Add(tbldTargetDetails);
                        Db.SaveChanges();
                    }
                }

                Db.tbld_Target_PSR_Details.RemoveRange(Db.tbld_Target_PSR_Details.Where(x => x.target_id == targetid && x.db_id == dbId));
                Db.SaveChanges();
                return RedirectToAction("Edit", "Target", new { id = targetid });
               
            }
            return null;


        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Target tbldTarget = Db.tbld_Target.Find(id);
            if (tbldTarget == null)
            {
                return HttpNotFound();
            }
            return View(tbldTarget);
        }

        // POST: tbld_Target/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_Target tbldTarget = Db.tbld_Target.Find(id);
            if (tbldTarget != null) Db.tbld_Target.Remove(tbldTarget);
            Db.SaveChanges();

            var tbldTargetDetails = Db.tbld_Target_Details.Where(x => x.target_id == id).ToList();
            Db.tbld_Target_Details.RemoveRange(tbldTargetDetails);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteTargetDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Target tbldTarget = Db.tbld_Target.Find(id);
            if (tbldTarget == null)
            {
                return HttpNotFound();
            }
            return View(tbldTarget);
        }

        // POST: tbld_Target/Delete/5
        [HttpPost, ActionName("DeleteTargetDetails")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTargetDetailsConfirmed(int id)
        {
            var tbldTargetDetails = Db.tbld_Target_Details.Where(x => x.target_id == id).ToList();
            Db.tbld_Target_Details.RemoveRange(tbldTargetDetails);
            Db.SaveChanges();

            TempData["alertbox"] = "success";
            TempData["alertboxMsg"] = "  Target Delete Successfully";
            return RedirectToAction("Index");
        }



        public ActionResult TargetBreakDownIndex()
        {

            var tgt = Db.tbld_Target.OrderByDescending(x => x.id).ToList();
            return View(tgt);
        }



        public ActionResult PsrTargetBreakDownIndex(int? id)
        {
            int dbid = (int) Session["DBId"];
            var psrlist = Db.tbld_distribution_employee.Where(x => x.DistributionId == dbid && x.Emp_Type == 2 && x.active == 1).Select(x => new PsrTargetBreak { Tgtid = (int)id, PsrId = x.id, PsrCode=x.Emp_code, PsrName = x.Name }).ToList();
            ViewBag.Target = id;
            ViewBag.TotalTarget = Db.tbld_Target_Details.Where(x => x.db_id == dbid && x.target_id == id)
                .Sum(x => x.total_Qty / x.Pack_size);
            return View(psrlist);
        }






        [HttpPost]
        public ActionResult PsrTargetBreakDownSave(List<PsrTargetBreak> psrTargetBreak)
        {
            List<tbld_Target_PSR_Details> psrTargetDetails = new List<tbld_Target_PSR_Details>();

            if (psrTargetBreak!=null)
            {
                int dbid = (int) Session["DBId"];
                int tgtid = psrTargetBreak.Select(x => x.Tgtid).FirstOrDefault();
                var dbTgt = Db.tbld_Target_Details.Where(x => x.db_id == dbid && x.target_id == tgtid);
                foreach (var item in psrTargetBreak)
                {
                    foreach (var tgtitem in dbTgt)
                    {
                        tbld_Target_PSR_Details psrDetails = new tbld_Target_PSR_Details
                        {
                            sku_id = tgtitem.sku_id,
                            target_id = tgtid,
                            Pack_size = tgtitem.Pack_size,
                            db_id = tgtitem.db_id,
                            psr_id = item.PsrId,
                            qtyinCS = Math.Floor((tgtitem.total_Qty / 100 * item.Contribution)/ tgtitem.Pack_size) ,
                            qtyinPS = Math.Floor((tgtitem.total_Qty / 100 * item.Contribution) % tgtitem.Pack_size),
                            total_Qty = Math.Floor(tgtitem.total_Qty / 100 * item.Contribution)
                        };

                        psrTargetDetails.Add(psrDetails);
                    }
 

                }
                Db.tbld_Target_PSR_Details.RemoveRange(Db.tbld_Target_PSR_Details.Where(x => x.db_id == dbid && x.target_id == tgtid));
                Db.SaveChanges();

                Db.tbld_Target_PSR_Details.AddRange(psrTargetDetails);

                Db.SaveChanges();
            }
            TempData["alertbox"] = "success";
            TempData["alertboxMsg"] = "  Target Break Down Successfully";
            return RedirectToAction("TargetBreakDownIndex");
        }

        public ActionResult PsrDetails(int id)
        {
            List<int> dbList = new List<int>();
            var tgt = Db.tbld_Target.SingleOrDefault(x => x.id == id);


            if (Session["DBId"] != null)
            {
                dbList.Add((int)Session["DBId"]);

            }
            else
            {
                dbList = Db.tbld_distribution_house.Where(x => x.Status == 1).Select(x => x.DB_Id).ToList();

            }

            if (tgt != null)
            {
                var tgtdetails = from a in Db.tbld_Target_PSR_Details
                    join b in Db.tbld_db_zone_view on a.db_id equals b.DB_Id
                    join c in Db.tbld_SKU on a.sku_id equals c.SKU_id
                    join d in Db.tbld_distribution_employee on a.psr_id equals d.id
                    where a.target_id == id && dbList.Contains(b.DB_Id)
                    select new TargetPsrDetailsVm
                    {
                        TargetId = tgt.id,
                        TargetName = tgt.name,
                        RegionName = b.REGION_Name,
                        AreaName = b.AREA_Name,
                        CeAreaName = b.CEAREA_Name,
                        DbId = b.DB_Id,
                        DbName = b.DB_Name,
                        PsrId = d.id,
                        PsrName = d.Name,
                        SkuId = a.sku_id,
                        SkuName = c.SKUName,
                        PackSize = a.Pack_size,
                        QtyinCs = a.qtyinCS,
                        QtyinPs = a.qtyinPS,
                        TotalQty = a.total_Qty
                    };


                var gv = new GridView { DataSource = tgtdetails.ToList() };

                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + tgt.name + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());

                Response.Flush();
                Response.End();
            }
            return View("Index");
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