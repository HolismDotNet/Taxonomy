using Holism.Api.Controllers;
using Holism.Business;
using Holism.Taxonomy.Business;
using Holism.Taxonomy.DataAccess.Models;
using Holism.Taxonomy.DataAccess.Models.Views;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Holism.Taxonomy.AdminApi.Controllers
{
    public class HierarchyItemController : Controller<HierarchyItemView, HierarchyItem>
    {
        public override Business<HierarchyItemView, HierarchyItem> Business => new HierarchyItemBusiness();

        public override ViewBusiness<HierarchyItemView> ViewBusiness => new HierarchyItemBusiness();

        string ;

        string ;

        public HierarchyItemController(string  = null, string  = null)
        {
            this. = ;
            this. = ;
        }

        [HttpGet]
        public List<HierarchyItemNode> Hierarchies(string entityType, Guid entityGuid)
        {
            return ((HierarchyItemBusiness)ViewBusiness).GetItemHierarchies(entityType, entityGuid);
        }

        [HttpPost]
        public IActionResult ToggleHierarchy(string entityType, long hierarchyId, Guid entityGuid)
        {
            ((HierarchyItemBusiness)Business).ToggleHierarchy(entityType, hierarchyId, entityGuid);
            return OkJson();
        }

        [HttpGet]
        public List<HierarchyItemView> AllItems(long hierarchyId)
        {
            return ((HierarchyItemBusiness)ViewBusiness).GetAllItems(hierarchyId);
        }

        [HttpPost]
        public IActionResult UpdateOrder(long hierarchyItemId, int newOrder)
        {
            ((HierarchyItemBusiness)Business).UpdateOrder(hierarchyItemId, newOrder);
            return OkJson();
        }
    }
}
