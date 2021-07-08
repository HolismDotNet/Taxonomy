using Holism.Framework;
using Holism.Taxonomy.Business;
using Holism.Taxonomy.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Api.Controllers
{
    public class HierarchyController : DefaultController
    {
        [HttpGet]
        public List<Hierarchy> List(long? ParentId = null)
        {
            return new HierarchyBusiness().GetList(ParentId);
        }

        [HttpGet]
        public object Hierarchy()
        {
            return new HierarchyBusiness().CacheAndGetHierarchy();
        }

        [HttpGet]
        public object SimpleHierarchy()
        {
            return new HierarchyBusiness().GetSimpleHierarchy();
        }

        [HttpGet]
        public object Hierarchy(string entityType)
        {
            return ((HierarchyBusiness)Business).GetHierarchy(entityType);
        }

        [HttpPost]
        public Hierarchy CreateForEntityType(Hierarchy hierarchy, string entityType)
        {
            return ((HierarchyBusiness)Business).Create(entityType, hierarchy);
        }

        [FileUploadChecker]
        [HttpPost]
        public IActionResult ChangeIcon(IFormFile file)
        {
            var hierarchyId = HttpContext.Request.Query["hierarchyId"];
            if (hierarchyId.Count == 0)
            {
                throw new ClientException("Please provide hierarchyId");
            }
            var bytes = file.OpenReadStream().GetBytes();
            var iconUrl = ((HierarchyBusiness)Business).ChangeIcon(hierarchyId[0].ToInt(), bytes);
            return OkJson(data: iconUrl);
        }

        [HttpPost]
        public IActionResult RemoveIcon(long hierarchyId)
        {
            ((HierarchyBusiness)Business).RemoveIcon(hierarchyId);
            return OkJson();
        }

        [HttpPost]
        public IActionResult ChangeTitle(Hierarchy hierarchy)
        {
            ((HierarchyBusiness)Business).ChangeTitle(hierarchy.Id, hierarchy.Title);
            return OkJson();
        }

        [HttpPost]
        public IActionResult ChangeDescription(Hierarchy hierarchy)
        {
            ((HierarchyBusiness)Business).ChangeDescription(hierarchy.Id, hierarchy.Description);
            return OkJson();
        }

        [HttpPost]
        public IActionResult CountItemsInHierarchies()
        {
            ((HierarchyBusiness)Business).CountItemsInHierarchies();
            return OkJson();
        }

        [HttpGet]
        public IActionResult GetTotalCategorizedItemsCount(string entityModuleName, string entityType)
        {
            var count = ((HierarchyBusiness)Business).GetTotalCategorizedItemsCount(entityModuleName, entityType);
            return OkJson(null, count);
        }
    }
}
