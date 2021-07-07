using System;

namespace Holism.Taxonomy.DataAccess.Models.Views
{
    public class HierarchyItemView : Holism.EntityFramework.IEntity
    {
        public HierarchyItemView()
        {
            RelatedItems = new System.Dynamic.ExpandoObject();
        }

        public long Id { get; set; }

        public Guid EntityTypeGuid { get; set; }

        public Guid EntityGuid { get; set; }

        public long HierarchyId { get; set; }

        public int Order { get; set; }

        public string HierarchyTitle { get; set; }

        public dynamic RelatedItems { get; set; }
    }
}
