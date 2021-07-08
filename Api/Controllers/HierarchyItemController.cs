using Holism.Api.Controllers;
using Holism.Business;
using Holism.Taxonomy.Business;
using Holism.Taxonomy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Holism.Taxonomy.Api.Controllers
{
    public class HierarchyItemController : Controller<HierarchyItem, HierarchyItem>
    {
        public override Business<HierarchyItem, HierarchyItem> Business => new HierarchyItemBusiness();

        public override ReadBusiness<HierarchyItem> ReadBusiness => new HierarchyItemBusiness();

        [HttpGet]
        public List<HierarchyItemNode> Hierarchies(string entityType, Guid entityGuid)
        {
            return ((HierarchyItemBusiness)ReadBusiness).GetItemHierarchies(entityType, entityGuid);
        }

        [HttpPost]
        public IActionResult ToggleHierarchy(string entityType, long hierarchyId, Guid entityGuid)
        {
            ((HierarchyItemBusiness)Business).ToggleHierarchy(entityType, hierarchyId, entityGuid);
            return OkJson();
        }

        [HttpGet]
        public List<HierarchyItem> AllItems(long hierarchyId)
        {
            return ((HierarchyItemBusiness)ReadBusiness).GetAllItems(hierarchyId);
        }

        [HttpPost]
        public IActionResult UpdateOrder(long hierarchyItemId, int newOrder)
        {
            ((HierarchyItemBusiness)Business).UpdateOrder(hierarchyItemId, newOrder);
            return OkJson();
        }
    }
}
