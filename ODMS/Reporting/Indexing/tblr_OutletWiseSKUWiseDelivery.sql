USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_OutletWiseSKUWiseDelivery]    Script Date: 30-Jul-2018 5:07:27 PM ******/
DROP INDEX [Indx_tblr_OutletWiseSKUWiseDelivery] ON [dbo].[tblr_OutletWiseSKUWiseDelivery] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_OutletWiseSKUWiseDelivery]    Script Date: 30-Jul-2018 5:07:27 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_OutletWiseSKUWiseDelivery] ON [dbo].[tblr_OutletWiseSKUWiseDelivery]
(
	[BatchDate] Desc,
	[OutletId] ASC,
	[Distributorid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


