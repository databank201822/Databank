USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_PSRWiseSKUWiseOrder]    Script Date: 30-Jul-2018 5:33:40 PM ******/
DROP INDEX [Indx_tblr_PSRWiseSKUWiseOrder] ON [dbo].[tblr_PSRWiseSKUWiseOrder] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_PSRWiseSKUWiseOrder]    Script Date: 30-Jul-2018 5:33:40 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_PSRWiseSKUWiseOrder] ON [dbo].[tblr_PSRWiseSKUWiseOrder]
(
	
	[BatchDate] DESC,
	[DB_id] ASC,
	[PSRId] ASC,
	[SKUId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


