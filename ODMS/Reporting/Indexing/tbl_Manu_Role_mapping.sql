USE [ODMS]
GO

/****** Object:  Index [Indx_tbl_Manu_Role_mapping]    Script Date: 30-Jul-2018 5:51:23 PM ******/
DROP INDEX [Indx_tbl_Manu_Role_mapping] ON [dbo].[tbl_Manu_Role_mapping] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [Indx_tbl_Manu_Role_mapping]    Script Date: 30-Jul-2018 5:51:23 PM ******/
CREATE CLUSTERED INDEX [Indx_tbl_Manu_Role_mapping] ON [dbo].[tbl_Manu_Role_mapping]
(
	[Roleid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


