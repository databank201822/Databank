using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODMS.Models.ViewModel
{
    public class PurchaseVM
    {
        public int Id { get; set; }
        public int ChallanNo { get; set; }
        public int DbId { get; set; }
        public System.DateTime ChallanDate { get; set; }
        public string ReceivedDate { get; set; }
    }

    public class PurchaseLineVm
    {

        public int Id { get; set; }
        public int Bundelitemid { get; set; }
        public int SkuId { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public double UnitSalePrice { get; set; }
        public int ChallanQuantity { get; set; }
        public double TotalPrice { get; set; }


    }


    public class PurchaseInsertVM
    {
        public int Id { get; set; }
        public int ChallanNo { get; set; }
        public int DbId { get; set; }
        public System.DateTime ChallanDate { get; set; }
        public string ReceivedDate { get; set; }
        public List<PurchaseLineVm> PurchaseLine { get; set; }
    }
}