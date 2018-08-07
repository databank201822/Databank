using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models.ViewModel
{
    public class DBhouseRouteiVm
    {
        public int RouteId { get; set; }

        [Display(Name = "Route Code")]
        [Required(ErrorMessage = "Enter Route Code")]
        public string RouteCode { get; set; }

        [Display(Name = "Route Name")]
        [Required(ErrorMessage = "Enter Route Name")]
        public string RouteName { get; set; }

        [Display(Name = "Route Type")]
        [Required(ErrorMessage = "Select Route Type")]
        public string RouteType { get; set; }

        [Display(Name = "Route Create date")]
        [Column(TypeName = "datetime2")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Route Modified Date")]
        [Column(TypeName = "datetime2")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Route Status")]
        [Required(ErrorMessage = "Select Route status")]
        public string IsActive { get; set; }

        [Display(Name = "Distributor Name")]
      
        public string Distributor { get; set; }

        [Display(Name = "Route Parent")]
        public string Parent{ get; set; }
    }
    public class DBhouseRouteVm
    {
        public int RouteId { get; set; }

        [Display(Name = "Route Code")]
        [Required(ErrorMessage="Enter Route Code")]
        public string RouteCode { get; set; }

        [Display(Name = "Route Name")]
        [Required(ErrorMessage = "Enter Route Name")]
        public string RouteName { get; set; }

        [Display(Name = "Route Type")]
        [Required(ErrorMessage = "Select Route Type")]
        public int RouteType { get; set; }

        [Display(Name = "Route Create date")]
        [Column(TypeName = "datetime2")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Route Modified Date")]
        [Column(TypeName = "datetime2")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Route Status")]
        [Required(ErrorMessage = "Select Route status")]
        public int IsActive { get; set; }

        [Display(Name = "Distributor Name")]
        [Required(ErrorMessage = "Select A Distributor")]
        public int DistributorId { get; set; }

        [Display(Name = "Route Parent")]
        public int? ParentId { get; set; }
    }
}