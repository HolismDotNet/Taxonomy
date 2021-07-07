using Holism.Taxonomy.DataAccess;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class DashboardBusiness
    {
        public object GetDashboardInfo()
        {
            dynamic info = new ExpandoObject();
            info.HierarchiesCount = RepositoryFactory.Hierarchy.All.Count();
            info.CategorizedItemsCount = RepositoryFactory.HierarchyItem.All.Select(i => i.EntityGuid).Distinct().Count();
            info.HierarchiesShare = GetHierarchiesShare();
            info.TagsCount = RepositoryFactory.Tag.All.Count();
            info.TaggedItemsCount = RepositoryFactory.TagItem.All.Select(i => i.EntityGuid).Distinct().Count();
            return info;
        }

        private object GetHierarchiesShare()
        {
            var hierarchyItems = RepositoryFactory.HierarchyItem.All.GroupBy(i => i.HierarchyId).ToDictionary(i => i.Key, i => i.Count());
            var totalItems = hierarchyItems.Sum(i => i.Value);
            var hierarchies = RepositoryFactory.Hierarchy.All.ToDictionary(i => i.Id, i => i.Title);
            var result = new List<dynamic>();
            foreach (var item in hierarchyItems)
            {
                dynamic temp = new ExpandoObject();
                temp.Hierarchy = hierarchies[item.Key];
                temp.ItemsCount = item.Value;
                temp.Share = Math.Round((Convert.ToDecimal(item.Value) / Convert.ToDecimal(totalItems)) * 100, 2);
                result.Add(temp);
            }
            result = result.OrderByDescending(i => i.Share).Take(10).ToList();
            return result;
        }
    }
}
