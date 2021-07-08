using Holism.Taxonomy.Business;
using Microsoft.AspNetCore.Mvc;
using Holism.Api.Controllers;

namespace Holism.Taxonomy.Api.Controllers
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
