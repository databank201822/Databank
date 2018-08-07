using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{

    public class UseriVm
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRoleId { get; set; }
        public string UserStatus { get; set; }
        public string UserBizRoleId { get; set; }
    }
    public class UserVm
    {
        public int UserId { get; set; }

        [DisplayName("User Name")]
        [Required(ErrorMessage="Enter User Name")]
        public string UserName { get; set; }
        [DisplayName("User Password")]
        [Required(ErrorMessage = "Enter User Password")]
        public string UserPassword { get; set; }

        [DisplayName("User Role")]
        [Required(ErrorMessage = "Select User Role")]
        public int UserRoleId { get; set; }
        [DisplayName("User Status")]
        [Required(ErrorMessage = "Select User Status")]
        public int UserStatus { get; set; }

        [DisplayName("Accessible Area")]
        [Required(ErrorMessage = "Enter User Name")]
        public int UserBizRoleId { get; set; }
    }
}