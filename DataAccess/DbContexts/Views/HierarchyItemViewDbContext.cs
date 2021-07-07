using Holism.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holism.Taxonomy.DataAccess.DbContexts.Views
{
    public class HierarchyItemViewDbContext : DbContext
    {
        string databaseName;

        public HierarchyItemViewDbContext()
            : base()
        {
        }

        public HierarchyItemViewDbContext(string databaseName)
            : base()
        {
            this.databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString(databaseName ?? Config.DatabaseName)).AddInterceptors(new PersianInterceptor());
        }

        public ICollection<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView> HierarchyItemViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView>().ToTable("HierarchyItemViews");
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView>().Ignore(i => i.RelatedItems);
			modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView>().HasKey(i => i.Id);
			modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.HierarchyItemView>().Property(i => i.Id).ValueGeneratedNever();
            base.OnModelCreating(modelBuilder);
        }
    }
}
