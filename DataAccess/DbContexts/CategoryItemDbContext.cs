using Holism.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Holism.Taxonomy.DataAccess.DbContexts
{
    public class CategoryItemDbContext : DbContext
    {
        string databaseName;

        public CategoryItemDbContext()
            : base()
        {
        }

        public CategoryItemDbContext(string databaseName)
            : base()
        {
            this.databaseName = databaseName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString(databaseName ?? Config.DatabaseName)).AddInterceptors(new PersianInterceptor());
        }

        public ICollection<Holism.Taxonomy.DataAccess.Models.CategoryItem> CategoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.CategoryItem>().ToTable("CategoryItems");
            modelBuilder.Entity<Holism.Taxonomy.DataAccess.Models.CategoryItem>().Ignore(i => i.RelatedItems);
            base.OnModelCreating(modelBuilder);
        }
    }
}
