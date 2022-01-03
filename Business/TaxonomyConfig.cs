using Holism.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Business;

public class TaxonomyConfig : InfraConfig
{
    public static int HierarchyThumbnailWidth
    {
        get
        {
            var thumbnailWidthKey = "HierarchyThumbnailKey";
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
