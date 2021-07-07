using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class SimpleCategoryNode
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long? ParentCategoryId { get; set; }

        public List<SimpleCategoryNode> Children { get; set; }
    }
}
