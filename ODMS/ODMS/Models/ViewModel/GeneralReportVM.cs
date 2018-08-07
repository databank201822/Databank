using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODMS.Models.ViewModel
{
    public class CurrentRouteplanVm
    {

        public string Region { get; set; }
    public string Area { get; set; }
    public string CeArea { get; set; }
    public string DbHouse { get; set; }
    public string PsrName { get; set; }
    public string SubRouteCode { get; set; }
    public string SubRouteName { get; set; }
    public int NumberOfOutlet { get; set; }
    public string Saturday { get; set; }
    public string Sunday { get; set; }
    public string Monday { get; set; }
    public string Tuesday { get; set; }
    public string Wednesday { get; set; }
    public string Thursday { get; set; }
    public string Friday { get; set; }
    }


    public class CurrentVisitplanVm
    {
        public int PsrId { get; set; }
        public DateTime Date { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string CeArea { get; set; }
        public string DbHouse { get; set; }
        public string PsrName { get; set; }
        public string SubRouteCode { get; set; }
        public string SubRouteName { get; set; }
        public int NumberOfOutlet { get; set; }
        public int NumberOforderedoutlet { get; set; }
        public int NumberOfnotorderedoutlet { get; set; }
    }
}
