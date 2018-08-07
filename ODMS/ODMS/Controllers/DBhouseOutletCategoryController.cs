using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class DBhouseOutletCategoryController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: DBhouseOutletCategory
        public ActionResult Index()
        {
            return View(db.tbld_Outlet_category.ToList());
        }

        // GET: DBhouseOutletCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_category tbld_Outlet_category = db.tbld_Outlet_category.Find(id);
            if (tbld_Outlet_category == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_category);
        }

        // GET: DBhouseOutletCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DBhouseOutletCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,outlet_category_name,outlet_category_code,outlet_category_description")] tbld_Outlet_category tbld_Outlet_category)
        {
            if (ModelState.IsValid)
            {
                db.tbld_Outlet_category.Add(tbld_Outlet_category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbld_Outlet_category);
        }

        // GET: DBhouseOutletCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_category tbld_Outlet_category = db.tbld_Outlet_category.Find(id);
            if (tbld_Outlet_category == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_category);
        }

        // POST: DBhouseOutletCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,outlet_category_name,outlet_category_code,outlet_category_description")] tbld_Outlet_category tbld_Outlet_category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbld_Outlet_category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbld_Outlet_category);
        }

        // GET: DBhouseOutletCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbld_Outlet_category tbld_Outlet_category = db.tbld_Outlet_category.Find(id);
            if (tbld_Outlet_category == null)
            {
                return HttpNotFound();
            }
            return View(tbld_Outlet_category);
        }

        // POST: DBhouseOutletCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbld_Outlet_category tbld_Outlet_category = db.tbld_Outlet_category.Find(id);
            db.tbld_Outlet_category.Remove(tbld_Outlet_category);
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
