using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class HierarchyItemNode
    {
        public long HierarchyId { get; set; }

        public string Title { get; set; }

        public string IconUrl { get; set; }

        public string IconSvg { get; set; }

        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }

        public List<HierarchyItemNode> Children { get; set; }

        public bool IsInThisHierarchy { get; set; }
    }
}
