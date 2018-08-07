USE [ODMS]
GO

/****** Object:  Index [Indx_tblr_PerformanceKPI]    Script Date: 30-Jul-2018 5:26:28 PM ******/
DROP INDEX [Indx_tblr_PerformanceKPI] ON [dbo].[tblr_PerformanceKPI] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tblr_PerformanceKPI]    Script Date: 30-Jul-2018 5:26:28 PM ******/
CREATE CLUSTERED INDEX [Indx_tblr_PerformanceKPI] ON [dbo].[tblr_PerformanceKPI]
(
	[BatchDate] DESC,
	[DB_id] ASC,
	[PerformerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


