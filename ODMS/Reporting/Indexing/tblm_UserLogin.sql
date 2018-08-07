USE [ODMS]
GO

/****** Object:  Index [ClusteredIndex-20180731-124949]    Script Date: 31-Jul-2018 12:50:15 PM ******/
DROP INDEX [Indx_tblm_UserLogin] ON [dbo].[tblm_UserLogin] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [ClusteredIndex-20180731-124949]    Script Date: 31-Jul-2018 12:50:15 PM ******/
CREATE CLUSTERED INDEX [Indx_tblm_UserLogin] ON [dbo].[tblm_UserLogin]
(
	[id] ASC,
	[Date] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


