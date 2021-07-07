SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE view [dbo].[CategoryItemViews] as
select 
	CategoryItems.Id,
	CategoryItems.EntityTypeGuid,
	CategoryItems.EntityGuid,
	CategoryItems.CategoryId,
	CategoryItems.[Order],
	Categories.Title as CategoryTitle
from CategoryItems
inner join Categories
on CategoryItems.CategoryId = Categories.Id 
GO
