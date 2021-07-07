using Holism.Taxonomy.Business;
using Microsoft.AspNetCore.Mvc;

namespace Holism.Taxonomy.AdminApi.Controllers
{
    public class TaxonomyController : DefaultController
    {
        [HttpGet]
        public object DashboardInfo()
        {
            return new DashboardBusiness().GetDashboardInfo();
        }
    }
}
