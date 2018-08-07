using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class DBhouseemployeeController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();

        // GET: DBhouseemployee
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


            var data = from a in Db.tbld_distribution_employee
                       join b in Db.tbld_distribution_employee_Type on a.Emp_Type equals b.EmpTypeid
                       join c in Db.tbld_distribution_house on a.DistributionId equals c.DB_Id
                       join d in Db.status on a.active equals d.status_Id
                       where dbids.Contains(c.DB_Id)
                       select new DBhouseemployeeiVm
                       {
                           Id = a.id,
                           EmpCode = a.Emp_code,
                           Name = a.Name,
                           Distribution = c.DBName,
                           EmpType = b.EmpType,
                           LoginUserId = a.login_user_id,
                           Active = d.status_code

                       };

            return PartialView(data.ToList());

        }

        // GET: DBhouseemployee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = (from a in Db.tbld_distribution_employee
                        join b in Db.tbld_distribution_employee_Type on a.Emp_Type equals b.EmpTypeid
                        join c in Db.tbld_distribution_house on a.DistributionId equals c.DB_Id
                        join d in Db.status on a.active equals d.status_Id
                        where a.id == id
                        select new DBhouseemployeeiVm
                        {
                            Id = a.id,
                            EmpCode = a.Emp_code,
                            Name = a.Name,
                            Distribution = c.DBName,
                            EmpType = b.EmpType,
                            LoginUserId = a.login_user_id,
                            Active = d.status_code,
                            EmpAddress = a.Emp_address,
                            LoginUserPassword = a.login_user_password,
                            ContactNo = a.contact_no,
                            JoiningDate = a.joining_date,
                            Designation = a.designation,
                            Dob = a.d_o_b,
                            Email = a.email,
                            EmergencyContactPerson = a.emergency_contact_person,
                            EmergencyContactNumber = a.emergency_contact_number,
                            EducationalQualification = a.educational_qualification,
                            Image = a.image

                        }).ToList();


            DBhouseemployeeiVm dBhouseemployeeiVm = new DBhouseemployeeiVm();

            foreach (var x in data)
            {
                dBhouseemployeeiVm.Id = x.Id;
                dBhouseemployeeiVm.EmpCode = x.EmpCode;
                dBhouseemployeeiVm.Name = x.Name;
                dBhouseemployeeiVm.Distribution = x.Distribution;
                dBhouseemployeeiVm.EmpType = x.EmpType;
                dBhouseemployeeiVm.LoginUserId = x.LoginUserId;
                dBhouseemployeeiVm.Active = x.EmpType;
                dBhouseemployeeiVm.EmpAddress = x.EmpAddress;
                dBhouseemployeeiVm.LoginUserPassword = x.LoginUserPassword;
                dBhouseemployeeiVm.ContactNo = x.ContactNo;
                dBhouseemployeeiVm.JoiningDate = x.JoiningDate;
                dBhouseemployeeiVm.Designation = x.Designation;
                dBhouseemployeeiVm.Dob = x.Dob;
                dBhouseemployeeiVm.Email = x.Email;
                dBhouseemployeeiVm.EmergencyContactPerson = x.EmergencyContactPerson;
                dBhouseemployeeiVm.EmergencyContactNumber = x.EmergencyContactNumber;
                dBhouseemployeeiVm.EducationalQualification = x.EducationalQualification;
                dBhouseemployeeiVm.Image = x.Image;
            }

            return View(dBhouseemployeeiVm);
        }

        // GET: DBhouseemployee/Create
       [CreateAccess]
        public ActionResult Create()
        {
            ViewBag.user_role = new SelectList(Db.user_role.Where(x=>x.user_role_id==7 || x.user_role_id==8).ToList(), "user_role_id", "user_role_name");
            ViewBag.db = new SelectList(Db.tbld_distribution_house.Where(x=>x.Status==1).ToList(), "DB_Id", "DBName");
            ViewBag.type = new SelectList(Db.tbld_distribution_employee_Type.ToList(), "EmpTypeid", "EmpType");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            return View();
        }

        // POST: DBhouseemployee/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DBhouseemployeeVm dBhouseemployeeVm, HttpPostedFileBase image)
        {
            var imagePath = "~/App_res/empImage/";

            if (ModelState.IsValid)
            {
                string recentGenerateId = DateTime.Now.ToString("yyyy-MMddhhmmss");
                if (image != null)
                {


                    image.SaveAs(Server.MapPath(imagePath + recentGenerateId +
                                                System.IO.Path.GetExtension(image.FileName)));
                    if (System.IO.File.Exists(imagePath + image.FileName))
                    {
                        System.IO.File.Delete(imagePath + image.FileName);

                    }
                    dBhouseemployeeVm.Image = image.FileName;
                }
                else
                {
                    dBhouseemployeeVm.Image = "NA";
                }



                tbld_distribution_employee tbldDistributionEmployee = new tbld_distribution_employee()
                    {
                        
                        Emp_code = dBhouseemployeeVm.EmpCode,
                        Name = dBhouseemployeeVm.Name,
                        Emp_address = dBhouseemployeeVm.EmpAddress,
                        User_role_id = dBhouseemployeeVm.UserRoleId,
                        DistributionId = dBhouseemployeeVm.DistributionId,
                        login_user_id = dBhouseemployeeVm.LoginUserId,
                        login_user_password = dBhouseemployeeVm.LoginUserPassword,
                        contact_no = dBhouseemployeeVm.ContactNo,
                        joining_date = dBhouseemployeeVm.JoiningDate,
                        designation = dBhouseemployeeVm.Designation,
                        Emp_Type = dBhouseemployeeVm.EmpType,
                        d_o_b = dBhouseemployeeVm.Dob,
                        email = dBhouseemployeeVm.Email,
                        emergency_contact_person = dBhouseemployeeVm.EmergencyContactPerson,
                        emergency_contact_number = dBhouseemployeeVm.EmergencyContactNumber,
                        educational_qualification = dBhouseemployeeVm.EducationalQualification,
                        image = recentGenerateId,
                        active = dBhouseemployeeVm.Active

                    };



                    Db.tbld_distribution_employee.Add(tbldDistributionEmployee);

                    Db.SaveChanges();
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseemployeeVm.Name + "  Create Successfully";
                return RedirectToAction("Index");
            }

            ViewBag.user_role = new SelectList(Db.user_role.Where(x => x.user_role_id == 7 || x.user_role_id == 8).ToList(), "user_role_id", "user_role_name");
            ViewBag.db = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
            ViewBag.type = new SelectList(Db.tbld_distribution_employee_Type.ToList(), "EmpTypeid", "EmpType");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            return View(dBhouseemployeeVm);
        }

        // GET: DBhouseemployee/Edit/5

        [EditAccess]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distribution_employee tbldDistributionEmployee = Db.tbld_distribution_employee.Find(id);
            if (tbldDistributionEmployee == null)
            {
                return HttpNotFound();
            }

            if (tbldDistributionEmployee.active != null)
            {
                DBhouseemployeeVm dBhouseemployeeVm = new DBhouseemployeeVm()
                {
                    Id = tbldDistributionEmployee.id,
                    EmpCode = tbldDistributionEmployee.Emp_code,
                    Name = tbldDistributionEmployee.Name,
                    EmpAddress = tbldDistributionEmployee.Emp_address,
                    UserRoleId = tbldDistributionEmployee.User_role_id,
                    DistributionId = tbldDistributionEmployee.DistributionId,
                    LoginUserId = tbldDistributionEmployee.login_user_id,
                    LoginUserPassword = tbldDistributionEmployee.login_user_password,
                    ContactNo = tbldDistributionEmployee.contact_no,
                    JoiningDate = tbldDistributionEmployee.joining_date,
                    Designation = tbldDistributionEmployee.designation,
                    EmpType = tbldDistributionEmployee.Emp_Type,
                    Dob = tbldDistributionEmployee.d_o_b,
                    Email = tbldDistributionEmployee.email,
                    EmergencyContactPerson = tbldDistributionEmployee.emergency_contact_person,
                    EmergencyContactNumber = tbldDistributionEmployee.emergency_contact_number,
                    EducationalQualification = tbldDistributionEmployee.educational_qualification,
                    Image = tbldDistributionEmployee.image,
                    Active = (int) tbldDistributionEmployee.active

                };
                ViewBag.user_role = new SelectList(Db.user_role.Where(x => x.user_role_id == 7 || x.user_role_id == 8).ToList(), "user_role_id", "user_role_name");
                ViewBag.db = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
                ViewBag.type = new SelectList(Db.tbld_distribution_employee_Type.ToList(), "EmpTypeid", "EmpType");
                ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
                return View(dBhouseemployeeVm);
            }
            return HttpNotFound();
        }

        // POST: DBhouseemployee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DBhouseemployeeVm dBhouseemployeeVm, HttpPostedFileBase image)
        {
            var imagePath = "~/App_res/empImage/";

            if (ModelState.IsValid)
            {
                string recentGenerateId = DateTime.Now.ToString("yyyy-MMddhhmmss");
                if (image != null)
                {


                    image.SaveAs(Server.MapPath(imagePath + recentGenerateId +
                                                System.IO.Path.GetExtension(image.FileName)));
                    if (System.IO.File.Exists(imagePath + image.FileName))
                    {
                        System.IO.File.Delete(imagePath + image.FileName);

                    }
                    dBhouseemployeeVm.Image = image.FileName;
                }
                else
                {
                    dBhouseemployeeVm.Image = "NA";
                }



                tbld_distribution_employee tbldDistributionEmployee = new tbld_distribution_employee()
                {
                    id =dBhouseemployeeVm.Id,
                    Emp_code = dBhouseemployeeVm.EmpCode,
                    Name = dBhouseemployeeVm.Name,
                    Emp_address = dBhouseemployeeVm.EmpAddress,
                    User_role_id = dBhouseemployeeVm.UserRoleId,
                    DistributionId = dBhouseemployeeVm.DistributionId,
                    login_user_id = dBhouseemployeeVm.LoginUserId,
                    login_user_password = dBhouseemployeeVm.LoginUserPassword,
                    contact_no = dBhouseemployeeVm.ContactNo,
                    joining_date = dBhouseemployeeVm.JoiningDate,
                    designation = dBhouseemployeeVm.Designation,
                    Emp_Type = dBhouseemployeeVm.EmpType,
                    d_o_b = dBhouseemployeeVm.Dob,
                    email = dBhouseemployeeVm.Email,
                    emergency_contact_person = dBhouseemployeeVm.EmergencyContactPerson,
                    emergency_contact_number = dBhouseemployeeVm.EmergencyContactNumber,
                    educational_qualification = dBhouseemployeeVm.EducationalQualification,
                    image = recentGenerateId,
                    active = dBhouseemployeeVm.Active

                };


                Db.Entry(tbldDistributionEmployee).State = EntityState.Modified;
                Db.SaveChanges();


                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = dBhouseemployeeVm.Name + "  Update Successfully";
                return RedirectToAction("Index");
            }

            ViewBag.user_role = new SelectList(Db.user_role.Where(x => x.user_role_id == 7 || x.user_role_id == 8).ToList(), "user_role_id", "user_role_name");
            ViewBag.db = new SelectList(Db.tbld_distribution_house.Where(x => x.Status == 1).ToList(), "DB_Id", "DBName");
            ViewBag.type = new SelectList(Db.tbld_distribution_employee_Type.ToList(), "EmpTypeid", "EmpType");
            ViewBag.status = new SelectList(Db.status.ToList(), "status_Id", "status_code");
            return View(dBhouseemployeeVm);
        }

        // GET: DBhouseemployee/Delete/5

        [DeleteAccess]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_distribution_employee tbldDistributionEmployee = Db.tbld_distribution_employee.Find(id);
            if (tbldDistributionEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tbldDistributionEmployee);
        }

        // POST: DBhouseemployee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_distribution_employee tbldDistributionEmployee = Db.tbld_distribution_employee.Find(id);
            if (tbldDistributionEmployee != null) Db.tbld_distribution_employee.Remove(tbldDistributionEmployee);
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
