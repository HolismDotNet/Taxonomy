using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts.Views;

namespace Holism.Taxonomy.DataAccess.Repositories.Views
{
    public partial class HierarchyItemViewRepository : ViewRepository<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView>
    {
        public HierarchyItemViewRepository(string databaseName = null)
            : base(new HierarchyItemViewDbContext(databaseName))
        {
        }

        public  string ViewName
        {
            get
            {
                return "[HierarchyItemViews]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView, bool>> result = null;
            if (t.Id > 0)
            {
                result = i => i.Id == t.Id;
            }
            else
            {
                result = i => i.Id == t.Id;
            }
            return result;
        }

    }
}
