with CategoriesCte as
(
	select
		*,
		cast(Title as nvarchar(max)) as Hierarchy,
		0 as Step
	from Categories
	where ParentCategoryId is null
	union all
	select
		Categories.*,
		cast(CategoriesCte.Hierarchy + ', ' + Categories.Title as nvarchar(max)) as Hierarchy,
		CategoriesCte.Step + 1 as Step
	from Categories
	inner join CategoriesCte
	on Categories.ParentCategoryId = CategoriesCte.Id
)
select *
from CategoriesCte