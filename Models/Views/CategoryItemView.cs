using System;

namespace Holism.Taxonomy.Models.Views
{
    public class CategoryItemView : Holism.Models.IEntity
    {
        public CategoryItemView()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public Guid EntityTypeGuid { get; set; }

        public Guid EntityGuid { get; set; }

        public long CategoryId { get; set; }

        public int Order { get; set; }

        public string CategoryTitle { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
