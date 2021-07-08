with HierarchiesCte as
(
	select
		*,
		cast(Title as nvarchar(max)) as Hierarchy,
		0 as Step
	from Hierarchies
	where ParentId is null
	union all
	select
		Hierarchies.*,
		cast(HierarchiesCte.Hierarchy + ', ' + Hierarchies.Title as nvarchar(max)) as Hierarchy,
		HierarchiesCte.Step + 1 as Step
	from Hierarchies
	inner join HierarchiesCte
	on Hierarchies.ParentId = HierarchiesCte.Id
)
select *
from HierarchiesCte