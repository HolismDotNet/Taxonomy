using Holism.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holism.Taxonomy.DataAccess.DbContexts
{
    public class HierarchyItemDbContext : DbContext
    {
        string databaseName;

        public HierarchyItemDbContext()
            : base()
        {
        }

        public HierarchyItemDbContext(string databaseName)
            : base()
        {
            this.databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString(databaseName ?? Config.DatabaseName)).AddInterceptors(new PersianInterceptor());
        }

        public ICollection<Holism.Taxonomy.DataAccess.Models.HierarchyItem> HierarchyItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.HierarchyItem>().ToTable("HierarchyItems");
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.HierarchyItem>().Ignore(i => i.RelatedItems);
            base.OnModelCreating(modelBuilder);
        }
    }
}
