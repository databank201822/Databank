using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ODMS.Models;
using ODMS.Models.ViewModel;



namespace ODMS.Controllers
{
    [SessionExpire]
    public class RouteplanController : Controller
    {
        private ODMSEntities DB = new ODMSEntities();
        // GET: Routeplan
        public ActionResult Index()
        {
            var data = from a in DB.tbld_Route_Plan
                join b in DB.tbld_distribution_house on a.db_id equals b.DB_Id into dBhouse
                 from dBhouseinfo in dBhouse.DefaultIfEmpty()
                       join c in DB.tbld_distribution_employee on a.db_emp_id equals c.id into dbEmpId
                       from dbEmpinfo in dbEmpId.DefaultIfEmpty()
                       
                       select new RoutePlaniVm
                       {
                           id=a.id,
                           RoutePlanName = a.route_plan_name,
                           RoutePlanCode = a.route_plan_code,
                           RoutePlanDescription = a.route_plan_description,
                         Db =dBhouseinfo.DBName,
                           DbEmp = dbEmpinfo.Name,
                           StartDate = a.start_date,
                           EndDate = a.end_date,
                           ModifyDate = a.Modify_date
                       };

            return View(data.ToList());
        }

        [EditAccess]
        public ActionResult MakeRoutePlan()
        {
            ViewBag.subroute = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.DB = new SelectList(DB.tbld_distribution_house.Where(x => x.Status == 1).OrderBy(x => x.Zone_id).ToList(), "DB_Id", "DBName");
            ViewBag.DBemp = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeRoutePlan(RoutePlanMVm routePlanMVm)
        {
            if (ModelState.IsValid)
            {
                DateTime startDate = routePlanMVm.StartDate;

                //DateTime startDate = Convert.ToDateTime("2018-07-14");
                DateTime endDate = routePlanMVm.EndDate;
                var dateDaff = (endDate - startDate).TotalDays;

                tbld_Route_Plan tbldRoutePlan = new tbld_Route_Plan
                {
                    route_plan_name = routePlanMVm.RoutePlanName,
                    route_plan_code = "RP-" + DateTime.Now.ToString("yyMMddHHmmss"),
                    route_plan_description = routePlanMVm.RoutePlanDescription,
                    db_id = routePlanMVm.DbId,
                    db_emp_id = routePlanMVm.DbEmpId,
                    creation_date = DateTime.Today,
                    start_date = routePlanMVm.StartDate,
                    end_date = routePlanMVm.EndDate,
                    Modify_date = DateTime.Today

                };

                DB.tbld_Route_Plan.Add(tbldRoutePlan);
                DB.SaveChanges();

                int routePlanid = tbldRoutePlan.id;

                //add datils routeplan mapping
                if (routePlanMVm.SatRoutes != null)
                {
                    foreach (var route in routePlanMVm.SatRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route,"Saturday");
                    }

                }
                if (routePlanMVm.SunRoutes != null)
                {
                    foreach (var route in routePlanMVm.SunRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Sunday");
                    }
                }
                if (routePlanMVm.MonRoutes != null)
                {
                    foreach (var route in routePlanMVm.MonRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Monday");
                    }
                }
                if (routePlanMVm.TueRoutes != null)
                {
                    foreach (var route in routePlanMVm.TueRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Tuesday");
                    }
                }
                if (routePlanMVm.WedRoutes != null)
                {
                    foreach (var route in routePlanMVm.WedRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route,"Wednesday");
                    }
                }
                if (routePlanMVm.ThuRoutes != null)
                {
                    foreach (var route in routePlanMVm.ThuRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route,"Thursday");
                    }
                }
                if (routePlanMVm.FriRoutes != null)
                {
                    foreach (var route in routePlanMVm.FriRoutes)
                    {
                        AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Friday");
                    }
                }
                //add datils routeplan mapping



                
                //add  routeplan datils

                for (var days = 0; days <= dateDaff; days++)
                {
                    var dayOfWeek = startDate.AddDays(days).DayOfWeek.ToString();

                    switch (dayOfWeek)
                    {
                        case "Saturday":
                            if (routePlanMVm.SatRoutes != null)
                            {
                                foreach (var route in routePlanMVm.SatRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 1),
                                        DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                }
                            }

                            break;
                        case "Sunday":
                            if (routePlanMVm.SunRoutes != null)
                            {
                                foreach (var route in routePlanMVm.SunRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 1),0);
                                }
                            }
                            break;
                        case "Monday":
                            if (routePlanMVm.MonRoutes != null)
                            {
                                foreach (var route in routePlanMVm.MonRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 1),0);
                                }
                            }
                            break;
                        case "Tuesday":
                            if (routePlanMVm.TueRoutes != null)
                            {
                                foreach (var route in routePlanMVm.TueRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 1),0);
                                }
                            }
                            break;
                        case "Wednesday":
                            if (routePlanMVm.WedRoutes != null)
                            {
                                foreach (var route in routePlanMVm.WedRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 1),0);
                                }
                            }
                            break;
                        case "Thursday":
                            if (routePlanMVm.ThuRoutes != null)
                            {
                                foreach (var route in routePlanMVm.ThuRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 2),0);
                                }
                            }
                            break;
                        case "Friday":
                            if (routePlanMVm.FriRoutes != null)
                            {
                                foreach (var route in routePlanMVm.FriRoutes)
                                {
                                    AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                        startDate.AddDays(days), startDate.AddDays(days + 1),0);
                                }
                            }
                            break;
                    }
                }
            
            //add  routeplan datils
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] ="Route Plan Create Successfully";
                return RedirectToAction("Index");
            }

            ViewBag.subroute = new SelectList(DB.tbld_distributor_Route.Where(x => x.DistributorID == routePlanMVm.DbId && x.IsActive == 1 && x.RouteType == 2).Select(x => new { x.RouteID, x.RouteName }).ToList(), "RouteID", "RouteName");
            ViewBag.DB = new SelectList(DB.tbld_distribution_house.Where(x => x.Status == 1).OrderBy(x => x.Zone_id).ToList(), "DB_Id", "DBName");
            ViewBag.DBemp = new SelectList(DB.tbld_distribution_employee.Where(x => x.DistributionId == routePlanMVm.DbId && x.Emp_Type == 2 && x.active == 1).Select(x => new { x.id, x.Name }).ToList(), "id", "Name");
            return View();
        }

    [EditAccess]
        public ActionResult EditRoutePlan(int id)
        {
            tbld_Route_Plan tbldRoutePlan = DB.tbld_Route_Plan.FirstOrDefault(x => x.id == id);

            if (tbldRoutePlan != null)
            {
                RoutePlanMVm routePlanMVm = new RoutePlanMVm
                {
                    id = id,
                    RoutePlanName = tbldRoutePlan.route_plan_name,
                    RoutePlanDescription = tbldRoutePlan.route_plan_description,
                    DbId = tbldRoutePlan.db_id,
                    DbEmpId = tbldRoutePlan.db_emp_id,
                    StartDate = tbldRoutePlan.start_date,
                    EndDate = tbldRoutePlan.end_date,
                    SatRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Saturday" && x.route_plan_id == id).Select(x => x.route_id).ToList(),
                    SunRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Sunday" && x.route_plan_id == id).Select(x => x.route_id).ToList(),
                    MonRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Monday" && x.route_plan_id == id).Select(x => x.route_id).ToList(),
                    TueRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Tuesday" && x.route_plan_id == id).Select(x => x.route_id).ToList(),
                    WedRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Wednesday" && x.route_plan_id == id).Select(x => x.route_id).ToList(),
                    ThuRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Thursday" && x.route_plan_id == id).Select(x => x.route_id).ToList(),
                    FriRoutes = DB.tbld_Route_Plan_Mapping.Where(x => x.day == "Friday" && x.route_plan_id == id).Select(x => x.route_id).ToList()
                };


                ViewBag.subroute = new SelectList(DB.tbld_distributor_Route.Where(x => x.DistributorID == routePlanMVm.DbId && x.IsActive == 1 && x.RouteType == 2).Select(x => new { x.RouteID, x.RouteName }).ToList(), "RouteID", "RouteName");
                ViewBag.DB = new SelectList(DB.tbld_distribution_house.Where(x => x.Status == 1).OrderBy(x => x.Zone_id).ToList(), "DB_Id", "DBName", tbldRoutePlan.db_id);
                ViewBag.DBemp = new SelectList(DB.tbld_distribution_employee.Where(x => x.DistributionId == routePlanMVm.DbId && x.Emp_Type == 2 && x.active == 1).Select(x => new { x.id, x.Name }).ToList(), "id", "Name", routePlanMVm.DbEmpId);


                return View(routePlanMVm);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoutePlan(RoutePlanMVm routePlanMVm)
        {
           

            if (ModelState.IsValid)
            {
               

                DateTime startDate = routePlanMVm.StartDate;
                DateTime endDate = routePlanMVm.EndDate;
                var dateDaff = (endDate - startDate).TotalDays;
                tbld_Route_Plan tbldRoutePlan = DB.tbld_Route_Plan.Find(routePlanMVm.id);

                if (tbldRoutePlan != null)
                {
                    tbldRoutePlan.route_plan_name = routePlanMVm.RoutePlanName;
                    tbldRoutePlan.route_plan_description = routePlanMVm.RoutePlanDescription;
                    tbldRoutePlan.db_id = routePlanMVm.DbId;
                    tbldRoutePlan.db_emp_id = routePlanMVm.DbEmpId;
                    tbldRoutePlan.end_date = routePlanMVm.EndDate;
                    tbldRoutePlan.Modify_date = DateTime.Today;
               
                    DB.Entry(tbldRoutePlan).State = EntityState.Modified;
                    DB.SaveChanges();

                    int routePlanid = tbldRoutePlan.id;

                    DB.tbld_Route_Plan_Mapping.RemoveRange(DB.tbld_Route_Plan_Mapping.Where(x => x.route_plan_id == routePlanid));
                    DB.SaveChanges();

                    //add datils routeplan mapping
                    if (routePlanMVm.SatRoutes != null)
                    {
                        foreach (var route in routePlanMVm.SatRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route,
                                "Saturday");
                        }

                    }
                    if (routePlanMVm.SunRoutes != null)
                    {
                        foreach (var route in routePlanMVm.SunRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Sunday");
                        }
                    }
                    if (routePlanMVm.MonRoutes != null)
                    {
                        foreach (var route in routePlanMVm.MonRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Monday");
                        }
                    }
                    if (routePlanMVm.TueRoutes != null)
                    {
                        foreach (var route in routePlanMVm.TueRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Tuesday");
                        }
                    }
                    if (routePlanMVm.WedRoutes != null)
                    {
                        foreach (var route in routePlanMVm.WedRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route,
                                "Wednesday");
                        }
                    }
                    if (routePlanMVm.ThuRoutes != null)
                    {
                        foreach (var route in routePlanMVm.ThuRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route,
                                "Thursday");
                        }
                    }
                    if (routePlanMVm.FriRoutes!=null)
                    {
                        foreach (var route in routePlanMVm.FriRoutes)
                        {
                            AddRoutePlanMapping(routePlanMVm.DbId, routePlanMVm.DbEmpId, routePlanid, route, "Friday");
                        }
                    }
                    //add datils routeplan mapping



                    DB.tbld_Route_Plan_Detail.RemoveRange(DB.tbld_Route_Plan_Detail.Where(x => x.route_plan_id == routePlanid && x.planned_visit_date >= routePlanMVm.StartDate));
                    DB.SaveChanges();

                    //add  routeplan datils

                    for (var days = 0; days <= dateDaff; days++)
                    {
                        var dayOfWeek = startDate.AddDays(days).DayOfWeek.ToString();

                        switch (dayOfWeek)
                        {
                            case "Saturday":
                                if (routePlanMVm.SatRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.SatRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 1),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }

                                break;
                            case "Sunday":
                                if (routePlanMVm.SunRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.SunRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 1),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }
                                break;
                            case "Monday":
                                if (routePlanMVm.MonRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.MonRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 1),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }
                                break;
                            case "Tuesday":
                                if (routePlanMVm.TueRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.TueRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 1),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }
                                break;
                            case "Wednesday":
                                if (routePlanMVm.WedRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.WedRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 1),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }
                                break;
                            case "Thursday":
                                if (routePlanMVm.ThuRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.ThuRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 2),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }
                                break;
                            case "Friday":
                                if (routePlanMVm.FriRoutes != null)
                                {
                                    foreach (var route in routePlanMVm.FriRoutes)
                                    {
                                        AddRoutePlanDetails(routePlanid, routePlanMVm.DbId, routePlanMVm.DbEmpId, route,
                                            startDate.AddDays(days), startDate.AddDays(days + 1),
                                            DB.tbld_Outlet.Count(x => x.IsActive == 1 && x.parentid == route));
                                    }
                                }
                                break;
                        }
                    }
                }
                //add  routeplan datils
                TempData["alertbox"] = "success";
                TempData["alertboxMsg"] = "Route Plan Edit Successfully";

                return RedirectToAction("Index");
            }
                ViewBag.subroute = new SelectList(DB.tbld_distributor_Route.Where(x => x.DistributorID == routePlanMVm.DbId && x.IsActive == 1 && x.RouteType == 2).Select(x => new { x.RouteID, x.RouteName }).ToList(), "RouteID", "RouteName");
                ViewBag.DB = new SelectList(DB.tbld_distribution_house.Where(x => x.Status == 1).OrderBy(x => x.Zone_id).ToList(), "DB_Id", "DBName",routePlanMVm.DbId);
                ViewBag.DBemp = new SelectList(DB.tbld_distribution_employee.Where(x => x.DistributionId == routePlanMVm.DbId && x.Emp_Type == 2 && x.active == 1).Select(x => new { x.id, x.Name }).ToList(), "id", "Name", routePlanMVm.DbEmpId);
           
           
            return View(routePlanMVm);
           
        }


        public void AddRoutePlanDetails(int routePlanId, int dbid, int dbEmpId, int routeId, DateTime plannedVisitDate, DateTime deliveryDate, int noOfOutlet)
        {
            tbld_Route_Plan_Detail tbldRoutePlanDetail = new tbld_Route_Plan_Detail
            {
                route_plan_id = routePlanId,
                dbid = dbid,
                db_emp_id = dbEmpId,
                route_id = routeId,
                planned_visit_date = plannedVisitDate,
                delivery_date = deliveryDate,
                no_of_outlet = noOfOutlet

            };
            DB.tbld_Route_Plan_Detail.Add(tbldRoutePlanDetail);
            DB.SaveChanges();
        }

        public void AddRoutePlanMapping(int dbid, int dbEmpId, int routePlanId, int routeId, string day)
        {
            tbld_Route_Plan_Mapping tbldRoutePlanMapping = new tbld_Route_Plan_Mapping
            {
               
                db_id = dbid,
                db_emp_id = dbEmpId,
                route_plan_id = routePlanId,
                route_id = routeId,
                day = day
              

            };
            DB.tbld_Route_Plan_Mapping.Add(tbldRoutePlanMapping);
            DB.SaveChanges();
        }


        public ActionResult GetPsrByDBid(int dbid)
        {
            HashSet<int> psrrouteplandone = new HashSet<int>(DB.tbld_Route_Plan_Mapping.Where(x => x.db_id == dbid).Select(x=>x.db_emp_id).Distinct());
            var psrlist = DB.tbld_distribution_employee.Where(x => x.DistributionId == dbid && x.Emp_Type == 2 && x.active == 1 && !psrrouteplandone.Contains(x.id)).Select(x => new { x.id, x.Name }).ToList();
            return Json(psrlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubroutebyDBid(int dbid)
        {

            var subrouteList = DB.tbld_distributor_Route
                .Where(x => x.DistributorID == dbid && x.IsActive == 1 && x.RouteType == 2)
                .Select(x => new { x.RouteID, x.RouteName }).ToList();
            return Json(subrouteList, JsonRequestBehavior.AllowGet);
        }

    }
}