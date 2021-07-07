using System;

namespace Holism.Taxonomy.DataAccess.Models
{
    public class TagItem : Holism.EntityFramework.IEntity
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
