USE [ODMS]
GO

/****** Object:  Index [Indx_tbld_Target_PSR_Details]    Script Date: 30-Jul-2018 5:37:02 PM ******/
DROP INDEX [Indx_tbld_Target_PSR_Details] ON [dbo].[tbld_Target_PSR_Details] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tbld_Target_PSR_Details]    Script Date: 30-Jul-2018 5:37:02 PM ******/
CREATE CLUSTERED INDEX [Indx_tbld_Target_PSR_Details] ON [dbo].[tbld_Target_PSR_Details]
(
	[target_id] DESC,
	[db_id] ASC,
	[psr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


