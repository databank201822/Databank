using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODMS.Models.ViewModel
{
    public class PrimaryReceiveVm
    {
        public int DeliveryChallanNumber { get; set; }

        public DateTime DeliveryChallanDate { get; set; }
        public int ProductID { get; set; }
        public int ConversionFactor { get; set; }
        public double Quantitycs { get; set; }

        public int PassCode { get; set; }

        
    }

    public class PrimaryReceiveLineVm
    {

        public int Id { get; set; }
        public int PrimaryChallanno { get; set; }
        public DateTime PrimaryChallanDate { get; set; }
        public string SkuName { get; set; }
        public int SkuId { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public double UnitSalePrice { get; set; }
        public int ChallanQuantity { get; set; }
        public int ChallanQuantityCs { get; set; }
        public int ChallanQuantityPs { get; set; }

        public int ReceiveQuantity { get; set; }
        public int ReceiveQuantityCs { get; set; }
        public int ReceiveQuantityPs { get; set; }
        public double TotalPrice { get; set; }


    }
}