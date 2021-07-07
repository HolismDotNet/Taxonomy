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
            info.CategoriesCount = RepositoryFactory.Category.All.Count();
            info.CategorizedItemsCount = RepositoryFactory.CategoryItem.All.Select(i => i.EntityGuid).Distinct().Count();
            info.CategoriesShare = GetCategoriesShare();
            info.TagsCount = RepositoryFactory.Tag.All.Count();
            info.TaggedItemsCount = RepositoryFactory.TagItem.All.Select(i => i.EntityGuid).Distinct().Count();
            return info;
        }

        private object GetCategoriesShare()
        {
            var categoryItems = RepositoryFactory.CategoryItem.All.GroupBy(i => i.CategoryId).ToDictionary(i => i.Key, i => i.Count());
            var totalItems = categoryItems.Sum(i => i.Value);
            var categories = RepositoryFactory.Category.All.ToDictionary(i => i.Id, i => i.Title);
            var result = new List<dynamic>();
            foreach (var item in categoryItems)
            {
                dynamic temp = new ExpandoObject();
                temp.Category = categories[item.Key];
                temp.ItemsCount = item.Value;
                temp.Share = Math.Round((Convert.ToDecimal(item.Value) / Convert.ToDecimal(totalItems)) * 100, 2);
                result.Add(temp);
            }
            result = result.OrderByDescending(i => i.Share).Take(10).ToList();
            return result;
        }
    }
}
