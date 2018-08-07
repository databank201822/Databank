using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODMS.Models.ViewModel
{
    public class ChallanVm
    {
        
        public int Id { get; set; }
        [Display(Name = "Challan Number")]
        public string ChallanNumber { get; set; }
        public int DbId { get; set; }
        public int PsrId { get; set; }
        [Display(Name = "PSR")]
        public string PsrName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        [Display(Name = "Status")]
        public int ChallanStatus { get; set; }
        [Display(Name = "No of Memo")]
        public int NoOfMemo { get; set; }
        public int NoOfOutlet { get; set; }
        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Delivery Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime SystemDate { get; set; }
        [Display(Name = "Order[CS]")]
        public double GrandTotalCs { get; set; }
        [Display(Name = "Delivery[CS]")]
        public double DeliveryGrandTotalCs { get; set; }
        [Display(Name = "Order Amount")]
        public double GrandTotal { get; set; }
        [Display(Name = "Delivery Amount")]
        public double DeliveryGrandTotal { get; set; }
      
    }
    public  class ChallanlineVm
    {
        public int Id { get; set; }
        public int ChallanId { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public int BatchId { get; set; }
        public double Price { get; set; }
        public int PackSize { get; set; }
        public int StockQty { get; set; }
        public int OrderCsQty { get; set; }
        public int OrderPsQty { get; set; }
        public int OrderQty { get; set; }
        public int ExtraQty { get; set; }
        public int FreeCsQty { get; set; }
        public int FreePsQty { get; set; }
        public int FreeQty { get; set; }
        public int TotalCsQty { get; set; }
        public int TotalPsQty { get; set; }
        public int TotalQty { get; set; }
        public int ConfirmQty { get; set; }

        public int ReturnQty { get; set; }
        public int ConfirmCsQty { get; set; }
        public int ConfirmPsQty { get; set; }
        public int ConfirmFreeQty { get; set; }
        public double TotalQtyInCs { get; set; }
        public double OrderQtyPrice { get; set; }
        public double ExtraQtyPrice { get; set; }
        public double TotalQtyPrice { get; set; }
        public double ConfirmQtyPrice { get; set; }

    }
    public class ChallaniVm
    {
        public int Id { get; set; }
        public string ChallanNumber { get; set; }
        public int DbId { get; set; }
        public int PsrId { get; set; }
        public string PsrName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int ChallanStatus { get; set; }
        public int NoOfMemo { get; set; }
        public int NoOfOutlet { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime SystemDate { get; set; }
        public double GrandTotalCs { get; set; }
        public double DeliveryGrandTotalCs { get; set; }
        public double GrandTotal { get; set; }
        public double DeliveryGrandTotal { get; set; }
        public string Momonumber { get; set; }
        
        public List<ChallanlineVm> Challanline{ get; set; }
        

    }
}