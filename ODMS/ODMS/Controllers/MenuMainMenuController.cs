using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class MenuMainMenuController : Controller
    {
        private ODMSEntities db = new ODMSEntities();

      
        // GET: MainMenu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MainMenu tbl_MainMenu = db.tbl_MainMenu.Find(id);
            if (tbl_MainMenu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MainMenu);
        }

        // GET: MainMenu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MainMenu/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_MainMenu tblMainMenu)
        {
            if (ModelState.IsValid)
            {
                var max = db.tbl_MainMenu.Select(x => x.sl).Max();
                if (max != null)
                {
                    int lastOrDefault = (int) max;

                    tblMainMenu.sl = lastOrDefault + 1;
                }
                db.tbl_MainMenu.Add(tblMainMenu);
                db.SaveChanges();
                return RedirectToAction("Manulist","Menu");
            }

            return View(tblMainMenu);
        }

        // GET: MainMenu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MainMenu tblMainMenu = db.tbl_MainMenu.Find(id);
            if (tblMainMenu == null)
            {
                return HttpNotFound();
            }
            return View(tblMainMenu);
        }

        // POST: MainMenu/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MainMenuName,Controller,Action")] tbl_MainMenu tbl_MainMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_MainMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manulist", "Menu");
            }
            return View(tbl_MainMenu);
        }

        // GET: MainMenu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_MainMenu tbl_MainMenu = db.tbl_MainMenu.Find(id);
            if (tbl_MainMenu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MainMenu);
        }

        // POST: MainMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_MainMenu tbl_MainMenu = db.tbl_MainMenu.Find(id);
            db.tbl_MainMenu.Remove(tbl_MainMenu);
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
