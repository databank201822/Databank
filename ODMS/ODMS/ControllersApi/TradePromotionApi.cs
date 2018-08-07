using System.Web.Http;
using ODMS.Models;

namespace ODMS.ControllersApi
{
    public class TradePromotionApiController : ApiController
    {

        private readonly ODMSEntitiesApi _dbapi = new ODMSEntitiesApi();
        [HttpPost]
        public IHttpActionResult GetTradePromotion(int dbid)
        {
            var tradePromotionList = _dbapi.ApiGetTradePromotion(dbid);
            return Ok(tradePromotionList);
        }


        [HttpPost]
        public IHttpActionResult GetTradePromotionDefinition(int dbid)
        {
            var tradePromotionList = _dbapi.ApiGetTradePromotionDefinition(dbid);
            return Ok(tradePromotionList);
        }

    }
}
