SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE RPT_Order_PSRWiseSKUWiseOrder
	@Start_Date Datetime,
	@End_Date Datetime
AS
BEGIN
	SELECT A.DB_Id, A.DB_Name, A.CEAREA_id, A.CEAREA_Name, A.AREA_id, A.AREA_Name, A.REGION_id, A.REGION_Name, B.PSRId, B.PSRName, B.SKUId, B.SKUName, B.PackSize, 
                  B.SKUVolume8oz, B.UnitPrice,SUM( B.Order_Quentity) AS Order_Quentity, SUM(B.Confirmed_Quantity)AS Confirmed_Quantity, SUM(B.FreeOrder_Quentity)ASFreeOrder_Quentity, SUM(B.FreeConfirmed_Quantity) AS FreeConfirmed_Quantity
FROM     tbld_db_zone_view AS A INNER JOIN
                  tblr_PSRWiseSKUWiseOrder AS B ON A.DB_Id = B.DB_id
				  where A.Status=1 and  (B.BatchDate between @Start_Date AND @End_Date)
				  GROUP BY A.DB_Id, A.DB_Name, A.CEAREA_id, A.CEAREA_Name, A.AREA_id, A.AREA_Name, A.REGION_id, A.REGION_Name,B.PSRId, B.PSRName, B.SKUId, B.SKUName, B.PackSize, 
                  B.SKUVolume8oz, B.UnitPrice
END
GO
