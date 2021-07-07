using Holism.Framework;
using Holism.Taxonomy.Business;
using Holism.Taxonomy.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Holism.Taxonomy.Api.Controllers
{
    public class CategoryController : DefaultController
    {
        [HttpGet]
        public List<Category> List(long? parentCategoryId = null)
        {
            return new CategoryBusiness().GetList(parentCategoryId);
        }

        [HttpGet]
        public object Hierarchy()
        {
            return new CategoryBusiness().CacheAndGetHierarchy();
        }

        [HttpGet]
        public object SimpleHierarchy()
        {
            return new CategoryBusiness().GetSimpleHierarchy();
        }

        [HttpGet]
        public object Hierarchy(string entityType)
        {
            return ((CategoryBusiness)Business).GetHierarchy(entityType);
        }

        [HttpPost]
        public Category CreateForEntityType(Category category, string entityType)
        {
            return ((CategoryBusiness)Business).Create(entityType, category);
        }

        [FileUploadChecker]
        [HttpPost]
        public IActionResult ChangeIcon(IFormFile file)
        {
            var categoryId = HttpContext.Request.Query["categoryId"];
            if (categoryId.Count == 0)
            {
                throw new BusinessException("Please provide categoryId");
            }
            var bytes = file.OpenReadStream().GetBytes();
            var iconUrl = ((CategoryBusiness)Business).ChangeIcon(categoryId[0].ToInt(), bytes);
            return OkJson(data: iconUrl);
        }

        [HttpPost]
        public IActionResult RemoveIcon(long categoryId)
        {
            ((CategoryBusiness)Business).RemoveIcon(categoryId);
            return OkJson();
        }

        [HttpPost]
        public IActionResult ChangeTitle(Category category)
        {
            ((CategoryBusiness)Business).ChangeTitle(category.Id, category.Title);
            return OkJson();
        }

        [HttpPost]
        public IActionResult ChangeDescription(Category category)
        {
            ((CategoryBusiness)Business).ChangeDescription(category.Id, category.Description);
            return OkJson();
        }

        [HttpPost]
        public IActionResult CountItemsInCategories()
        {
            ((CategoryBusiness)Business).CountItemsInCategories();
            return OkJson();
        }

        [HttpGet]
        public IActionResult GetTotalCategorizedItemsCount(string entityModuleName, string entityType)
        {
            var count = ((CategoryBusiness)Business).GetTotalCategorizedItemsCount(entityModuleName, entityType);
            return OkJson(null, count);
        }
    }
}
