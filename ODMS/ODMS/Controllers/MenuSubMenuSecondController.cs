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
    public class MenuSubMenuSecondController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        // GET: MenuSubMenuSecond
       

        // GET: MenuSubMenuSecond/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_SubMenuSecond tbl_SubMenuSecond = db.tbl_SubMenuSecond.Find(id);
            if (tbl_SubMenuSecond == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SubMenuSecond);
        }

        // GET: MenuSubMenuSecond/Create
        public ActionResult Create()
        {
            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName");
            ViewBag.SubMenuId = new SelectList(db.tbl_SubMenu, "Id", "SubMenuName");
            return View();
        }

        // POST: MenuSubMenuSecond/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubMenuName,Controller,Action,SubMenuId,MainMenuId")] tbl_SubMenuSecond tbl_SubMenuSecond)
        {
            if (ModelState.IsValid)
            {
                var max = db.tbl_SubMenuSecond.Select(x => x.sl).Max();
                if (max != null)
                {
                    int tblSubMenuSecond = (int) max;

                    tbl_SubMenuSecond.sl = tblSubMenuSecond + 1;
                }

                db.tbl_SubMenuSecond.Add(tbl_SubMenuSecond);
                db.SaveChanges();
                return RedirectToAction("Manulist","Menu");
            }

            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName", tbl_SubMenuSecond.MainMenuId);
            ViewBag.SubMenuId = new SelectList(db.tbl_SubMenu, "Id", "SubMenuName", tbl_SubMenuSecond.SubMenuId);
            return View(tbl_SubMenuSecond);
        }

        // GET: MenuSubMenuSecond/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_SubMenuSecond tbl_SubMenuSecond = db.tbl_SubMenuSecond.Find(id);
            if (tbl_SubMenuSecond == null)
            {
                return HttpNotFound();
            }
            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName", tbl_SubMenuSecond.MainMenuId);
            ViewBag.SubMenuId = new SelectList(db.tbl_SubMenu, "Id", "SubMenuName", tbl_SubMenuSecond.SubMenuId);
            return View(tbl_SubMenuSecond);
        }

        // POST: MenuSubMenuSecond/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubMenuName,Controller,Action,SubMenuId,MainMenuId")] tbl_SubMenuSecond tbl_SubMenuSecond)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_SubMenuSecond).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manulist", "Menu");
            }
            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName", tbl_SubMenuSecond.MainMenuId);
            ViewBag.SubMenuId = new SelectList(db.tbl_SubMenu, "Id", "SubMenuName", tbl_SubMenuSecond.SubMenuId);
            return View(tbl_SubMenuSecond);
        }

        // GET: MenuSubMenuSecond/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_SubMenuSecond tbl_SubMenuSecond = db.tbl_SubMenuSecond.Find(id);
            if (tbl_SubMenuSecond == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SubMenuSecond);
        }

        // POST: MenuSubMenuSecond/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_SubMenuSecond tbl_SubMenuSecond = db.tbl_SubMenuSecond.Find(id);
            db.tbl_SubMenuSecond.Remove(tbl_SubMenuSecond);
            db.SaveChanges();
            return RedirectToAction("Manulist", "Menu");
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
