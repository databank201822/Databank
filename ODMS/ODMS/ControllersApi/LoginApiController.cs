using System;
using System.Linq;
using System.Web.Http;
using ODMS.Models;

namespace ODMS.ControllersApi
{
    public class LoginApiController : ApiController
    {
        private readonly ODMSEntitiesApi _dbapi = new ODMSEntitiesApi();

        

        [HttpPost]
        public IHttpActionResult Checkuser(string user, string pass, string lat, string lon)
        {
            var userinfo = _dbapi.ApiUserLogin(user, pass).SingleOrDefault();

            DateTime todayDate = DateTime.Today;

            var priviouslogin =_dbapi.tblm_UserLogin.Where(x => x.PSR_id == userinfo.PSRid && x.Date == todayDate);


            if (userinfo != null)
            {
                if (priviouslogin.FirstOrDefault() == null)
                {
                
                tblm_UserLogin tblmUserLogin = new tblm_UserLogin
                    {
                        Date = todayDate,
                        PSR_id = userinfo.PSRid,
                        Date_time_stamp = DateTime.Now,
                        current_lat = lat,
                        current_lon = lon
                    };
                    _dbapi.tblm_UserLogin.Add(tblmUserLogin);
                    _dbapi.SaveChanges();
                   
                }

                return Ok(userinfo);
            }
            return Ok("sorry");
        }

    }
}
