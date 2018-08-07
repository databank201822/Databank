using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models.ViewModel
{
    public class DBhouseMv
    {

        public int DbId { get; set; }

        [DisplayName("Distributor Name")]
        [Required(ErrorMessage = "Enter DB House Name")]
        public string DbName { get; set; }

        [DisplayName("Distributor Code")]
        [Required(ErrorMessage = "Enter DB House Code")]
        public string DbCode { get; set; }

        [DisplayName("Distributor Description")]
        [Required(ErrorMessage = "Enter DB House Description")]
        public string DbDescription { get; set; }

        [DisplayName("Office Address")]
        [Required(ErrorMessage = "Enter DB House Office Address")]
        public string OfficeAddress { get; set; }

        [DisplayName("Warehouse Address")]
        [Required(ErrorMessage = "Enter DB House Warehouse Address")]
        public string WarehouseAddress { get; set; }

        [DisplayName("Owner Name")]
        [Required(ErrorMessage = "Enter DB House Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Owner Mobile")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Enter DB House OwnerMoble")]
        public string OwnerMoble { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Create Date")]
        [Column(TypeName = "datetime2")]
        [Required(ErrorMessage = "Enter Create Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Modified Date")]
        [Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Cluster")]
        [Required(ErrorMessage = "Select DB House Cluster")]
        public int ClusterId { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Select DB House  Area")]
        public int ZoneId { get; set; }

        [DisplayName("Price Buandle")]
        [Required(ErrorMessage = "Select DB House Price Buandle")]
        public int PriceBuandleId { get; set; }

        [DisplayName("Delivery Module")]
        [Required(ErrorMessage = "Select DB House Delivery Module status")]
        public int DeliveryModuleStatus { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "Select DB House  status")]
        public int Status { get; set; }

    }
    public class DBhouseiMv
    {

        public int DbId { get; set; }

        [DisplayName("Distributor Name")]
        public string DbName { get; set; }

        [DisplayName("Distributor Code")]
        public string DbCode { get; set; }

        [DisplayName("Office Address")]
        public string OfficeAddress { get; set; }


        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Create Date")]
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Modified Date")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Cluster")]
        public string ClusterId { get; set; }

        [DisplayName("Area")]
        public string Zone { get; set; }

        [DisplayName("Price Buandle")]
        public string PriceBuandle { get; set; }

        [DisplayName("Delivery Module")]
        public string DeliveryModuleStatus { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }


    }
}