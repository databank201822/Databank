using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
   
    public class ManagementEmployeeiVm
    {
        public int Id { get; set; }

        [DisplayName("Emp Code")]
        [Required(ErrorMessage = "Enter Emp Code")]
        public string SalesEmpCode { get; set; }

        [DisplayName("First Name")]
        [Required (ErrorMessage="Enter Name")]
        public string FirstName { get; set; }
        

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Enter Address")]
        public string SalesEmpAddress { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Select Role")]
        public int SalesRoleId { get; set; }

        [DisplayName("Parent Zone")]
        public int SalesManagerId { get; set; }

        [DisplayName("User Name")]
        [Required(ErrorMessage = "Select A Username")]
        public int LoginUserId { get; set; }

        [DisplayName("Zone")]
        [Required(ErrorMessage = "Select Zone")]
        public int BizZoneId { get; set; }
    }
    public class ManagementEmployeeiVMindex
    {
        public int Id { get; set; }

        [DisplayName("Emp Code")]
        public string SalesEmpCode { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
       
        public string Email { get; set; }
        [DisplayName("Address Name")]
        public string SalesEmpAddress { get; set; }
        [DisplayName("Role")]
        public string SalesRole { get; set; }

        [DisplayName("User Name")]
        public string LoginUserName { get; set; }

        [DisplayName("Zone")]
        public string BizZoneName { get; set; }
    }
}