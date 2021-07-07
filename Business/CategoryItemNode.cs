using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class CategoryItemNode
    {
        public long CategoryId { get; set; }

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

        public List<CategoryItemNode> Children { get; set; }

        public bool IsInThisCategory { get; set; }
    }
}
