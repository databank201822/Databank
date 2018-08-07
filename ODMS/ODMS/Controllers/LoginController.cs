using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    public class LoginController : Controller
    {
        public  ODMSEntities Db = new ODMSEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "Login";
            return PartialView();
        }

        [HttpPost]
        public ActionResult Index(UserinfoVm userinfoVm)
        {

            Session["IpAddress"] = userinfoVm.IpAddress;
            if (ModelState.IsValid)
            {
                var x = Db.User_check(userinfoVm.UserName, userinfoVm.UserPassword).ToList(); //from Procudure for Zone
                var z = Db.DB_User_check(userinfoVm.UserName, userinfoVm.UserPassword).ToList();//from Procudure for db User
                if (x.Count() == 1)
                {
                    foreach (var y in x)
                    {
                        if (y.User_status == 1)
                        {
                            //var a = db.tbl_Manu_Role_mapping.Where(data => data.Roleid == y.User_role_id).ToList();
                            
                            Session["User_Id"] = y.User_Id;
                            Session["User_Name"] = y.User_Name;
                            Session["user_role_code"] = y.user_role_code;
                            Session["first_name"] = y.first_name;
                            Session["User_biz_role_id"] = y.User_biz_role_id;
                            Session["biz_zone_id"] = y.biz_zone_id;
                            Session["User_role_id"] = y.User_role_id;
                            Session["MenuMaster"] = Db.tbl_Manu_Role_mapping.Where(s=> s.Roleid == y.User_role_id).ToList();

                        }
                        else
                        {
                            ViewBag.Title = "Login";
                            ViewBag.alertbox = "error";
                            ViewBag.alertboxMsg = "Your User is InActive";
                            return PartialView();
                        }
                    }
                    return RedirectToAction("Index", "Home");

                }
                if (z.Count() == 1)
                {
                    foreach (var i in z)
                    {
                        if (i.active == 1)
                        {
                            var dbName = Db.tbld_distribution_house.Where(a => a.DB_Id == i.DistributionId).Select(p => p.DBName).ToList();
                            var systemDate = Db.tblt_System.Where(v=> v.DBid == i.DistributionId).Select(s=>s.CurrentDate).ToList();

                            Session["SystemDate"] = systemDate[0];
                            Session["User_Id"] = i.id;
                            Session["User_Name"] = i.login_user_id;
                            Session["user_role_code"] = i.user_role_code;
                            Session["first_name"] = i.Name;
                            Session["User_biz_role_id"] = i.biz_zone_category_id;
                            Session["biz_zone_id"] = i.Zone_id;
                            Session["User_role_id"] = i.User_role_id;
                            Session["DBId"] = i.DistributionId;
                            Session["DBName"] = dbName[0];

                        }
                        else
                        {
                            ViewBag.Title = "Login";
                            ViewBag.alertbox = "error";
                            ViewBag.alertboxMsg = "Your User is InActive";

                            return PartialView();
                        }
                    }


                    

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Title = "Login";
            ViewBag.alertbox = "error";
            ViewBag.alertboxMsg = "Please Enter valid User and Password";
            return PartialView();

        }

        [SessionExpire]
        public ActionResult Logout()
        {
            Session["User_Id"] = null;
            Session["User_Name"] = null;
            Session["user_role_code"] = null;
            Session["first_name"] = null;
            Session["User_biz_role_id"] = null;
            Session["biz_zone_id"] = null;
            Session["User_role_id"] = null;
            Session["DBId"] = null;
            Session["DBName"] = null;
            Session["SystemDate"] = null;
            return RedirectToAction("Index", "Login");

        }
    }

}

