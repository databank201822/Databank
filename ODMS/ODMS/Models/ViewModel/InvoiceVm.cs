using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    public class InvoiceVm
    {

        public int Orderid { get; set; }
        [Display(Name = "Memo NO")]
        public string SoId { get; set; }
        public string LocalSoId { get; set; }
        [Display(Name = "Route Name")]
        public string RouteName { get; set; }
        [Display(Name = "Outlet Name")]
        public string OutletName { get; set; }

        [Display(Name = "Outlet Address")]
        public string OutletAddress { get; set; }
        public int ChallanNo { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PlannedOrderDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDateTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ShippingDate { get; set; }

        [Display(Name = "Delivery Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Distributor Name")]
        public string DbName { get; set; }

        [Display(Name = "Distributor Address")]
        public string DbAddress { get; set; }


        [Display(Name = "Distributor Mobile")]
        public string DbMobile { get; set; }

        
      
        [Display(Name = "PSR")]
        public string PsrName { get; set; }

        [Display(Name = "PSR Mobile")]
        public string PsrMobile { get; set; }

        [Display(Name = "Status")]
        public int SoStatus { get; set; }
        public String TotalConfirmed { get; set; }
        [Display(Name = "Order[CS]")]
        public String OrderCs { get; set; }
        [Display(Name = "Delivery[CS]")]
        public String DeliveryCs { get; set; }

        [Display(Name = "Order Amount")]
        public int TotalOrder { get; set; }

        [Display(Name = "Delivery Amount")]
        public int TotalDelivered { get; set; }
        public int SalesOrderTypeId { get; set; }
        public double ManualDiscount { get; set; }


        public String TotalInword { get; set; }
        
        public List<InvoiceLineDetilsVm> InvoiceLine { get; set; }
    }


    public class InvoiceLineDetilsVm
    {

        public int Id { get; set; }
        public int Orderid { get; set; }
        public int Bundelitemid { get; set; }
        public int SkuId { get; set; }


        public int SkuCode { get; set; }
        
        
        public string SkuName { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public double UnitSalePrice { get; set; }
        public int SkuOrderTypeId { get; set; }
        public int PromotionId { get; set; }
        public int Lpec { get; set; }
        
        public int QuantityDeliveredCs { get; set; }
        public int QuantityDeliveredPs { get; set; }
        public int QuantityDelivered { get; set; }

        public int QuantityFree { get; set; }
       


        public double TotalSalePrice { get; set; }
        public double TotalDiscountAmount { get; set; }
        public double TotalBilledAmount { get; set; }


    }

}