using System.Web.Http;
using ODMS.Models;

namespace ODMS.ControllersApi
{
    public class SkuApiController : ApiController
    {

            private readonly ODMSEntitiesApi _dbapi = new ODMSEntitiesApi();
            [HttpPost]
            public IHttpActionResult GetSku(int dbid)
            {
                var skuList = _dbapi.ApiGetSku(dbid);
                return Ok(skuList);
            }
        
    }
}
