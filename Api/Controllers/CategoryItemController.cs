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
    public class CategoryItemController : Controller<CategoryItemView, CategoryItem>
    {
        public override Business<CategoryItemView, CategoryItem> Business => new CategoryItemBusiness(taxonomyDatabaseName, entityDatabaseName);

        public override ViewBusiness<CategoryItemView> ViewBusiness => new CategoryItemBusiness(taxonomyDatabaseName, entityDatabaseName);

        string taxonomyDatabaseName;

        string entityDatabaseName;

        public CategoryItemController(string taxonomyDatabaseName = null, string entityDatabaseName = null)
        {
            this.taxonomyDatabaseName = taxonomyDatabaseName;
            this.entityDatabaseName = entityDatabaseName;
        }

        [HttpGet]
        public List<CategoryItemNode> Categories(string entityType, Guid entityGuid)
        {
            return ((CategoryItemBusiness)ViewBusiness).GetItemCategories(entityType, entityGuid);
        }

        [HttpPost]
        public IActionResult ToggleCategory(string entityType, long categoryId, Guid entityGuid)
        {
            ((CategoryItemBusiness)Business).ToggleCategory(entityType, categoryId, entityGuid);
            return OkJson();
        }

        [HttpGet]
        public List<CategoryItemView> AllItems(long categoryId)
        {
            return ((CategoryItemBusiness)ViewBusiness).GetAllItems(categoryId);
        }

        [HttpPost]
        public IActionResult UpdateOrder(long categoryItemId, int newOrder)
        {
            ((CategoryItemBusiness)Business).UpdateOrder(categoryItemId, newOrder);
            return OkJson();
        }
    }
}
