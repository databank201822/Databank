USE [ODMS]
GO

/****** Object:  Index [Indx_tbld_Route_Plan_Detail]    Script Date: 30-Jul-2018 5:39:11 PM ******/
DROP INDEX [Indx_tbld_Route_Plan_Detail] ON [dbo].[tbld_Route_Plan_Detail] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tbld_Route_Plan_Detail]    Script Date: 30-Jul-2018 5:39:11 PM ******/
CREATE CLUSTERED INDEX [Indx_tbld_Route_Plan_Detail] ON [dbo].[tbld_Route_Plan_Detail]
(
	[planned_visit_date] ASC,
	[dbid] ASC,	
	[db_emp_id] ASC,
	[route_id] ASC
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


