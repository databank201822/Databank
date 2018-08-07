using System.Web.Http;


namespace ODMS.ControllersApi
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Connected");
        }
    }
}
