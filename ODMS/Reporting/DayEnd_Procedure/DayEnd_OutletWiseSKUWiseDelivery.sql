USE [ODMS]
GO
/****** Object:  StoredProcedure [dbo].[DayEnd_OutletWiseSKUWiseDelivery]    Script Date: 10-Jul-18 3:33:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[DayEnd_OutletWiseSKUWiseDelivery]
	@Db_id int,
	@BatchDate Datetime
	
AS
BEGIN
DELETE FROM [dbo].[tblr_OutletWiseSKUWiseDelivery]  where [tblr_OutletWiseSKUWiseDelivery].BatchDate=@BatchDate AND [tblr_OutletWiseSKUWiseDelivery].Distributorid= @Db_id;
INSERT INTO [dbo].[tblr_OutletWiseSKUWiseDelivery]
           ([BatchDate]
           ,[OutletId]
           ,[OutletCode]
           ,[OutletName]
           ,[Distributorid]
           ,[HaveVisicooler]
           ,[SKUId]
           ,[SKUName]
           ,[PackSize]
           ,[UnitPrice]
           ,[SKUVolume8oz]
           ,[Delivered_Quentity]
           ,[FreeDelivered_Quentity])
SELECT 
@BatchDate                       AS BatchDate
,A.OutletId
      ,A.OutletCode
      ,A.OutletName 
      ,A.Distributorid
      ,A.HaveVisicooler
	  ,B.sku_id AS SKUId
	  ,B.SKUName
	  ,B.PackSize AS PackSize
	  ,Isnull(C.UnitPrice, 0)         AS UnitPrice
	  ,B.SKUVolume_8oz As SKUVolume8oz
	  ,Isnull(C.Delivered_Quentity, 0)         AS Delivered_Quentity,       
       Isnull(D.FreeDelivered_Quentity, 0)     AS FreeDelivered_Quentity
       
  FROM [ODMS].[dbo].[tbld_Outlet] as A

   INNER JOIN (SELECT DISTINCT t1.db_id
		,t1.outlet_id
		,t2.sku_id
		,t3.SKUName
		,t2.Pack_size as PackSize,
		 t3.SKUVolume_8oz
	FROM tblt_Order AS t1
	INNER JOIN tblt_Order_line AS t2 ON t1.Orderid = t2.Orderid
	LEFT JOIN tbld_SKU AS t3 ON t2.sku_id = t3.SKU_id
                   WHERE  t1.delivery_date = @BatchDate ) AS B 
               ON A.Distributorid = B.db_id 
                  AND A.OutletId = B.outlet_id

				  Left Join (SELECT t1.db_id, t1.outlet_id, t2.sku_id,t2.unit_sale_price as UnitPrice, sum(t2.quantity_delivered) AS Delivered_Quentity
FROM     tblt_Order as t1 INNER JOIN
                  tblt_Order_line as t2 ON t1.Orderid = t2.Orderid
				  Left Join tbld_SKU as t3 on t2.sku_id=t3.SKU_id
				  where  t1.delivery_date=@BatchDate and  t2.sku_order_type_id=1
				  Group by t1.db_id,t1.outlet_id,t3.SKUName, t2.sku_id,t2.Pack_size,t2.unit_sale_price) As C on A.Distributorid=C.db_id AND A.OutletId=C.outlet_id AND B.sku_id=C.sku_id
				   Left Join (SELECT t1.db_id, t1.outlet_id, t2.sku_id,t3.SKUName,t2.Pack_size,sum(t2.quantity_delivered) AS FreeDelivered_Quentity
FROM     tblt_Order as t1 INNER JOIN
                  tblt_Order_line as t2 ON t1.Orderid = t2.Orderid
				  Left Join tbld_SKU as t3 on t2.sku_id=t3.SKU_id
				  where  t1.delivery_date=@BatchDate and  t2.sku_order_type_id=2
				  Group by t1.db_id,t1.outlet_id,t3.SKUName, t2.sku_id,t2.Pack_size) As D on A.Distributorid=D.db_id AND A.OutletId=D.outlet_id AND B.sku_id=D.sku_id
  where A.Distributorid=@Db_id
  
  

	
END
