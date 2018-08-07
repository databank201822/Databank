using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    public class DBhouseoutletiVm
    {
        [DisplayName("OutletId")]
        public int OutletId { get; set; }

        [DisplayName("Outlet Code")]
        public string OutletCode { get; set; }

        [DisplayName("Outlet Name")]
        public string OutletName { get; set; }

        [DisplayName("Outlet Name [Bangla]")]
        public string OutletNameB { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("GPS Location")]
        public string GpsLocation { get; set; }

        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Contact No")]
        public string ContactNo { get; set; }

        [DisplayName("Distributor Name")]
        public String Distributor { get; set; }

        [DisplayName("Visicooler")]
        public string HaveVisicooler { get; set; }

        [DisplayName("Parent")]
        public string Parentid { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Grading")]
        public string Grading { get; set; }

        [DisplayName("Channel")]
        public string Channel { get; set; }

        [DisplayName("Latitude")]
        public string Latitude { get; set; }

        [DisplayName("Longitude")]
        public string Longitude { get; set; }

        [DisplayName("Picture")]
        public string Picture { get; set; }

        [DisplayName("Status")]
        public string IsActive { get; set; }

        [DisplayName("Create Date")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createdate { get; set; }
    }

    public class OutletUploadVm
    {
        [DisplayName("OutletId")]
        public string OutletId { get; set; }

        [DisplayName("Outlet Code")]
        public string OutletCode { get; set; }

        [DisplayName("Outlet Name")]
        public string OutletName { get; set; }

        [DisplayName("Outlet Name [Bangla]")]
        public string OutletNameB { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("GPS Location")]
        public string GpsLocation { get; set; }

        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Contact No")]
        public string ContactNo { get; set; }

        [DisplayName("Distributor ID")]
        public int Distributor { get; set; }

        [DisplayName("Distributor Name")]
        public String DistributorName { get; set; }

        [DisplayName("Visicooler")]
        public string HaveVisicooler { get; set; }

        [DisplayName("Route_id")]
        public int Parentid { get; set; }


        [DisplayName("Route_Name")]
        public string ParentName { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Grading")]
        public string Grading { get; set; }

        [DisplayName("Channel")]
        public string Channel { get; set; }

        [DisplayName("Latitude")]
        public string Latitude { get; set; }

        [DisplayName("Longitude")]
        public string Longitude { get; set; }

        [DisplayName("Picture")]
        public string Picture { get; set; }

        [DisplayName("Status")]
        public string IsActive { get; set; }

      
        public string Createdate { get; set; }
    }
    public class DBhouseoutletVm
    {
        [DisplayName("OutletId")]
        [Required(ErrorMessage="Enter Outlet ID")]
        public int OutletId { get; set; }

        [DisplayName("Outlet Code")]
        [Required(ErrorMessage = "Enter Outlet Code")]
        public string OutletCode { get; set; }

        [DisplayName("Outlet Name")]
        [Required(ErrorMessage = "Enter Outlet Name")]
        public string OutletName { get; set; }

        [DisplayName("Outlet Name [Bangla]")]
        public string OutletNameB { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Enter Outlet Address")]
        public string Address { get; set; }

        [DisplayName("GPS Location")]
        public string GpsLocation { get; set; }

        [DisplayName("Owner Name")]
        [Required(ErrorMessage = "Enter Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Contact No")]
        [Required(ErrorMessage = "Enter Outlet Contact No")]
        public string ContactNo { get; set; }

        [DisplayName("Distributor Name")]
        [Required(ErrorMessage = "Select Distributor Name")]
        public int Distributorid { get; set; }

        [DisplayName("Visicooler")]
        [Required(ErrorMessage="Select Visicooler")]
        public int HaveVisicooler { get; set; }

        [DisplayName("Sub Route")]
        [Required(ErrorMessage="Enter Outlet ID")]
        public int Parentid { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Enter Outlet Category")]
        public int OutletCategoryId { get; set; }

        [DisplayName("Grading")]
        [Required(ErrorMessage = "Enter Outlet Grading")]
        public int Grading { get; set; }

        [DisplayName("Channel")]
        [Required(ErrorMessage = "Enter Outlet Channel")]
        public int Channel { get; set; }

        [DisplayName("Latitude")]

        public string Latitude { get; set; }

        [DisplayName("Longitude")]
        public string Longitude { get; set; }

        [DisplayName("Picture")]
        public string Picture { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "Enter Outlet Status")]
        public int IsActive { get; set; }
       

        [DisplayName("Create Date")]        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createdate { get; set; }
    }

    public class DBhouseoutletQrVm
    {
        [DisplayName("OutletId")]
        public int OutletId { get; set; }

        [DisplayName("Outlet Code")]
        public string OutletCode { get; set; }

        [DisplayName("Outlet Name")]
        public string OutletName { get; set; }

        [DisplayName("Outlet Mobile")]
        public string OutletMobile { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Distributor Name")]
        public String Distributor { get; set; }

        
        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Grading")]
        public string Grading { get; set; }

        [DisplayName("Channel")]
        public string Channel { get; set; }


        public byte[] QrImage { get; set; }
    }
}