using System.Collections.Generic;
using System.Linq;
using ODMS.Models;
using ODMS.Models.ViewModel;

namespace ODMS.Controllers
{
    [SessionExpire]
    public class TradePromotionSupporting
    {
        private ODMSEntities _db = new ODMSEntities();

        private int _multiSkuAndNumberOfSkuInOrderCount = 0;
        private int _multiSkuOrNumberOfSkuInOrderCount = 0;
        private int _multiSlabNumberOfSkuInOrderCount = 0;

        private List<int> _tpAndAffactedPromoids = new List<int>();
        private List<int> _tpOrAffactedPromoids = new List<int>();
        private List<int> _tpslabAffactedPromoids = new List<int>();


        private List<int> _multiSkuAndSkusList = new List<int>();


        public void ImpactTradepromotaton(int orderid, HashSet<int> dbtpList)
        {

            _db.tblt_Order_line.RemoveRange(_db.tblt_Order_line.Where(x => x.Orderid == orderid && x.sku_order_type_id == 2));
            _db.SaveChanges();

            HashSet<int> orederLineeligiblesku = new HashSet<int>(_db.tblt_TradePromotionDefinition.Where(x => dbtpList.Contains(x.promo_id) && x.promo_line_type != 2).Select(x => x.condition_sku_id));


            _db.tblt_Order_line.Where(x => x.Orderid == orderid && x.sku_order_type_id == 1).ToList().ForEach(x =>
            {
                x.promotion_id = 0;
            });
            _db.SaveChanges();

            var orederLine = _db.tblt_Order_line.Where(x => x.Orderid == orderid && orederLineeligiblesku.Contains(x.sku_id) && x.sku_order_type_id == 1).ToList();

         
           

            if (orederLine.Count != 0)
            {
                TradepromotatonNotDefine(dbtpList, orederLine, orderid);
            }
            

            _db.tblt_Order.Where(x => x.Orderid == orderid).ToList().ForEach(x =>
            {
                x.isProcess = 1;
            });

            _db.SaveChanges();

        }


        private void TradepromotatonNotDefine(HashSet<int> dbtpList, List<tblt_Order_line> orederLine,int orderid)
        {
            _multiSkuAndNumberOfSkuInOrderCount = 0;
            _multiSkuOrNumberOfSkuInOrderCount = 0;
            _multiSlabNumberOfSkuInOrderCount = 0;
            //Quentity && AND && Single_SKU

            foreach (var orederLineitem in orederLine)
            {
                var tradePromotionDefinitionLine = _db.tblt_TradePromotionDefinition.FirstOrDefault(x => dbtpList.Contains(x.promo_id) && x.condition_sku_id == orederLineitem.sku_id && x.promo_line_type != 2);
                
                if (tradePromotionDefinitionLine != null && tradePromotionDefinitionLine.offer_type == 2 && tradePromotionDefinitionLine.promo_line_type == 3)
                {
                    Value_and_Single_SKU(orederLineitem, tradePromotionDefinitionLine.promo_id);
                }

                //Quentity Free
                if (tradePromotionDefinitionLine != null && tradePromotionDefinitionLine.offer_type == 1 && tradePromotionDefinitionLine.promo_line_type == 1)
                {
                    // Start Quentity && AND && Single_SKU
                    if (tradePromotionDefinitionLine.condition_type == 1 && tradePromotionDefinitionLine.rule_type == 1)
                    {

                        Quentity_and_Single_SKU(orederLineitem, tradePromotionDefinitionLine.promo_id);

                    }
                    //End Quentity && AND && Single_SKU


                    // Start Quentity && AND && Multi_SKU 
                    if (tradePromotionDefinitionLine.condition_type == 1 && tradePromotionDefinitionLine.rule_type == 2)
                    {
                        _multiSkuAndNumberOfSkuInOrderCount++;

                        if (!_tpAndAffactedPromoids.Contains(tradePromotionDefinitionLine.promo_id))
                        {
                            _tpAndAffactedPromoids.Add(tradePromotionDefinitionLine.promo_id);
                        }

                        if (!_multiSkuAndSkusList.Contains(tradePromotionDefinitionLine.condition_sku_id))
                        {
                            _multiSkuAndSkusList.Add(tradePromotionDefinitionLine.condition_sku_id);
                        }


                    }

                    // End Quentity && AND && Multi_SKU 


                    // Start Quentity && OR && Multi_SKU 
                    if (tradePromotionDefinitionLine.condition_type == 2 && tradePromotionDefinitionLine.rule_type == 2)
                    {
                        _multiSkuOrNumberOfSkuInOrderCount++;

                        if (!_tpOrAffactedPromoids.Contains(tradePromotionDefinitionLine.promo_id))
                        {
                            _tpOrAffactedPromoids.Add(tradePromotionDefinitionLine.promo_id);
                        }
                        
                    }

                    // End Quentity && OR && Multi_SKU 



                    // Start Quentity && OR && Multi_Slab
                    if (tradePromotionDefinitionLine.condition_type == 2 && tradePromotionDefinitionLine.rule_type == 3)
                    {
                        _multiSlabNumberOfSkuInOrderCount++;

                        if (!_tpslabAffactedPromoids.Contains(tradePromotionDefinitionLine.promo_id))
                        {
                            _tpslabAffactedPromoids.Add(tradePromotionDefinitionLine.promo_id);
                        }

                    }
                    // End Quentity && OR && Multi_Slab 

                }
            }


            // Quentity && And && Multi_SKU

            if (_multiSkuAndNumberOfSkuInOrderCount > 1)
            {
                Quentity_AND_Multi_SKU(_tpAndAffactedPromoids, orederLine, orderid);
            }

            // Quentity && OR && Multi_SKU
           if(_multiSkuOrNumberOfSkuInOrderCount>=1)
            {
                Quentity_OR_Multi_SKU(_tpOrAffactedPromoids, orederLine, orderid);
            }


            // Quentity && OR && Multi_Slab
           if (_multiSlabNumberOfSkuInOrderCount >= 1)
            {
                Quentity_OR_Multi_Slab(_tpslabAffactedPromoids, orederLine, orderid);
            }

        }

        //Value && AND && Single_SKU
        private void Value_and_Single_SKU(tblt_Order_line orederLineitem, int promoId)
        {

            var tpSingleSkuDefinitionOrderLine = _db.tblt_TradePromotionDefinition.FirstOrDefault(x => x.condition_sku_id == orederLineitem.sku_id && x.promo_line_type == 3 && x.promo_id == promoId); //Get Full Line   
            
            if (tpSingleSkuDefinitionOrderLine != null)
            {
                int discountAmount = ((orederLineitem.quantity_delivered - (orederLineitem.quantity_delivered % tpSingleSkuDefinitionOrderLine.condition_sku_amount)) / tpSingleSkuDefinitionOrderLine.condition_sku_amount) * tpSingleSkuDefinitionOrderLine.offer_sku_amount;


                _db.tblt_Order.Where(x => x.Orderid == orederLineitem.Orderid).ToList().ForEach(x =>
                {
                    x.total_delivered = x.total_delivered- discountAmount;
                    
                }); //Update TP Order By discount Update

            

                _db.tblt_Order_line.Where(x => x.id == orederLineitem.id).ToList().ForEach(x =>
                {
                    x.total_discount_amount = discountAmount;
                    
                    x.total_billed_amount = (x.unit_sale_price * x.quantity_delivered) - discountAmount;
                    x.promotion_id = tpSingleSkuDefinitionOrderLine.promo_id;

                }); //Update TP Order Line By Promo_id Update

                _db.SaveChanges();





            }

            
        }
        
        //Quentity && AND && Single_SKU
        private void Quentity_and_Single_SKU(tblt_Order_line orederLineitem, int promoId)
        {

            var tpSingleSkuDefinitionOrderLine =_db.tblt_TradePromotionDefinition.FirstOrDefault(x => x.condition_sku_id == orederLineitem.sku_id && x.promo_line_type == 1 && x.promo_id==promoId); //Get Full Line   
            var tpSingleSkuDefinitionFreeLine =_db.tblt_TradePromotionDefinition.FirstOrDefault(x => x.promo_id == tpSingleSkuDefinitionOrderLine.promo_id &&x.promo_line_type == 2 && x.promo_id == promoId); //Get Free Line

            if (tpSingleSkuDefinitionFreeLine != null && tpSingleSkuDefinitionOrderLine != null)
            {

               

                int getfreequantityOrder =
                    (orederLineitem.quantity_ordered - (orederLineitem.quantity_ordered %
                                                        tpSingleSkuDefinitionOrderLine.condition_sku_amount)) /
                    tpSingleSkuDefinitionOrderLine.condition_sku_amount * tpSingleSkuDefinitionFreeLine.offer_sku_amount;
                int getfreequantityDelivered =
                    (orederLineitem.quantity_delivered - (orederLineitem.quantity_delivered %
                                                          tpSingleSkuDefinitionOrderLine.condition_sku_amount)) /
                    tpSingleSkuDefinitionOrderLine.condition_sku_amount * tpSingleSkuDefinitionFreeLine.offer_sku_amount;
                int getfreequantityconfirmed =
                    (orederLineitem.quantity_confirmed - (orederLineitem.quantity_confirmed %
                                                          tpSingleSkuDefinitionOrderLine.condition_sku_amount)) /
                    tpSingleSkuDefinitionOrderLine.condition_sku_amount * tpSingleSkuDefinitionFreeLine.offer_sku_amount;

                if (getfreequantityOrder > 0 || getfreequantityDelivered > 0 || getfreequantityconfirmed > 0)
                {

                    tblt_Order_line freeOrderLine = new tblt_Order_line
                    {
                        Orderid = orederLineitem.Orderid,
                        Bundelitmeid = 0,
                        sku_id = tpSingleSkuDefinitionFreeLine.offer_sku_id,
                        Betch_id = tpSingleSkuDefinitionFreeLine.offer_sku_Batch,
                        Pack_size = tpSingleSkuDefinitionFreeLine.offer_sku_pack_size,
                        unit_sale_price = 0,
                        sku_order_type_id = 2,
                        promotion_id = tpSingleSkuDefinitionOrderLine.promo_id,
                        lpec = 0,
                        quantity_ordered = getfreequantityOrder,
                        quantity_confirmed = getfreequantityconfirmed,
                        quantity_delivered = getfreequantityDelivered,
                        total_sale_price = 0,
                        total_discount_amount = 0,
                        total_billed_amount = 0,

                    };

                    TradepromotatonFreeOrderLine(freeOrderLine);
                }
                
                _db.tblt_Order_line.Where(x => x.id == orederLineitem.id).ToList().ForEach(x =>
                {
                    x.promotion_id = tpSingleSkuDefinitionOrderLine.promo_id;

                }); //Update TP Order Line By Promo_id Update

                _db.SaveChanges();





            }
        }
        
        //Quentity && AND && Multi_SKU
        private void Quentity_AND_Multi_SKU(List<int> tpAffactedPromoid, List<tblt_Order_line> orederLine, int orderid)
        {
           

            foreach (var promoid in tpAffactedPromoid)
            {
                List<Tpvm> slabCount = new List<Tpvm>();
               
                var tradePromotionDefinitionLine =_db.tblt_TradePromotionDefinition.Where(x => x.promo_id == promoid && x.promo_line_type == 1);
                var tradePromotionFreeLine =_db.tblt_TradePromotionDefinition.SingleOrDefault(x => x.promo_id == promoid && x.promo_line_type == 2);

                foreach (var item in tradePromotionDefinitionLine)
                {
                    var dataLine = (from a in orederLine
                        where a.sku_id == item.condition_sku_id && a.Betch_id == item.condition_sku_Batch
                        select new Tpvm
                        {
                            PromoId = promoid,
                            SkuId = a.sku_id,
                            BatchId = a.Betch_id,
                            QuantityOrdered = a.quantity_ordered / item.condition_sku_amount,
                            QuantityConfirmed = a.quantity_confirmed / item.condition_sku_amount,
                            QuantityDelivered = a.quantity_delivered / item.condition_sku_amount
                        }).SingleOrDefault();

                    slabCount.Add(dataLine);
                   

                }


                var quantityOrderedslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityOrdered).Min();
                var quantityConfirmedslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityConfirmed).Min();
                var quantityDeliveredslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityDelivered).Min();


                var orederLineids = (from a in orederLine
                    join b in slabCount on new {A = a.sku_id, B = a.Betch_id} equals new {A = b.SkuId, B = b.BatchId}
                    select a.id).ToList();

                if (tradePromotionFreeLine != null)
                {
                    int getfreequantityOrder = tradePromotionFreeLine.offer_sku_amount * quantityOrderedslabCount;
                    int getfreequantityDelivered =tradePromotionFreeLine.offer_sku_amount * quantityConfirmedslabCount;
                    int getfreequantityconfirmed =tradePromotionFreeLine.offer_sku_amount * quantityDeliveredslabCount;


                    if (getfreequantityOrder > 0 || getfreequantityDelivered > 0 || getfreequantityconfirmed > 0)
                    {
                        
                            tblt_Order_line freeOrderLine = new tblt_Order_line
                            {
                                Orderid = orderid,
                                Bundelitmeid = 0,
                                sku_id = tradePromotionFreeLine.offer_sku_id,
                                Betch_id = tradePromotionFreeLine.offer_sku_Batch,
                                Pack_size = tradePromotionFreeLine.offer_sku_pack_size,
                                unit_sale_price = 0,
                                sku_order_type_id = 2,
                                promotion_id = tradePromotionFreeLine.promo_id,
                                lpec = 0,
                                quantity_ordered = getfreequantityOrder,
                                quantity_confirmed = getfreequantityconfirmed,
                                quantity_delivered = getfreequantityDelivered,
                                total_sale_price = 0,
                                total_discount_amount = 0,
                                total_billed_amount = 0,

                            };

                            TradepromotatonFreeOrderLine(freeOrderLine);
                        
                    }
                    _db.tblt_Order_line.Where(x => orederLineids.Contains(x.id)).ToList().ForEach(x =>
                    {
                        x.promotion_id = promoid;

                    }); //Update TP Order Line By Promo_id Update

                    _db.SaveChanges();  

                }

            }
        }
        
        //Quentity && OR && Multi_SKU
        private void Quentity_OR_Multi_SKU(List<int> tpAffactedPromoid, List<tblt_Order_line> orederLine, int orderid)
        {


            foreach (var promoid in tpAffactedPromoid)
            {
                List<Tpvm> slabCount = new List<Tpvm>();

                var tradePromotionDefinitionLine = _db.tblt_TradePromotionDefinition.Where(x => x.promo_id == promoid && x.promo_line_type == 1);
                var tradePromotionFreeLine = _db.tblt_TradePromotionDefinition.SingleOrDefault(x => x.promo_id == promoid && x.promo_line_type == 2);

                foreach (var item in tradePromotionDefinitionLine)
                {
                    var dataLine = (from a in orederLine                        where a.sku_id == item.condition_sku_id && a.Betch_id == item.condition_sku_Batch
                        select new Tpvm
                        {
                            PromoId = promoid,
                            SkuId = a.sku_id,
                            BatchId = a.Betch_id,
                            QuantityOrdered = a.quantity_ordered / item.condition_sku_amount,
                            QuantityConfirmed = a.quantity_confirmed / item.condition_sku_amount,
                            QuantityDelivered = a.quantity_delivered / item.condition_sku_amount
                        }).SingleOrDefault();
                    if (dataLine != null)
                    {
                        slabCount.Add(dataLine);
                    }

                }


                var quantityOrderedslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityOrdered).Sum();
                var quantityConfirmedslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityConfirmed).Sum();
                var quantityDeliveredslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityDelivered).Sum();


                var orederLineids = (from a in orederLine
                    join b in slabCount on new { A = a.sku_id, B = a.Betch_id } equals new { A = b.SkuId, B = b.BatchId }
                    select a.id).ToList();

                if (tradePromotionFreeLine != null)
                {
                    int getfreequantityOrder = tradePromotionFreeLine.offer_sku_amount * quantityOrderedslabCount;
                    int getfreequantityDelivered = tradePromotionFreeLine.offer_sku_amount * quantityConfirmedslabCount;
                    int getfreequantityconfirmed = tradePromotionFreeLine.offer_sku_amount * quantityDeliveredslabCount;


                    if (getfreequantityOrder > 0 || getfreequantityDelivered > 0 || getfreequantityconfirmed > 0)
                    {

                        tblt_Order_line freeOrderLine = new tblt_Order_line
                        {
                            Orderid = orderid,
                            Bundelitmeid = 0,
                            sku_id = tradePromotionFreeLine.offer_sku_id,
                            Betch_id = tradePromotionFreeLine.offer_sku_Batch,
                            Pack_size = tradePromotionFreeLine.offer_sku_pack_size,
                            unit_sale_price = 0,
                            sku_order_type_id = 2,
                            promotion_id = tradePromotionFreeLine.promo_id,
                            lpec = 0,
                            quantity_ordered = getfreequantityOrder,
                            quantity_confirmed = getfreequantityconfirmed,
                            quantity_delivered = getfreequantityDelivered,
                            total_sale_price = 0,
                            total_discount_amount = 0,
                            total_billed_amount = 0,

                        };

                        TradepromotatonFreeOrderLine(freeOrderLine);



                       

                    }
                    _db.tblt_Order_line.Where(x => orederLineids.Contains(x.id)).ToList().ForEach(x =>
                    {
                        x.promotion_id = promoid;

                    }); //Update TP Order Line By Promo_id Update

                    _db.SaveChanges();

                }

            }
        }
        

        //Quentity && OR && Multi_Slub
        private void Quentity_OR_Multi_Slab(List<int> tpAffactedPromoid, List<tblt_Order_line> orederLine, int orderid)
        {


            foreach (var promoid in tpAffactedPromoid)
            {
                List<Tpvm> slabCount = new List<Tpvm>();

                var tradePromotionDefinitionLine = _db.tblt_TradePromotionDefinition.Where(x => x.promo_id == promoid && x.promo_line_type == 1);
                var tradePromotionFreeLine = _db.tblt_TradePromotionDefinition.SingleOrDefault(x => x.promo_id == promoid && x.promo_line_type == 2);

                foreach (var item in tradePromotionDefinitionLine)
                {
                    var dataLine = (from a in orederLine
                        where a.sku_id == item.condition_sku_id && a.Betch_id == item.condition_sku_Batch
                        select new Tpvm
                        {
                            PromoId = promoid,
                            SkuId = a.sku_id,
                            BatchId = a.Betch_id,
                            QuantityOrdered = a.quantity_ordered >=item.condition_sku_amount?a.quantity_ordered/item.condition_sku_pack_size:0,
                            QuantityConfirmed = a.quantity_confirmed >= item.condition_sku_amount ? a.quantity_confirmed / item.condition_sku_pack_size : 0,
                            QuantityDelivered = a.quantity_delivered >= item.condition_sku_amount ? a.quantity_delivered / item.condition_sku_pack_size : 0,
                        }).SingleOrDefault();
                    if (dataLine != null)
                    {
                        slabCount.Add(dataLine);
                    }

                }

                if (tradePromotionFreeLine != null)
                {
                var quantityOrderedslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityOrdered).Sum() / tradePromotionFreeLine.condition_bundle_qty_CS;
                var quantityConfirmedslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityConfirmed).Sum() / tradePromotionFreeLine.condition_bundle_qty_CS; 
                var quantityDeliveredslabCount = slabCount.Where(x => x.PromoId == promoid).Select(x => x.QuantityDelivered).Sum() / tradePromotionFreeLine.condition_bundle_qty_CS;


                var orederLineids = (from a in orederLine
                    join b in slabCount on new { A = a.sku_id, B = a.Betch_id } equals new { A = b.SkuId, B = b.BatchId }
                    select a.id).ToList();

                
                    int getfreequantityOrder = tradePromotionFreeLine.offer_sku_amount * quantityOrderedslabCount;
                    int getfreequantityDelivered = tradePromotionFreeLine.offer_sku_amount * quantityConfirmedslabCount;
                    int getfreequantityconfirmed = tradePromotionFreeLine.offer_sku_amount * quantityDeliveredslabCount;


                    if (getfreequantityOrder > 0 || getfreequantityDelivered > 0 || getfreequantityconfirmed > 0)
                    {

                        tblt_Order_line freeOrderLine = new tblt_Order_line
                        {
                            Orderid = orderid,
                            Bundelitmeid = 0,
                            sku_id = tradePromotionFreeLine.offer_sku_id,
                            Betch_id = tradePromotionFreeLine.offer_sku_Batch,
                            Pack_size = tradePromotionFreeLine.offer_sku_pack_size,
                            unit_sale_price = 0,
                            sku_order_type_id = 2,
                            promotion_id = tradePromotionFreeLine.promo_id,
                            lpec = 0,
                            quantity_ordered = getfreequantityOrder,
                            quantity_confirmed = getfreequantityconfirmed,
                            quantity_delivered = getfreequantityDelivered,
                            total_sale_price = 0,
                            total_discount_amount = 0,
                            total_billed_amount = 0,

                        };

                        TradepromotatonFreeOrderLine(freeOrderLine);

                    }
                    _db.tblt_Order_line.Where(x => orederLineids.Contains(x.id)).ToList().ForEach(x =>
                    {
                        x.promotion_id = promoid;

                    }); //Update TP Order Line By Promo_id Update

                    _db.SaveChanges();

                }

            }
        }



        private void TradepromotatonFreeOrderLine(tblt_Order_line freeOrderLine)
        {

            _db.tblt_Order_line.Add(freeOrderLine);
            _db.SaveChanges();
        }

       


    }
}