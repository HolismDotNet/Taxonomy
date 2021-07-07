using Holism.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holism.Taxonomy.DataAccess.DbContexts.Views
{
    public class TagItemViewDbContext : DbContext
    {
        string databaseName;

        public TagItemViewDbContext()
            : base()
        {
        }

        public TagItemViewDbContext(string databaseName)
            : base()
        {
            this.databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString(databaseName ?? Config.DatabaseName)).AddInterceptors(new PersianInterceptor());
        }

        public ICollection<Holism.Taxonomy.DataAccess.Models.Views.TagItemView> TagItemViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.TagItemView>().ToTable("TagItemViews");
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.TagItemView>().Ignore(i => i.RelatedItems);
			modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.TagItemView>().HasKey(i => i.Id);
			modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.Views.TagItemView>().Property(i => i.Id).ValueGeneratedNever();
            base.OnModelCreating(modelBuilder);
        }
    }
}
