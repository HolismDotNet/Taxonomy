using Holism.EntityFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq.Expressions;
using Holism.Taxonomy.DataAccess.DbContexts.Views;

namespace Holism.Taxonomy.DataAccess.Repositories.Views
{
    public partial class TagItemViewRepository : ViewRepository<Holism.Taxonomy.DataAccess.Models.Views.TagItemView>
    {
        public TagItemViewRepository(string databaseName = null)
            : base(new TagItemViewDbContext(databaseName))
        {
        }

        public  string ViewName
        {
            get
            {
                return "[TagItemViews]";
            }
        }

        public override Expression<Func<Holism.Taxonomy.DataAccess.Models.Views.TagItemView, bool>> ExistenceFilter(Holism.Taxonomy.DataAccess.Models.Views.TagItemView t)
        {
            Expression<Func<Holism.Taxonomy.DataAccess.Models.Views.TagItemView, bool>> result = null;
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
