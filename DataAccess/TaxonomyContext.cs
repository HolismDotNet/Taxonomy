using System.Collections.Generic;
using Holism.Taxonomy.Models;
using Holism.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Holism.Taxonomy.DataAccess
{
    public class TaxonomyContext : DatabaseContext
    {
        public override string ConnectionStringName => "Taxonomy";

        public DbSet<Hierarchy> Hierarchies { get; set; }

        public DbSet<HierarchyItem> HierarchyItems { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TagItem> TagItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
