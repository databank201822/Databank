USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_PSRWiseSKUWiseDelivery]    Script Date: 30-Jul-2018 5:31:22 PM ******/
DROP INDEX [Indx_tblr_PSRWiseSKUWiseDelivery] ON [dbo].[tblr_PSRWiseSKUWiseDelivery] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_PSRWiseSKUWiseDelivery]    Script Date: 30-Jul-2018 5:31:22 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_PSRWiseSKUWiseDelivery] ON [dbo].[tblr_PSRWiseSKUWiseDelivery]
(
		[BatchDate] DESC,
	[DB_id] DESC,
	[PSRId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


