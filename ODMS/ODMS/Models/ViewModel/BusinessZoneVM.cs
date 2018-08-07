using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    // ReSharper disable once InconsistentNaming
    public class BusinessZoneVM
    {
        public int Id { get; set; }

        [DisplayName("Zone Name")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Enter biz_zone_name")]
        public string BizZoneName { get; set; }

        [DisplayName("Zone code")]
        [Required(ErrorMessage = "Enter biz_zone_code")]
        public string BizZoneCode { get; set; }

        [DisplayName("Zone description")]
        [Required(ErrorMessage = "Enter biz_zone_description")]
        public string BizZoneDescription { get; set; }

        [DisplayName("Zone Category")]
        [Required(ErrorMessage = "Select biz_zone_category_id")]
        public int BizZoneCategoryId { get; set; }

        [DisplayName("Parent Name")]

        public int ParentBizZoneId { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class businessZoneVMindex
    {
        public int Id { get; set; }

        [DisplayName("Zone Name")]
    
        public string BizZoneName { get; set; }

        [DisplayName("Zone code")]
       
        public string BizZoneCode { get; set; }

        [DisplayName("Zone description")]
       
        public string BizZoneDescription { get; set; }

        [DisplayName("Zone Category")]

        public string BizZoneCategory { get; set; }

        [DisplayName("Parent Name")]

        public string ParentBizZone{ get; set; }
    }


}