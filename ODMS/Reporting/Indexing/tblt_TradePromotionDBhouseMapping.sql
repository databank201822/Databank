USE [ODMS]
GO

/****** Object:  Index [Indx_tblt_TradePromotionDBhouseMapping]    Script Date: 30-Jul-2018 6:11:08 PM ******/
DROP INDEX [Indx_tblt_TradePromotionDBhouseMapping] ON [dbo].[tblt_TradePromotionDBhouseMapping] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblt_TradePromotionDBhouseMapping]    Script Date: 30-Jul-2018 6:11:08 PM ******/
CREATE CLUSTERED INDEX [Indx_tblt_TradePromotionDBhouseMapping] ON [dbo].[tblt_TradePromotionDBhouseMapping]
(  
	[db_id] ASC,
	[promo_id] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


