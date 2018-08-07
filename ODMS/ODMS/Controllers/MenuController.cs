using System;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{

    [SessionExpire]
    public class MenuController : Controller
    {
        public ODMSEntities Db = new ODMSEntities();
        // GET: Menu
        public ActionResult ShowMainMenu()
        {
            
            var userRole = Convert.ToInt32(Session["User_role_id"]);

            var mainManuid = Db.tbl_Manu_Role_mapping.Where(s => s.Roleid == userRole).Select(x => x.MainMenuid).ToList();
            var submenuListid = Db.tbl_Manu_Role_mapping.Where(s => s.Roleid == userRole).Select(x => x.SubMenuid).ToList();
            var submenuSecondListid = Db.tbl_Manu_Role_mapping.Where(s => s.Roleid == userRole).Select(x => x.SubMenuSecondid).ToList();

            MenuiVm menuiVm = new MenuiVm()
            {
                MainMenus = Db.tbl_MainMenu.Where(s => mainManuid.Contains(s.Id)).OrderBy(x=>x.sl).ToList(),
                SubMenu = Db.tbl_SubMenu.Where(s => submenuListid.Contains(s.Id)).OrderBy(x=>x.sl).ToList(),
                SubMenuSecond = Db.tbl_SubMenuSecond.Where(s => submenuSecondListid.Contains(s.Id)).OrderBy(x=>x.sl).ToList()
            };
            return PartialView(menuiVm);
        }

        public ActionResult InsertRoleMenu(int? id)
        {
            user_role userRole = Db.user_role.Find(id);

            if (userRole != null)
            {
                ViewBag.user_role_name = userRole.user_role_name;
                ViewBag.user_role_id = userRole.user_role_id;
            }


            ViewBag.MainMenuids = Db.tbl_Manu_Role_mapping.Where(x => x.Roleid == id).Select(a => a.MainMenuid).ToList();
            ViewBag.SubMenuids = Db.tbl_Manu_Role_mapping.Where(x => x.Roleid == id).Select(a => a.SubMenuid).ToList();
            ViewBag.SubMenuSecondids = Db.tbl_Manu_Role_mapping.Where(x => x.Roleid == id).Select(a => a.SubMenuSecondid).ToList();


            MenuiVm menuiVm = new MenuiVm()
            {
                MainMenus = Db.tbl_MainMenu.OrderBy(x=>x.sl).ToList(),
                SubMenu = Db.tbl_SubMenu.OrderBy(x => x.sl).ToList(),
                SubMenuSecond = Db.tbl_SubMenuSecond.OrderBy(x => x.sl).ToList()
            };


            return View(menuiVm);
        }

        [HttpPost]
        public ActionResult UpdateRoleMenu(int userRoleId, int[] mainManu, int[] subMainManu, int[] submenuSecond)
        {
            var data = Db.tbl_Manu_Role_mapping.Where(x => x.Roleid == userRoleId).ToList();
            foreach (var i in data)
            {
                tbl_Manu_Role_mapping tblManuRoleMapping = Db.tbl_Manu_Role_mapping.Find(i.id);
                if (tblManuRoleMapping != null) Db.tbl_Manu_Role_mapping.Remove(tblManuRoleMapping);
            }
            Db.SaveChanges();

            if (mainManu != null)
            {
                foreach (var main in mainManu)
                {
                    tbl_Manu_Role_mapping tblManuRoleMapping = new tbl_Manu_Role_mapping()
                    {

                        Roleid = userRoleId,
                        MainMenuid = main,
                        SubMenuid = 0,
                        SubMenuSecondid = 0
                    };
                    Db.tbl_Manu_Role_mapping.Add(tblManuRoleMapping);
                }
                Db.SaveChanges();
            }
            if (subMainManu != null)
            {
                foreach (var subMainManuitem in subMainManu)
                {
                    var tblSubMenu = Db.tbl_SubMenu.Find(subMainManuitem);
                    if (tblSubMenu != null)
                    {
                        int parentId = tblSubMenu.MainMenuId ?? 0;

                        tbl_Manu_Role_mapping tblManuRoleMapping = new tbl_Manu_Role_mapping()
                        {

                            Roleid = userRoleId,
                            MainMenuid = parentId,
                            SubMenuid = subMainManuitem,
                            SubMenuSecondid = 0
                        };
                        Db.tbl_Manu_Role_mapping.Add(tblManuRoleMapping);
                    }

                }
                Db.SaveChanges();
            }

            if (submenuSecond != null)
            {
                foreach (var submenuSeconditem in submenuSecond)
                {
                    var tblSubMenuSecond = Db.tbl_SubMenuSecond.Find(submenuSeconditem);
                    if (tblSubMenuSecond != null)
                    {
                        var mainMenuId = tblSubMenuSecond.MainMenuId ?? 0;
                        var subMenuId = tblSubMenuSecond.SubMenuId ?? 0;

                        tbl_Manu_Role_mapping tblManuRoleMapping = new tbl_Manu_Role_mapping()
                        {
                            Roleid = userRoleId,
                            MainMenuid = mainMenuId,
                            SubMenuid = subMenuId,
                            SubMenuSecondid = submenuSeconditem
                        };
                        Db.tbl_Manu_Role_mapping.Add(tblManuRoleMapping);
                    }

                }
                Db.SaveChanges();
            }
            return Json(userRoleId, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Manulist()
        {
            MenuiVm menuiVm = new MenuiVm()
            {
                MainMenus = Db.tbl_MainMenu.ToList(),
                SubMenu = Db.tbl_SubMenu.ToList(),
                SubMenuSecond = Db.tbl_SubMenuSecond.ToList()
            };

            return View(menuiVm);
        }
    }
}