using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class SimpleHierarchyNode
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long? ParentId { get; set; }

        public List<SimpleHierarchyNode> Children { get; set; }
    }
}
