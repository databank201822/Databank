using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models.ViewModel
{
    public class DBhouseemployeeiVm
    {
        public int Id { get; set; }

        [DisplayName("Employee Code")]
        public string EmpCode { get; set; }
        [DisplayName("Employee Name")]
        public string Name { get; set; }

        [DisplayName("DB House Name")]
        public string Distribution { get; set; }

        [DisplayName("Employee Type")]
        public string EmpType { get; set; }

        [DisplayName("Employee Login Id")]
        public string LoginUserId { get; set; }

        [DisplayName("Employee Status")]
        public string Active { get; set; }

        public string EmpAddress { get; set; }

        [Column(TypeName = "Password")]
        public string LoginUserPassword { get; set; }
        public string ContactNo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? JoiningDate { get; set; }
        public string Designation { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string EmergencyContactPerson { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EducationalQualification { get; set; }
        public string Image { get; set; }
    }
    public class DBhouseemployeeVm
    {
        public int Id { get; set; }
        [DisplayName("Employee Code")]
        public string EmpCode { get; set; }
        [DisplayName("Employee Name")]
        public string Name { get; set; }

        [DisplayName("Employee Address")]
        public string EmpAddress { get; set; }
        [DisplayName("Employee User Role")]
        public int? UserRoleId { get; set; }
        [DisplayName("Distribution House Name")]
        public int? DistributionId { get; set; }
        [DisplayName("Login User")]
        public string LoginUserId { get; set; }
        [DisplayName("Login Password")]
        public string LoginUserPassword { get; set; }
        [DisplayName("Mobile No")]
        public string ContactNo { get; set; }
        [DisplayName("Joining Date")]
        [Column(TypeName = "datetime2")]
        public DateTime? JoiningDate { get; set; }
       
        public string Designation { get; set; }
        public int? EmpType { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public string EmergencyContactPerson { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EducationalQualification { get; set; }
        public string Image { get; set; }
        public int Active { get; set; }
    }
}