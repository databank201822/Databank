using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ODMS.Models.ViewModel
{
    public class TradePromotionVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TpType { get; set; }
        public int TpOfferType { get; set; }
        public int PromotionUnitId { get; set; }
        public int PromotionSubUnitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreateDate { get; set; }
        [DisplayName("Status")]
        public int IsActive { get; set; }
    }

    public class TradePromotioninfoVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TpType { get; set; }
        public int TpOfferType { get; set; }
        public int PromotionUnitId { get; set; }
        public int PromotionSubUnitId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? IsActive { get; set; }
       
        public List<int> DbId { get; set; }

        public int[] ClusterList { get; set; }
        public string OfferType { get; set; }
        public string ConditionType { get; set; }
        public string RuleType { get; set; }


        public List<TpOrderskuDetailslistVm> Orderskulist { get; set; }
        public List<TpOrderskuDetailslistVm> Freeskulist { get; set; }
    }

    public class TpOrderskuDetailslistVm
    {
        public string Sku { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public int Qty { get; set; }

        public int SlabCsQty { get; set; }
    }
    public class TradePromotionCreateVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TpType { get; set; }
        public int TpOfferType { get; set; }
        public int PromotionUnitId { get; set; }
        public int PromotionSubUnitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int IsActive { get; set; }
        public int[] RsmId { get; set; }
        public int[] AsmId { get; set; }
        public int[] CeId { get; set; }
        public int[] DbId { get; set; }

        public int[] ClusterList { get; set; }
        public int OfferType { get; set; }
        public int ConditionType { get; set; }
        public int RuleType { get; set; }
        

        public List<TpOrderskulistVm> Orderskulist { get; set; }
        public List<TpOrderskulistVm> Freeskulist { get; set; }
        public int ValueDiscount { get; set; }
    }

    public class TpOrderskulistVm
    {
        public int SkuId { get; set; }
        public int BetchId { get; set; }
        public int PackSize { get; set; }
        public int Qty { get; set; }

        public int SlabCsQty { get; set; }
    }

    public class Tpvm
    {
        public int SkuId { get; set; }
        public int PromoId { get; set; }
        public int BatchId { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityConfirmed { get; set; }
        public int QuantityDelivered { get; set; }
    }
}