USE [ODMS]
GO

/****** Object:  Index [Indx_tbld_distributor_Route]    Script Date: 30-Jul-2018 5:52:48 PM ******/
DROP INDEX [Indx_tbld_distributor_Route] ON [dbo].[tbld_distributor_Route] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tbld_distributor_Route]    Script Date: 30-Jul-2018 5:52:48 PM ******/
CREATE CLUSTERED INDEX [Indx_tbld_distributor_Route] ON [dbo].[tbld_distributor_Route]
(
	[RouteID] ASC,
	[DistributorID] ASC,
	[RouteType] ASC
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


