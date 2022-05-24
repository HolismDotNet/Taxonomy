namespace Taxonomy;

public class DashboardBusiness
{
    public object GetDashboardInfo()
    {
        dynamic info = new ExpandoObject();
        info.HierarchiesCount = Repository.Hierarchy.All.Count();
        info.CategorizedEntitiesCount = Repository.EntityHierarchy.All.Select(i => i.EntityGuid).Distinct().Count();
        info.HierarchiesShare = GetHierarchiesShare();
        info.TagsCount = Repository.Tag.All.Count();
        info.TaggedEntitiesCount = Repository.EntityTag.All.Select(i => i.EntityGuid).Distinct().Count();
        return info;
    }

    private object GetHierarchiesShare()
    {
        var entityHierarchys = Repository.EntityHierarchy.All.GroupBy(i => i.HierarchyId).ToDictionary(i => i.Key, i => i.Count());
        var totalEntities = entityHierarchys.Sum(i => i.Value);
        var hierarchies = Repository.Hierarchy.All.ToDictionary(i => i.Id, i => i.Title);
        var result = new List<dynamic>();
        foreach (var item in entityHierarchys)
        {
            dynamic temp = new ExpandoObject();
            temp.Hierarchy = hierarchies[item.Key];
            temp.EntitiesCount = item.Value;
            temp.Share = Math.Round((Convert.ToDecimal(item.Value) / Convert.ToDecimal(totalEntities)) * 100, 2);
            result.Add(temp);
        }
        result = result.OrderByDescending(i => i.Share).Take(10).ToList();
        return result;
    }
}
