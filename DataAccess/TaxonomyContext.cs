using Holism.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Holism.Taxonomy.Models;

namespace Holism.Taxonomy.DataAccess
{
    public class TaxonomyContext : DatabaseContext
    {
        public override string ConnectionStringName => "Taxonomy";

        public DbSet<Hierarchy> Hierarchies { get; set; }

        public DbSet<HierarchyItem> HierarchyItems { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TagItem> TagItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
