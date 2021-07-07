SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE view [dbo].[HierarchyItemViews] as
select 
	HierarchyItems.Id,
	HierarchyItems.EntityTypeGuid,
	HierarchyItems.EntityGuid,
	HierarchyItems.HierarchyId,
	HierarchyItems.[Order],
	Hierarchies.Title as HierarchyTitle
from HierarchyItems
inner join Hierarchies
on HierarchyItems.HierarchyId = Hierarchies.Id 
GO
