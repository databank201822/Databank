USE [ODMS]
GO

/****** Object:  Index [Indx_tblt_Challan]    Script Date: 30-Jul-2018 5:57:44 PM ******/
DROP INDEX [Indx_tblt_Challan] ON [dbo].[tblt_Challan]
GO

/****** Object:  Index [Indx_tblt_Challan]    Script Date: 30-Jul-2018 5:57:44 PM ******/
CREATE NONCLUSTERED INDEX [Indx_tblt_Challan] ON [dbo].[tblt_Challan]
(
	[order_date] DESC,
	[db_id] ASC,
	[challan_status] ASC
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


