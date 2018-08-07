using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{

[SessionExpire]
    public class DatauploadController : Controller
    {
        // GET: Dataupload

        public ODMSEntities Db = new ODMSEntities();
        public ActionResult Index()
        {
            return View();
        }
    [HttpPost]
        public ActionResult OutletUploadFormat(int distributor)
        {
            List<OutletUploadVm> outletFormatList = new List<OutletUploadVm>();

          
            var routeList = Db.tbld_distributor_Route.Where(x => x.DistributorID == distributor && x.RouteType == 2);
            var db = Db.tbld_distribution_house.SingleOrDefault(x => x.DB_Id==distributor);
            if (db != null)
            {
                var dbName = db.DBName;
                foreach (var item  in routeList)
                {
                   
                        OutletUploadVm outletFormat = new OutletUploadVm
                        {
                            Distributor = distributor,
                            DistributorName = db.DBName,
                            Parentid = item.RouteID,
                            ParentName = item.RouteName

                        };
                        outletFormatList.Add(outletFormat);
                    
                }

                var gv = new GridView();
                gv.DataSource = outletFormatList;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + dbName + "_OutletFormat.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
            }
            Response.Flush();
            Response.End();
            return View("Index");
        }
        public ActionResult OutletUpload()
        {
            ViewBag.Distributor = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1), "DB_Id", "DBName");
            return View();
        }
        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = Db.tbld_Outlet.Where(x => x.Distributorid == 1).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xlsx");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("Index");
        } 


    }
}