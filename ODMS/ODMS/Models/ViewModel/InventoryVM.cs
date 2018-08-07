namespace ODMS.Models.ViewModel
{
    public class InventoryiVm
    {
        public int Id { get; set; }
        public int DbId { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public int? PackSize { get; set; }
        public int BundleItemid { get; set; }
        public int BatchNo { get; set; }
        public double TradePrice { get; set; }
        public int Currentqty { get; set; }
        public int Bookedqty { get; set; }
        public int Totalqty { get; set; }
       

    }

    public class InventoryAdjustmentVm
    {
        public int Id { get; set; }
        public int DbId { get; set; }
        public int SkuId { get; set; }
        public string SkuName { get; set; }
        public int? PackSize { get; set; }
        public int BundleItemid { get; set; }
        public int BatchNo { get; set; }
        public double TradePrice { get; set; }
        public int Currentqty { get; set; }
        public int CurrentCSqty { get; set; }
        public int CurrentPSqty { get; set; }
        
        public int Adjustmentqty { get; set; }
        public int AdjustmentCSqty { get; set; }
        public int AdjustmentPSqty { get; set; }


    }




}