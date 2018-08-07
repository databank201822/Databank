using System.Web.Mvc;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class ErrorReportingController : Controller
    {
        // GET: ErrorReporting
        public ActionResult EditPermission()
        {
            return View();
        }
        public ActionResult DeletePermission()
        {
            return View();
        }
    }
}