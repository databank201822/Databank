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
    public class MenuSubMenuController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

        
        // GET: MenuSubMenu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_SubMenu tbl_SubMenu = db.tbl_SubMenu.Find(id);
            if (tbl_SubMenu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SubMenu);
        }

        // GET: MenuSubMenu/Create
        public ActionResult Create()
        {
            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName");
            return View();
        }

        // POST: MenuSubMenu/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubMenuName,Controller,Action,MainMenuId")] tbl_SubMenu tbl_SubMenu)
        {
            if (ModelState.IsValid)
            {
                var max = db.tbl_SubMenu.Select(x => x.sl).Max();
                if (max != null)
                {
                    int tblSubMenu = (int) max;
                    tbl_SubMenu.sl = tblSubMenu+ 1;
                }

                db.tbl_SubMenu.Add(tbl_SubMenu);
                db.SaveChanges();
                return RedirectToAction("Manulist","Menu");
            }

            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName", tbl_SubMenu.MainMenuId);
            return View(tbl_SubMenu);
        }

        // GET: MenuSubMenu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_SubMenu tbl_SubMenu = db.tbl_SubMenu.Find(id);
            if (tbl_SubMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName", tbl_SubMenu.MainMenuId);
            return View(tbl_SubMenu);
        }

        // POST: MenuSubMenu/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubMenuName,Controller,Action,MainMenuId")] tbl_SubMenu tbl_SubMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_SubMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manulist", "Menu");
            }
            ViewBag.MainMenuId = new SelectList(db.tbl_MainMenu, "Id", "MainMenuName", tbl_SubMenu.MainMenuId);
            return View(tbl_SubMenu);
        }

        // GET: MenuSubMenu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_SubMenu tbl_SubMenu = db.tbl_SubMenu.Find(id);
            if (tbl_SubMenu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SubMenu);
        }

        // POST: MenuSubMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_SubMenu tbl_SubMenu = db.tbl_SubMenu.Find(id);
            db.tbl_SubMenu.Remove(tbl_SubMenu);
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
