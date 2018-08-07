USE [ODMS]
GO

/****** Object:  Index [Indx_tbld_Outlet]    Script Date: 30-Jul-2018 5:43:38 PM ******/
DROP INDEX [Indx_tbld_Outlet] ON [dbo].[tbld_Outlet] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tbld_Outlet]    Script Date: 30-Jul-2018 5:43:38 PM ******/
CREATE CLUSTERED INDEX [Indx_tbld_Outlet] ON [dbo].[tbld_Outlet]
(
	[Distributorid] ASC,
	[parentid] ASC,
	[OutletId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


