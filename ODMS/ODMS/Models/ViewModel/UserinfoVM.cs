using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    public class UserinfoVm
    {


        public string UserId { get; set; }

        [Required(ErrorMessage = "Enter user name")]
        public string UserName { get; set; }

        public string UserFullName { get; set; }


        [Required ( ErrorMessage="Enter password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        public string UserRoleCode { get; set; }

        public string UserType { get; set; }
                
        public string FirstName { get; set; }
        public int BizZoneId { get; set; }
        public string IpAddress { get; set; }
      
    }
}