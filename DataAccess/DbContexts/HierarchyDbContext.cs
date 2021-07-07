using Holism.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holism.Taxonomy.DataAccess.DbContexts
{
    public class HierarchyDbContext : DbContext
    {
        string databaseName;

        public HierarchyDbContext()
            : base()
        {
        }

        public HierarchyDbContext(string databaseName)
            : base()
        {
            this.databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString(databaseName ?? Config.DatabaseName)).AddInterceptors(new PersianInterceptor());
        }

        public ICollection<Holism.Taxonomy.DataAccess.Models.Hierarchy> Hierarchies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Hierarchy>().ToTable("Hierarchies");
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Hierarchy>().Ignore(i => i.RelatedItems);
            base.OnModelCreating(modelBuilder);
        }
    }
}
