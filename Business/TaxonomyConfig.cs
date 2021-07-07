using Holism.Framework;
using Holism.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class TaxonomyConfig : Config
    {
        public static int CategoryThumbnailWidth
        {
            get
            {
                var thumbnailWidthKey = "CategoryThumbnailKey";
                if (HasSetting(thumbnailWidthKey) && GetSetting(thumbnailWidthKey).IsNumeric())
                {
                    return GetSetting(thumbnailWidthKey).ToInt();
                }
                return 100;
            }
        }
        public static int TagThumbnailWidth
        {
            get
            {
                var thumbnailWidthKey = "TagThumbnailWidth";
                if (HasSetting(thumbnailWidthKey) && GetSetting(thumbnailWidthKey).IsNumeric())
                {
                    return GetSetting(thumbnailWidthKey).ToInt();
                }
                return 100;
            }
        }
    }
}
