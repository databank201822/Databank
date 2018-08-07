USE [ODMS]
GO

/****** Object:  Index [Indx_tblt_Order]    Script Date: 30-Jul-2018 6:08:20 PM ******/
DROP INDEX [Indx_tblt_Order] ON [dbo].[tblt_Order]
GO

/****** Object:  Index [Indx_tblt_Order]    Script Date: 30-Jul-2018 6:08:20 PM ******/
CREATE NONCLUSTERED INDEX [Indx_tblt_Order] ON [dbo].[tblt_Order]
(
	[planned_order_date] DESC,
	[delivery_date] DESC,
	[db_id] ASC,
	[psr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

ALTER INDEX [Indx_tblt_Order] ON [dbo].[tblt_Order] DISABLE
GO


