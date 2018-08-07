USE [ODMS]
GO

/****** Object:  Index [Indx_tblt_Challan_line]    Script Date: 30-Jul-2018 5:54:36 PM ******/
DROP INDEX [Indx_tblt_Challan_line] ON [dbo].[tblt_Challan_line] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblt_Challan_line]    Script Date: 30-Jul-2018 5:54:36 PM ******/
CREATE CLUSTERED INDEX [Indx_tblt_Challan_line] ON [dbo].[tblt_Challan_line]
(
	[challan_id] DESC,
	[sku_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


