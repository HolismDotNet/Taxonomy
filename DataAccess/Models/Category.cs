using System;

namespace Holism.Taxonomy.DataAccess.Models
{
    public class Category : Holism.EntityFramework.IEntity
    {
        public Category()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public Guid Guid { get; set; }

        public Guid EntityTypeGuid { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public Guid? IconGuid { get; set; }

        public string IconSvg { get; set; }

        public long? ParentCategoryId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public bool Show { get; set; }

        public int? ItemsCount { get; set; }

        public string UrlKey { get; set; }

        public int? Level { get; set; }

        public bool? IsLeaf { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
