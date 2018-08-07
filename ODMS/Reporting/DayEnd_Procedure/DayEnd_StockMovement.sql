USE [ODMS]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Alter PROCEDURE [dbo].[DayEnd_StockMovement]
	@Db_id int,
	@BatchDate Datetime
	
AS
BEGIN
DELETE FROM [dbo].[tblr_StockMovement]  where [tblr_StockMovement].[BatchDate]=@BatchDate AND [tblr_StockMovement].[dbId]= @Db_id;
	INSERT INTO [dbo].[tblr_StockMovement]
           ([BatchDate]
           ,[dbId]
           ,[SKUid]
           ,[BatchNo]
           ,[PackSize]
           ,[db_lifting_price]
           ,[outlet_lifting_price]
           ,[mrp]
           ,[ClosingSoundStockQty]
           ,[ClosingBookedStockQty]
           ,[PrimaryChallanQty]
           ,[PrimaryQty]
           ,[SalesQty]
           ,[FreeSalesQty])

		  SELECT  @BatchDate,C.dbId, A.sku_id As SKUid, A.batch_id As BatchNo , C.packSize As PackSize , A.db_lifting_price, A.outlet_lifting_price, A.mrp, C.qtyPs AS ClosingSoundStock, ISNULL(D.BookedStock, 0) AS ClosingBookedStock, 
                  ISNULL(E.ChallanQty,0) AS PrimaryChallanQty, ISNULL(E.ReciveQty,0) As PrimaryQty,ISNULL(F.Sales, 0) AS Sales,ISNULL(F.FreeSales, 0) AS FreeSales
FROM     tbld_bundle_price_details AS A INNER JOIN
                  tbld_distribution_house AS B ON A.bundle_price_id = B.PriceBuandle_id INNER JOIN
                  tblt_inventory AS C ON A.sku_id = C.skuId AND A.batch_id = C.batchNo LEFT OUTER JOIN
                      (SELECT t1.db_id, t2.sku_id, t2.batch_id, SUM(t2.Total_qty) AS BookedStock
                       FROM      tblt_Challan AS t1 INNER JOIN
                                         tblt_Challan_line AS t2 ON t1.id = t2.challan_id
                       WHERE   (t1.db_id = @Db_id) AND (t1.challan_status = 1)
                       GROUP BY t1.db_id, t2.sku_id, t2.batch_id) AS D ON D.db_id = C.dbId AND D.sku_id = A.sku_id AND D.batch_id = C.batchNo LEFT OUTER JOIN
                      (SELECT a1.DbId, a1.ReceivedDate, a2.sku_id, a2.BatchId, SUM(a2.ChallanQty) AS ChallanQty, SUM(a2.ReciveQty) AS ReciveQty
                       FROM      tblt_PurchaseOrder AS a1 INNER JOIN
                                         tblt_PurchaseOrderLine AS a2 ON a1.Id = a2.POId
                       WHERE   (a1.ReceivedDate = @BatchDate) AND a1.DbId=@Db_id
                       GROUP BY a1.DbId, a1.ReceivedDate, a2.sku_id, a2.BatchId) AS E ON E.DbId = C.dbId AND E.sku_id = A.sku_id AND E.BatchId = C.batchNo
					   LEFT OUTER JOIN
                      (SELECT t1.db_id, t2.sku_id, t2.batch_id, SUM(t2.Confirm_qty) AS Sales,SUM(t2.Confirm_Free_qty) AS FreeSales
                       FROM      tblt_Challan AS t1 INNER JOIN
                                         tblt_Challan_line AS t2 ON t1.id = t2.challan_id
                       WHERE   (t1.db_id = @Db_id) AND (t1.challan_status = 2) AND  t1.delivery_date=@BatchDate
                       GROUP BY t1.db_id, t2.sku_id, t2.batch_id) AS F ON F.db_id = C.dbId AND F.sku_id = A.sku_id AND F.batch_id = C.batchNo 
WHERE  (B.DB_Id = @Db_id)
END

GO


