using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ODMS.Models.ViewModel
{
    public class RoutePlaniVm
    {
        public int id { get; set; }
        public string RoutePlanName { get; set; }
        public string RoutePlanCode { get; set; }
        [DisplayName("DB House Name")]
        public string RoutePlanDescription { get; set; }
        public string Db { get; set; }
        public string DbEmp { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime CreationDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ModifyDate { get; set; }
        public string CreatedBy { get; set; }
    }
    public class RoutePlanMVm
    {
        public int id { get; set; }
        public string RoutePlanName { get; set; }
        public string RoutePlanCode { get; set; }
        public string RoutePlanDescription { get; set; }
        public int DbId { get; set; }
        public int DbEmpId { get; set; }
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ModifyDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public string CreatedBy { get; set; }
        public List<int> SatRoutes { get; set; }
        public List<int> SunRoutes { get; set; }
        public List<int> MonRoutes { get; set; }
        public List<int> TueRoutes { get; set; }
        public List<int> WedRoutes { get; set; }
        public List<int> ThuRoutes { get; set; }
        public List<int> FriRoutes { get; set; }

    }
    public class RoutePlanDetailVm
    {
        public int id { get; set; }
        public int RoutePlanId { get; set; }
        public int RouteId { get; set; }
        public int Dbid { get; set; }
        public int DbEmpId { get; set; }
        public System.DateTime PlannedVisitDate { get; set; }
        public System.DateTime DeliveryDate { get; set; }


    }

    public class RoutePlanMappingVm
    {
        public int Id { get; set; }
        public int DbId { get; set; }
        public int DbEmpId { get; set; }
        public int RoutePlanId { get; set; }
        public int RouteId { get; set; }
        public string Day { get; set; }
    }



}