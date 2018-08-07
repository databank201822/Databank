using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class UserRoleController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: user_role
        public ActionResult Index()
        {
            return View(db.user_role.ToList());
        }

        // GET: user_role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_role user_role = db.user_role.Find(id);
            if (user_role == null)
            {
                return HttpNotFound();
            }
            return View(user_role);
        }

        // GET: user_role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: user_role/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(user_role user_role)
        {
            if (ModelState.IsValid)
            {
                db.user_role.Add(user_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_role);
        }

        // GET: user_role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_role user_role = db.user_role.Find(id);
            if (user_role == null)
            {
                return HttpNotFound();
            }
            return View(user_role);
        }

        // POST: user_role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_role_id,user_role_name,user_role_code,user_role_status,isOnlineLogin,isReportView,isCreate,isEdit,isDelete")] user_role user_role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_role);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user_role user_role = db.user_role.Find(id);
            if (user_role == null)
            {
                return HttpNotFound();
            }
            return View(user_role);
        }

        // POST: user_role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user_role user_role = db.user_role.Find(id);
            db.user_role.Remove(user_role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
