using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODMS.Models.ViewModel
{
    public class BundlePriceVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }   
        public int Count { get; set; }
      
    }
    public class BundlePriceDetailsVm
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BundlePriceDetailsitemVm> BundlePriceDetailsitemVm { get; set; }
       
    }


     public class BundlePriceDetailsitemVm
    {
        public int Id { get; set; }
        public int BundlePriceId { get; set; }

        [DisplayName("SKU")]
        
        [Required(ErrorMessage = "Enter SKU")]
        public int SkuId { get; set; }
        public int BatchId { get; set; }
        public int Quantity { get; set; }

        [DisplayName("DB Price")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage="Enter DB House Price")]
        public double DbLiftingPrice { get; set; }

        [DisplayName("Trade Price")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Enter Trade Price")]
        public double OutletLiftingPrice { get; set; }

        [DisplayName("MRP")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Enter MRP")]
        public double Mrp { get; set; }

        [DisplayName("Start Date")]
        [Column(TypeName = "datetime2")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        [Column(TypeName = "datetime2")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    
    }


     public class BundlePriceindexVm
     {
         public int Id { get; set; }
         [DisplayName("Bundle Price Name")]
         public string Name { get; set; }

         public List<BundlePriceindexitemVm> BundlePriceindexitemVm { get; set; }
     }


     public class BundlePriceindexitemVm
     {
         public int Id { get; set; }
         public int BundlePriceId { get; set; }
         public string SkuName { get; set; }
         public int BatchId { get; set; }
         public double DbPrice { get; set; }
         public double OutletPrice { get; set; }
         public double Mrp { get; set; }

        [DataType(DataType.Date)]
         public DateTime StartDate { get; set; }
         [DataType(DataType.Date)]
         public DateTime EndDate { get; set; }
         public string Status { get; set; }

     }
}