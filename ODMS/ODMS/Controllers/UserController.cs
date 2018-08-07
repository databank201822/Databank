using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire,SuperAccess]
    public class UserController : Controller
    {
        private ODMSEntities _db = new ODMSEntities();

        // GET: user
        public ActionResult Index()
        {
            var data = from a in _db.user_info
                       join b in _db.user_role on a.User_role_id equals b.user_role_id
                       join c in _db.status on a.User_status equals c.status_Id
                       join d in _db.tbld_business_zone_hierarchy on a.User_biz_role_id equals d.id into userZone
                       from bizzone in userZone.DefaultIfEmpty()
                       select new UseriVm
                       {
                           UserId = a.User_Id,
                           UserName = a.User_Name,
                           UserRoleId = b.user_role_name,
                           UserStatus = c.status_code,
                           UserBizRoleId = bizzone.biz_zone_category_name
                       };

            return View(data.ToList());
        }

        // GET: user/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_info userInfo = _db.user_info.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }

            var userRole = _db.user_role.SingleOrDefault(x => x.user_role_id==userInfo.User_role_id);
            var userBizRoleId = _db.tbld_business_zone_hierarchy.SingleOrDefault(x => x.id==userInfo.User_biz_role_id);
            if (userRole != null)
            {
                if (userBizRoleId != null)
                {
                    var userStatus = _db.status.SingleOrDefault(x => x.status_Id == userInfo.User_status);
                    if (userStatus != null)
                    {
                        UseriVm useriVm = new UseriVm 
                        {
                            UserId = userInfo.User_Id,
                            UserName = userInfo.User_Name,
                            UserPassword = userInfo.User_Password,
                            UserRoleId = userRole.user_role_name,
                            UserBizRoleId = userBizRoleId.biz_zone_category_name,
                            UserStatus = userStatus.status_code

                        };
                        return View(useriVm);
                    }
                }
            }
            return HttpNotFound();
        }

        // GET: user/Create
        public ActionResult Create()
        {
            ViewBag.User_role = new SelectList(_db.user_role.ToList(), "user_role_id", "user_role_name");
            ViewBag.User_status = new SelectList(_db.status.ToList(), "status_Id", "status_code");
            ViewBag.biz_role = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "id", "biz_zone_category_name");
            return View();
        }

        // POST: user/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserVm userVm)
        {

            var data = _db.user_info.Where(x => x.User_Name == userVm.UserName).ToList();

            if (ModelState.IsValid)
            {
                if (!data.Any())
                {
                    user_info userInfo = new user_info
                    {
                        User_Name = userVm.UserName,
                        User_Password = userVm.UserPassword,
                        User_role_id = userVm.UserRoleId,
                        User_status = userVm.UserStatus,
                        User_biz_role_id = userVm.UserBizRoleId
                    };
                    _db.user_info.Add(userInfo);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.User_role = new SelectList(_db.user_role.ToList(), "user_role_id", "user_role_name", userVm.UserRoleId);
                ViewBag.User_status = new SelectList(_db.status.ToList(), "status_Id", "status_code", userVm.UserStatus);
                ViewBag.biz_role = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "id", "biz_zone_category_name", userVm.UserBizRoleId);

                TempData["alertbox"] = "error";
                TempData["alertboxMsg"] = userVm.UserName + " User Name Already Use";
                return View(userVm);
            }

            ViewBag.User_role = new SelectList(_db.user_role.ToList(), "user_role_id", "user_role_name", userVm.UserRoleId);
            ViewBag.User_status = new SelectList(_db.status.ToList(), "status_Id", "status_code", userVm.UserStatus);
            ViewBag.biz_role = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "id", "biz_zone_category_name", userVm.UserBizRoleId);
            return View(userVm);
        }

        // GET: user/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_info userIcnfo = _db.user_info.Find(id);
            if (userIcnfo == null)
            {
                return HttpNotFound();
            }
            
            if (userIcnfo.User_biz_role_id != null)
            {
                if (userIcnfo.User_status != null)
                {
                    UserVm userVm = new UserVm
                    {
                        UserId = userIcnfo.User_Id,
                        UserName = userIcnfo.User_Name,
                        UserPassword = userIcnfo.User_Password,
                        UserRoleId = userIcnfo.User_role_id,
                        UserBizRoleId = (int) userIcnfo.User_biz_role_id,
                        UserStatus = (int) userIcnfo.User_status

                    };

                    ViewBag.User_role = new SelectList(_db.user_role.ToList(), "user_role_id", "user_role_name", userVm.UserRoleId);
                    ViewBag.User_status = new SelectList(_db.status.ToList(), "status_Id", "status_code", userVm.UserStatus);
                    ViewBag.biz_role = new SelectList(_db.tbld_business_zone_hierarchy.ToList(), "id", "biz_zone_category_name", userVm.UserBizRoleId);
                    return View(userVm);
                }
            }
            return HttpNotFound();
        }

        // POST: user/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserVm userVm)
        {
            if (ModelState.IsValid)
            {
                user_info userInfo = new user_info
                {
                    User_Id =userVm.UserId,
                    User_Name = userVm.UserName,
                    User_Password = userVm.UserPassword,
                    User_role_id = userVm.UserRoleId,
                    User_status = userVm.UserStatus,
                    User_biz_role_id = userVm.UserBizRoleId
                };
               
                _db.Entry(userInfo).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userVm);
        }

        // GET: user/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_info userInfo = _db.user_info.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // POST: user/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            user_info userInfo = _db.user_info.Find(id);
            if (userInfo != null) _db.user_info.Remove(userInfo);
            _db.SaveChanges();
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
