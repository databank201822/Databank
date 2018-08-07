using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODMS.Models;

namespace ODMS.Controllers
{
    public class Supporting
    {
        private readonly ODMSEntities _db = new ODMSEntities();
        // GET: Supporting
        private HashSet<int> GetdbidsbyNsm()
        {
            HashSet<int> dbids = new HashSet<int>(_db.tbld_db_zone_view.Where(x => x.Status == 1).Select(x => x.DB_Id));
            return dbids;
        }

        private HashSet<int> GetdbidsbyRsm(int[] rsMid)
        {
            HashSet<int> dbids = new HashSet<int>(_db.tbld_db_zone_view.Where(x => rsMid.Contains(x.REGION_id) && x.Status == 1).Select(x => x.DB_Id));

            return dbids;

        }

        private HashSet<int> GetdbidsbyTdm(int[] asMid)
        {
            HashSet<int> dbids = new HashSet<int>(_db.tbld_db_zone_view.Where(x => asMid.Contains(x.AREA_id) && x.Status == 1).Select(x => x.DB_Id));

            return dbids;
        }

        private HashSet<int> GetdbidsbyCe(int[] cEid)
        {
            HashSet<int> dbids = new HashSet<int>(_db.tbld_db_zone_view.Where(x => cEid.Contains(x.CEAREA_id) && x.Status == 1).Select(x => x.DB_Id));

            return dbids;

        }

        public HashSet<int> Alldbids(int[] rsMid, int[] asMid, int[] cEid, int[] id)
        {
            HashSet<int> dbids;
           

            Supporting sp = new Supporting();

            if (id != null)
            {

                dbids = new HashSet<int>(id);

            }
            else if (cEid != null)
            {


                dbids = sp.GetdbidsbyCe(cEid);

            }
            else if (asMid != null)
            {

                dbids = sp.GetdbidsbyTdm(asMid);

            }
            else if (rsMid != null)
            {

                dbids = sp.GetdbidsbyRsm(rsMid);

            }
            else
            {

                dbids = sp.GetdbidsbyNsm();
            }
            return dbids;
       
       }


        public string Remove_Special_Characters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('_');
                }
            }
            return sb.ToString();
        }
    }
}