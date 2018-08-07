using System.Web.Http;
using ODMS.Models;

namespace ODMS.ControllersApi
{
    public class OutletApiController : ApiController
    {
        private readonly ODMSEntitiesApi _dbapi = new ODMSEntitiesApi();
        [HttpPost]
        public IHttpActionResult GetOutlet(int psrid)
        {
            var outletList = _dbapi.ApiGetOutlet(psrid);

            return Ok(outletList);
        }
    }
}
