using System;
using System.Linq;
using System.Web.Http;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.ControllersApi
{
    public class SubRouteApiController : ApiController
    {
        private readonly ODMSEntitiesApi _dbapi = new ODMSEntitiesApi();
        [HttpPost]
        public IHttpActionResult GetSubRoute(int psrid)
        {
            DateTime currentdate=DateTime.Today;

            var subRoute = _dbapi.ApiGetSubRoute(psrid, currentdate).Select(x=>new SubRouteApiVm
            {
                Subrouteid = x.route_id,
                SubrouteName = x.RouteName,
                Todayvisit = x.planned_visit_date==null?0:1
            });


            return Ok(subRoute);
        }
    }
}
