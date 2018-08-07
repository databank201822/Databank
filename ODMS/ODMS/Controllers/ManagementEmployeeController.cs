using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire,SuperAccess]
    public class ManagementEmployeeController : Controller
    {
        private ODMSEntities _db = new ODMSEntities();

        // GET: ManagementEmployee
        public ActionResult Index()
        {
            ViewBag.title = "Management Employee";
            var data = from a in _db.tbld_management_employee
                       join c in _db.tbld_business_zone on a.biz_zone_id equals c.id
                       join d in _db.tbld_business_zone_hierarchy on a.sales_role_id equals d.id into zoneHierarchy
                       from zone in zoneHierarchy.DefaultIfEmpty()
                       join b in _db.user_info on a.login_user_id equals b.User_Id into user
                       from userinfo in user.DefaultIfEmpty()
                       select new ManagementEmployeeiVMindex
                       {
                           Id = a.id,
                           FirstName = a.first_name,
                           BizZoneName = c.biz_zone_name,
                           SalesEmpCode = a.sales_emp_code,
                           SalesRole = zone.biz_zone_category_name,
                           LoginUserName = userinfo.User_Name
                       };


            return View(data.ToList());

        }

        // GET: ManagementEmployee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = (from a in _db.tbld_management_employee
                        join c in _db.tbld_business_zone on a.biz_zone_id equals c.id
                        join d in _db.tbld_business_zone_hierarchy on a.sales_role_id equals d.id into zoneHierarchy
                        from zone in zoneHierarchy.DefaultIfEmpty()
                        join b in _db.user_info on a.login_user_id equals b.User_Id into user
                        from userinfo in user.DefaultIfEmpty()
                        where a.id == id
                        select new ManagementEmployeeiVMindex
                        {
                            Id = a.id,
                            FirstName = a.first_name,
                            Email = a.email,
                            BizZoneName = c.biz_zone_name,
                            SalesEmpCode = a.sales_emp_code,
                            SalesRole = zone.biz_zone_category_name,
                            LoginUserName = userinfo.User_Name
                        }

                        ).ToList();


            ManagementEmployeeiVMindex emvi = new ManagementEmployeeiVMindex();
            foreach (var item in data)
            {
                emvi.Id = item.Id;
                emvi.SalesEmpCode = item.SalesEmpCode;
                emvi.FirstName = item.FirstName;
                emvi.Email = item.Email;
                emvi.SalesEmpAddress = item.SalesEmpAddress;
                emvi.SalesRole = item.SalesRole;
                emvi.LoginUserName = item.LoginUserName;
                emvi.BizZoneName = item.BizZoneName;
            }
            ViewBag.title = "Management Employee Details";
            return View(emvi);
        }

        // GET: ManagementEmployee/Create
        public ActionResult Create()
        {
           
            ViewBag.title = "Management Employee Create";

            ViewBag.ZoneCatagory = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "Id", "biz_zone_category_name");
            ViewBag.Zonebiz_zone_id = new SelectList("", "id", "biz_zone_name");
            ViewBag.login_user_id = new SelectList("", "User_Id", "User_Name");
            return View();
        }

        // POST: ManagementEmployee/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ManagementEmployeeiVm managementEmployeeiVm)
        {
           
            if (ModelState.IsValid)
            {
                tbld_management_employee tbldManagementEmployee = new tbld_management_employee
                {
                    id = managementEmployeeiVm.Id,
                    sales_emp_code = managementEmployeeiVm.SalesEmpCode,
                    first_name = managementEmployeeiVm.FirstName,
                    email = managementEmployeeiVm.Email,
                    sales_emp_address = managementEmployeeiVm.SalesEmpAddress,
                    sales_role_id = managementEmployeeiVm.SalesRoleId,
                    sales_manager_id = managementEmployeeiVm.SalesManagerId,
                    login_user_id = managementEmployeeiVm.LoginUserId,
                    biz_zone_id = managementEmployeeiVm.BizZoneId
                };
                _db.tbld_management_employee.Add(tbldManagementEmployee);
                _db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = managementEmployeeiVm.FirstName + " Employee Create Successfully";

                return RedirectToAction("Index");
            }


            ViewBag.title = "Management Employee Create";
            ViewBag.ZoneCatagory = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "Id", "biz_zone_category_name");
            ViewBag.Zonebiz_zone_id = new SelectList(_db.tbld_business_zone.Where(x => x.biz_zone_category_id == managementEmployeeiVm.SalesRoleId).ToList(), "id", "biz_zone_name");
            ViewBag.login_user_id = new SelectList(_db.user_info.Where(x => x.User_biz_role_id == managementEmployeeiVm.SalesRoleId).ToList(), "User_Id", "User_name");
            return View();
        }

        //For get Perent by Zone category
        public ActionResult GetPerentbycategoryId(int? id)
        {
            var tbldBusinessZone = _db.tbld_business_zone.Where(x => x.biz_zone_category_id == id).ToList();
            return Json(tbldBusinessZone, JsonRequestBehavior.AllowGet);
        }

        //For get User by Zone category
        public ActionResult GetuserbycategoryId(int? id)
        {
            var tbldBusinessZone = _db.user_info.Where(x => x.User_biz_role_id == id).ToList();
            return Json(tbldBusinessZone, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_management_employee tbldManagementEmployee = _db.tbld_management_employee.Find(id);

            if (tbldManagementEmployee != null)
            {
                ManagementEmployeeiVm managementEmployeeiVm = new ManagementEmployeeiVm
                {
                    Id = tbldManagementEmployee.id,
                    SalesEmpCode = tbldManagementEmployee.sales_emp_code,
                    FirstName = tbldManagementEmployee.first_name,
                    Email = tbldManagementEmployee.email,
                    SalesEmpAddress = tbldManagementEmployee.sales_emp_address,
                    SalesRoleId = tbldManagementEmployee.sales_role_id,
                    SalesManagerId = tbldManagementEmployee.sales_manager_id,
                    LoginUserId = tbldManagementEmployee.login_user_id,
                    BizZoneId = tbldManagementEmployee.biz_zone_id
                };

                ViewBag.title = "Management Employee Edit";
                ViewBag.ZoneCatagory = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "Id", "biz_zone_category_name");
                ViewBag.Zonebiz_zone_id = new SelectList(_db.tbld_business_zone.Where(x => x.biz_zone_category_id == managementEmployeeiVm.SalesRoleId).ToList(), "id", "biz_zone_name");
                ViewBag.login_id = new SelectList(_db.user_info.Where(x => x.User_biz_role_id == managementEmployeeiVm.SalesRoleId).ToList(), "User_Id", "User_Name");
                return View(managementEmployeeiVm);
            }

            return RedirectToAction("Index");
        }

        // POST: tbld_management_employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManagementEmployeeiVm managementEmployeeiVm)
        {


            if (ModelState.IsValid)
            {

                tbld_management_employee tbldManagementEmployee = new tbld_management_employee
                {
                    id = managementEmployeeiVm.Id,
                    sales_emp_code = managementEmployeeiVm.SalesEmpCode,
                    first_name = managementEmployeeiVm.FirstName,
                    email = managementEmployeeiVm.Email,
                    sales_emp_address = managementEmployeeiVm.SalesEmpAddress,
                    sales_role_id = managementEmployeeiVm.SalesRoleId,
                    sales_manager_id = managementEmployeeiVm.SalesManagerId,
                    login_user_id = managementEmployeeiVm.LoginUserId,
                    biz_zone_id = managementEmployeeiVm.BizZoneId
                };

                _db.Entry(tbldManagementEmployee).State = EntityState.Modified;
                _db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = managementEmployeeiVm.FirstName + " Employee Edit Successfully";

                return RedirectToAction("Index");


            }
            ViewBag.title = "Management Employee Edit";
            ViewBag.ZoneCatagory = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "Id", "biz_zone_category_name");
            ViewBag.Zonebiz_zone_id = new SelectList(_db.tbld_business_zone.Where(x => x.biz_zone_category_id == managementEmployeeiVm.SalesRoleId).ToList(), "id", "biz_zone_name");
            ViewBag.login_user_id = new SelectList(_db.user_info.ToList(), "User_Id", "User_name");
            return View(managementEmployeeiVm);
        }



        // GET: ManagementEmployee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_management_employee tbldManagementEmployee = _db.tbld_management_employee.Find(id);
            if (tbldManagementEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tbldManagementEmployee);
        }

        // POST: ManagementEmployee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_management_employee tbldManagementEmployee = _db.tbld_management_employee.Find(id);
            if (tbldManagementEmployee != null)
            {
                _db.tbld_management_employee.Remove(tbldManagementEmployee);
                _db.SaveChanges();

                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = tbldManagementEmployee.first_name + " Employee Delete Successfully";
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
