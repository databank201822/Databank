USE [ODMS]
GO

/****** Object:  Index [Indx_tblt_TradePromotionDefinition]    Script Date: 30-Jul-2018 6:13:58 PM ******/
DROP INDEX [Indx_tblt_TradePromotionDefinition] ON [dbo].[tblt_TradePromotionDefinition] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblt_TradePromotionDefinition]    Script Date: 30-Jul-2018 6:13:58 PM ******/
CREATE CLUSTERED INDEX [Indx_tblt_TradePromotionDefinition] ON [dbo].[tblt_TradePromotionDefinition]
(
	[promo_id] DESC,
	[condition_sku_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


