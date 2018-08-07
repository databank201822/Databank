using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // check  sessions here
            if (HttpContext.Current.Session["user_role_code"] == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
    public class EditAccessAttribute : ActionFilterAttribute
    {
        ODMSEntities _db = new ODMSEntities();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {          

            HttpContext ctx = HttpContext.Current;
            int userRoleId=Convert.ToInt32(ctx.Session["User_role_id"]);

            var edit = _db.user_role.Where(x => x.user_role_id == userRoleId && x.isEdit==1).ToList();

            // check  sessions here
            if (edit.Count() == 1)
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            filterContext.Result = new RedirectResult("/ErrorReporting/EditPermission");
        }
    }
    public class DeleteAccessAttribute : ActionFilterAttribute
    {
        public ODMSEntities Db = new ODMSEntities();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            int userRoleId = Convert.ToInt32(ctx.Session["User_role_id"]);
            var delete = Db.user_role.Where(x => x.user_role_id == userRoleId && x.isDelete == 1).ToList();

            // check  sessions here
            if (delete.Count() == 1)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult("/ErrorReporting/EditPermission");
        }


    }
    public class CreateAccessAttribute : ActionFilterAttribute
    {
        ODMSEntities _db = new ODMSEntities();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            int userRoleId = Convert.ToInt32(ctx.Session["User_role_id"]);
            var create = _db.user_role.Where(x => x.user_role_id == userRoleId && x.isCreate == 1).ToList();

            // check  sessions here
            if (create.Count() == 1)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult("/ErrorReporting/EditPermission");
        }


    }
    public class SuperAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            //int User_role_id = 1;
            //var Edit = db.user_role.Where(x => x.user_role_id == User_role_id).ToList();

            // check  sessions here
            if (Convert.ToInt32(ctx.Session["User_role_id"]) == 1)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult("/ErrorReporting/EditPermission");
        }


    }


    public class DBAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            //int User_role_id = 1;
            //var Edit = db.user_role.Where(x => x.user_role_id == User_role_id).ToList();

            // check  sessions here
            if (Convert.ToInt32(ctx.Session["User_role_id"]) == 7)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult("/ErrorReporting/EditPermission");
        }


    }

    public class AccessLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;
            var value = descriptor.GetParameters();

                base.OnActionExecuting(filterContext);
                return;
           
        }


    }




}