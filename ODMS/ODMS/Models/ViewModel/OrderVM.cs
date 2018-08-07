using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    public class OrderVm
    {

        public int Orderid { get; set; }
        public string SoId { get; set; }
        public string LocalSoId { get; set; }
        public int RouteId { get; set; }
        public int OutletId { get; set; }
        public int ChallanNo { get; set; }
        public DateTime PlannedOrderDate { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DbId { get; set; }
        public int PsrId { get; set; }
        public int SoStatus { get; set; }
        public double TotalOrder { get; set; }
        public double TotalConfirmed { get; set; }
        public double TotalDelivered { get; set; }
        public double SalesOrderTypeId { get; set; }
        public double ManualDiscount { get; set; }
    }

    public class OrderLineVm
    {

        public int Id { get; set; }
        public int Orderid { get; set; }
        public int Bundelitemid { get; set; }

        public int SkuId { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public double UnitSalePrice { get; set; }
        public int SkuOrderTypeId { get; set; }
        public int PromotionId { get; set; }
        public string Promotion { get; set; }
        public int Lpec { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityConfirmed { get; set; }
        public int QuantityDelivered { get; set; }
        public double TotalSalePrice { get; set; }
        public double TotalDiscountAmount { get; set; }
        public double TotalBilledAmount { get; set; }


    }

    public class OrderiVm
    {

        public int Orderid { get; set; }
        [Display(Name = "Memo NO")]
        public string SoId { get; set; }
        public string LocalSoId { get; set; }
        [Display(Name = "Route Name")]
        public string RouteName { get; set; }
        [Display(Name = "Outlet Name")]
        public string OutletName { get; set; }
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
        public DateTime? DeliveryDate { get; set; }
        public int DbId { get; set; }
        [Display(Name = "PSR")]
        public string PsrName { get; set; }

        [Display(Name = "Status")]
        public int SoStatus { get; set; }
        public String TotalConfirmed { get; set; }
        [Display(Name = "Order[CS]")]
        public String OrderCs { get; set; }
        [Display(Name = "Delivery[CS]")]
        public String DeliveryCs { get; set; }

        [Display(Name = "Order Amount")]
        public String TotalOrder { get; set; }
      
        [Display(Name = "Delivery Amount")]
        public String TotalDelivered { get; set; }
        public String SalesOrderTypeId { get; set; }
        public String ManualDiscount { get; set; }
        public int IsProcess { get; set; }
       
    }

    public class OrderCreateVm
    {
        
        public int Orderid { get; set; }
        public string SoId { get; set; }
        public string LocalSoId { get; set; }
        [DisplayName("Outlet id")]
        [Required(ErrorMessage = "Enter Outlet Code")]
        public int RouteId { get; set; }

        [DisplayName("Outlet id")]
        [Required(ErrorMessage = "Enter Outlet Code")]
        public int OutletId { get; set; }
        public int ChallanNo { get; set; }
     
        public DateTime PlannedOrderDate { get; set; }

        public DateTime OrderDateTime { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DbId { get; set; }
        public int PsrId { get; set; }
        public int SoStatus { get; set; }
        public double TotalOrder { get; set; }
        public double TotalConfirmed { get; set; }
        public double TotalDelivered { get; set; }
        public int? SalesOrderTypeId { get; set; }
        public double ManualDiscount { get; set; }

        public List<OrderLineVm> OrderLine { get; set; }
    }

    public class OrderLineDetilsVm
    {

        public int Id { get; set; }
        public int Orderid { get; set; }
        public int Bundelitemid { get; set; }
        public int SkuId { get; set; }

        public string SkuName { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public double UnitSalePrice { get; set; }
        public int SkuOrderTypeId { get; set; }
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int Lpec { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityConfirmed { get; set; }
        public int QuantityDelivered { get; set; }
        public double TotalSalePrice { get; set; }
        public double TotalDiscountAmount { get; set; }
        public double TotalBilledAmount { get; set; }


    }
    public class OrderDetailsVm
    {

        public int Orderid { get; set; }
        public string SoId { get; set; }
        public string LocalSoId { get; set; }
        public int ChallanId { get; set; }
        public string SubRoute { get; set; }
        public string OutletName { get; set; }
        public string OutletAddress { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PlannedOrderDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDateTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ShippingDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }
        public int DbId { get; set; }
        public string PsrName { get; set; }
        public int SoStatus { get; set; }
        public double TotalOrder { get; set; }
        public double TotalConfirmed { get; set; }
        public double TotalDelivered { get; set; }
       
        public double ManualDiscount { get; set; }

        public string Promotation { get; set; }

        public List<OrderLineDetilsVm> OrderLine { get; set; }
    }

}