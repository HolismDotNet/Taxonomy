using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holism.Taxonomy.DataAccess
{
    public class Config : Holism.Framework.Config
    {
        public static string DatabaseName
        {
            get
            {
                return GetSetting("TaxonomyDatabaseName");
            }
        }
    }
}