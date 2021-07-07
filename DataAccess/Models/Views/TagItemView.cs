using System;

namespace Holism.Taxonomy.DataAccess.Models.Views
{
    public class TagItemView : Holism.EntityFramework.IEntity
    {
        public TagItemView()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public Guid EntityTypeGuid { get; set; }

        public Guid EntityGuid { get; set; }

        public long TagId { get; set; }

        public int Order { get; set; }

        public string TagName { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
