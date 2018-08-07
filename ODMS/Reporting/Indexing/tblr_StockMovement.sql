USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_StockMovement]    Script Date: 30-Jul-2018 6:01:46 PM ******/
DROP INDEX [Indx_tblr_StockMovement] ON [dbo].[tblr_StockMovement] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_StockMovement]    Script Date: 30-Jul-2018 6:01:46 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_StockMovement] ON [dbo].[tblr_StockMovement]
(
	[BatchDate] DESC,
	[dbId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


