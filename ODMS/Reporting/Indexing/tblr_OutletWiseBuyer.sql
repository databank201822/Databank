USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_OutletWiseBuyer]    Script Date: 30-Jul-2018 4:57:32 PM ******/
DROP INDEX [Indx_tblr_OutletWiseBuyer] ON [dbo].[tblr_OutletWiseBuyer] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_OutletWiseBuyer]    Script Date: 30-Jul-2018 4:57:32 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_OutletWiseBuyer] ON [dbo].[tblr_OutletWiseBuyer]
(  
	[BatchDate] Desc,
	[BatchDeliveryDate] Desc,
	[outlet_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


