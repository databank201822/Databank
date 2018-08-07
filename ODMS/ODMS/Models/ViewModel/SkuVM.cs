using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ODMS.Models.ViewModel
{
    public class SkuiVM
    {
        public int SkuId { get; set; }

        [DisplayName("Name")]
        public string SkuName { get; set; }
        [DisplayName("CODE")]
        public int SkUcode { get; set; }

        [DisplayName("SKU Description")]
        public string SkUdescription { get; set; }

        [DisplayName("SKU Launch Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime SkUlaunchdate { get; set; }
        public string SkUbrand { get; set; }

        [DisplayName("Volume [ml]")]
        public double SkuVolumeMl { get; set; }
        [DisplayName("Volume [8oz]")]
        public double SkuVolume8Oz { get; set; }

        public string SkuNameB { get; set; }

        [DisplayName("LPSC")]
        public int SkUlpc { get; set; }
        public int SkuUnit { get; set; }
        [DisplayName("SKU Status")]
        public string SkuStatus { get; set; }



    }

    public class Skuvm
    {
        [DisplayName("SKU id")]
        public int SkuId { get; set; }

        [DisplayName("SKU Name")]
        [Required(ErrorMessage = "Enter SKU Name")]
        public string SkuName { get; set; }

        [DisplayName("SKU  Bangla Name")]
        [Required(ErrorMessage = "Enter SKU Bangla Name")]
        public string SkuNameB { get; set; }
        [DisplayName("SKU Code")]
        [Required(ErrorMessage = "Enter SKU Code")]
        public int SkUcode { get; set; }

        [DisplayName("SKU Description")]
        [Required(ErrorMessage = "Enter SKU Nam Description")]
        public string SkUdescription { get; set; }
        [DisplayName("SKU SL")]
        [Required(ErrorMessage = "Enter SKU SL")]
        public int SkUsl { get; set; }

        [DisplayName("SKU Flavor")]

        public int SkUtypeId { get; set; }

        [DisplayName("SKU node type ")]

        public int SkUnodetypeId { get; set; }

        [DisplayName("SKU Status")]
        [Required(ErrorMessage = "Enter SKU Status")]
        public int SkuStatus { get; set; }


        [DisplayName("SKU Create date")]
        [Column(TypeName = "datetime2")]
        public DateTime SkUcreationdate { get; set; }

        [DisplayName("SKU launch Date")]
        [Column(TypeName = "datetime2")]
        public DateTime SkUlaunchdate { get; set; }

        [DisplayName("SKU Expiry Date")]
        [Column(TypeName = "datetime2")]
        public DateTime SkUexpirydate { get; set; }

        [DisplayName("SKU brand")]
        [Required(ErrorMessage = "Enter SKU brand")]
        public int SkUbrandId { get; set; }

        [DisplayName("SKU Category")]
        [Required(ErrorMessage = "Enter SKU Category")]
        public int SkUcategoryid { get; set; }

        [DisplayName("SKU Volume [ml]")]
        [Required(ErrorMessage = "Enter SKU Volume [ml]")]
        public double SkuVolumeMl { get; set; }

        [DisplayName("SKU Volume [8oZ]")]
        [Required(ErrorMessage = "Enter SKU Volume [8oZ]")]
        public double SkuVolume8Oz { get; set; }

        [DisplayName("SKU Container Type")]
        [Required(ErrorMessage = "Enter SKU Container Type")]
        public int SkuContainertypeid { get; set; }

        [DisplayName("SKU LPSC")]
        [Required(ErrorMessage = "Enter SKU LPSC")]
        public int SkUlpc { get; set; }

        [DisplayName("SKU Unit")]
        [Required(ErrorMessage = "Enter SKU Unit")]
        public int SkuUnit { get; set; }
    }



    public class SkuBrand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
        public string BrandDescription { get; set; }
        public string Category { get; set; }
        public string Parent { get; set; }
    }
}