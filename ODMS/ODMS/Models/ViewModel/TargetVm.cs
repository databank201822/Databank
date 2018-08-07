using System;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    public class TargetVm
    {
            public int Id { get; set; }

        [Display (Name = "Target Name")]
            public string Name { get; set; }
        [Display(Name = "Start Date")]
       
            public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
       
            public DateTime EndDate { get; set; }
       
    }

    public  class TargetDetailsVm
    {
        public int TargetId { get; set; }
        public string TargetName { get; set; }
        public string RegionName { get; set; }
        public string AreaName { get; set; }
        public string CeAreaName { get; set; }
        public int DbId { get; set; }
        public string DbName { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public int PackSize { get; set; }
        public double QtyinCs { get; set; }
        public double QtyinPs { get; set; }
        public double TotalQty { get; set; }
    }

    public class TargetPsrDetailsVm
    {
        public int TargetId { get; set; }
        public string TargetName { get; set; }
        public string RegionName { get; set; }
        public string AreaName { get; set; }
        public string CeAreaName { get; set; }
        public int DbId { get; set; }
        public string DbName { get; set; }
        public int PsrId { get; set; }
        public string PsrName { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public int PackSize { get; set; }
        public double QtyinCs { get; set; }
        public double QtyinPs { get; set; }
        public double TotalQty { get; set; }
    }

    public class PsrTargetBreak
    {
        public int Tgtid { get; set; }
        public int PsrId { get; set; }
        public string PsrCode { get; set; }
        public string PsrName { get; set; }

        public Double Contribution { get; set; }


        public int Skuid { get; set; }
     
        public int Qty { get; set; }

    }
}