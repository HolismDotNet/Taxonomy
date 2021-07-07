using Holism.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holism.Taxonomy.DataAccess.DbContexts.Views
{
    public class CategoryItemViewDbContext : DbContext
    {
        string databaseName;

        public CategoryItemViewDbContext()
            : base()
        {
        }

        public CategoryItemViewDbContext(string databaseName)
            : base()
        {
            this.databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString(databaseName ?? Config.DatabaseName)).AddInterceptors(new PersianInterceptor());
        }

        public ICollection<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView> CategoryItemViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView>().ToTable("CategoryItemViews");
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView>().Ignore(i => i.RelatedItems);
			modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView>().HasKey(i => i.Id);
			modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.CategoryItemView>().Property(i => i.Id).ValueGeneratedNever();
            base.OnModelCreating(modelBuilder);
        }
    }
}
