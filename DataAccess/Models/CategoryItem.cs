using System;

namespace Holism.Taxonomy.DataAccess.Models
{
    public class CategoryItem : Holism.EntityFramework.IEntity
    {
        public CategoryItem()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public long CategoryId { get; set; }

        public Guid EntityTypeGuid { get; set; }

        public Guid EntityGuid { get; set; }

        public int Order { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
