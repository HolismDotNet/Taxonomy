using Holism.Framework;
using Holism.Business;
using Holism.Taxonomy.Business;
using Holism.Taxonomy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Holism.Api.Controllers;

namespace Holism.Taxonomy.Api.Controllers
{
    public class HierarchyController : ReadController<Hierarchy>
    {

        public override ReadBusiness<Hierarchy> ReadBusiness => new HierarchyBusiness();

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
            return new HierarchyBusiness().GetHierarchy(entityType);
        }

        [HttpPost]
        public Hierarchy CreateForEntityType(Hierarchy hierarchy, string entityType)
        {
            return new HierarchyBusiness().Create(entityType, hierarchy);
        }

        // [FileUploadChecker]
        [HttpPost]
        public IActionResult ChangeIcon(IFormFile file)
        {
            var hierarchyId = HttpContext.Request.Query["hierarchyId"];
            if (hierarchyId.Count == 0)
            {
                throw new ClientException("Please provide hierarchyId");
            }
            var bytes = file.OpenReadStream().GetBytes();
            var iconUrl = new HierarchyBusiness().ChangeIcon(hierarchyId[0].ToInt(), bytes);
            return OkJson(data: iconUrl);
        }

        [HttpPost]
        public IActionResult RemoveIcon(long hierarchyId)
        {
            new HierarchyBusiness().RemoveIcon(hierarchyId);
            return OkJson();
        }

        [HttpPost]
        public IActionResult ChangeTitle(Hierarchy hierarchy)
        {
            new HierarchyBusiness().ChangeTitle(hierarchy.Id, hierarchy.Title);
            return OkJson();
        }

        [HttpPost]
        public IActionResult ChangeDescription(Hierarchy hierarchy)
        {
            new HierarchyBusiness().ChangeDescription(hierarchy.Id, hierarchy.Description);
            return OkJson();
        }

        [HttpPost]
        public IActionResult CountItemsInHierarchies()
        {
            new HierarchyBusiness().CountItemsInHierarchies();
            return OkJson();
        }

        // [HttpGet]
        // public IActionResult GetTotalCategorizedItemsCount(string entityModuleName, string entityType)
        // {
        //     var count = new HierarchyBusiness().GetTotalCategorizedItemsCount(entityModuleName, entityType);
        //     return OkJson(null, count);
        // }
    }
}
