/****** Object:  View [dbo].[vw_aspnet_UsersInRoles]    Script Date: 08/14/2012 21:59:26 ******/

CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
GO
