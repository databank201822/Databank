USE [ODMS]
GO

/****** Object:  Index [Indx_tbld_distribution_employee]    Script Date: 30-Jul-2018 5:49:04 PM ******/
DROP INDEX [Indx_tbld_distribution_employee] ON [dbo].[tbld_distribution_employee] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tbld_distribution_employee]    Script Date: 30-Jul-2018 5:49:04 PM ******/
CREATE CLUSTERED INDEX [Indx_tbld_distribution_employee] ON [dbo].[tbld_distribution_employee]
(
		[DistributionId] ASC,
		[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


