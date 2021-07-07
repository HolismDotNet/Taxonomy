using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts.Views;

namespace Holism.Taxonomy.DataAccess.Repositories.Views
{
    public partial class CategoryItemViewRepository : ViewRepository<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView>
    {
        public CategoryItemViewRepository(string databaseName = null)
            : base(new CategoryItemViewDbContext(databaseName))
        {
        }

        public  string ViewName
        {
            get
            {
                return "[CategoryItemViews]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView, bool>> result = null;
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
