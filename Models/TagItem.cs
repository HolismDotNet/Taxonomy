using System;

namespace Holism.Taxonomy.Models
{
    public class TagItem : Holism.Models.IEntity
    {
        public TagItem()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public long TagId { get; set; }

        public Guid EntityTypeGuid { get; set; }

        public Guid EntityGuid { get; set; }

        public int Order { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
