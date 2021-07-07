SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE view [dbo].[TagItemViews] as
select 
	TagItems.Id,
	TagItems.EntityTypeGuid,
	TagItems.EntityGuid,
	TagItems.TagId,
	TagItems.[Order],
	Tags.[Name] as TagName
from TagItems
inner join Tags
on TagItems.TagId = Tags.Id 
GO
