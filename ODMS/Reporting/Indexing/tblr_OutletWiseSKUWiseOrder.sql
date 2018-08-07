USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_OutletWiseSKUWiseOrder]    Script Date: 30-Jul-2018 5:09:20 PM ******/
DROP INDEX [Indx_tblr_OutletWiseSKUWiseOrder] ON [dbo].[tblr_OutletWiseSKUWiseOrder] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_OutletWiseSKUWiseOrder]    Script Date: 30-Jul-2018 5:09:20 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_OutletWiseSKUWiseOrder] ON [dbo].[tblr_OutletWiseSKUWiseOrder]
(
	[BatchDate] Desc,
	[OutletId] ASC,
	[Distributorid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


