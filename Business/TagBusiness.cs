﻿using Holism.Azure;
using Holism.Business;
using Holism.Entity.Business;
using Holism.DataAccess;
using Holism.Framework;
using Holism.Taxonomy.DataAccess;
using Holism.Taxonomy.Models;
using Holism.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holism.Taxonomy.Business
{
    public class TagBusiness : Business<Tag, Tag>
    {
        public const string TagIconsContainerName = "tagicons";

        protected override Repository<Tag> WriteRepository => Repository.Tag;

        protected override ReadRepository<Tag> ReadRepository => Repository.Tag;

        public Tag Create(string entityType, Tag tag)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
            tag.EntityTypeGuid = entityTypeGuid;
            tag.Guid = Guid.NewGuid();
            return Create(tag);
        }

        public void CountItemsInTags()
        {
            var tags = GetAll();
            foreach (var tag in tags)
            {
                tag.ItemsCount = new TagItemBusiness().GetCountOfItemsInTag(tag);
            }
            WriteRepository.BulkUpdate(tags);
        }

        public int GetTotalTaggedItemsCount(string entityType)
        {
            var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
            CountItemsInTags();
            var count = ReadRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid).Sum(i => i.ItemsCount) ?? 0;
            return count;
        }

        public void CountItemsInTag(long tagId)
        {
            var tag = Get(tagId);
            tag.ItemsCount = new TagItemBusiness().GetCountOfItemsInTag(tag);
            Update(tag);
        }

        public Tag GetTag(string name)
        {
            var tag = GetOrNull(i => i.Name == name);
            return tag;
        }

        protected override void ModifyItemBeforeReturning(Tag item)
        {
            item.RelatedItems.IconUrl = GetIconUrl(item);
            item.RelatedItems.HasDefaultIcon = !item.IconGuid.HasValue;
            base.ModifyItemBeforeReturning(item);
        }

        private dynamic GetIconUrl(Tag tag)
        {
            var iconUrl = Storage.GetImageUrl(TagIconsContainerName, tag.IconGuid.HasValue ? tag.IconGuid.Value : Guid.Empty);
            return iconUrl;
        }

        public void ChangeName(long id, string name)
        {
            var tag = Get(id);
            tag.Name = name;
            WriteRepository.Update(tag);
        }

        public void ChangeDescription(long id, string description)
        {
            var tag = Get(id);
            tag.Description = description;
            WriteRepository.Update(tag);
        }

        public string ChangeIcon(long tagId, byte[] bytes)
        {
            var tag = Get(tagId);
            if (tag.IconGuid.HasValue)
            {
                Storage.DeleteImage(TagIconsContainerName, tag.IconGuid.Value);
            }
            var thumbnail = ImageHelper.MakeImageThumbnail(TaxonomyConfig.TagThumbnailWidth, null, bytes);
            tag.IconGuid = Guid.NewGuid();
            Storage.UploadImage(thumbnail.GetBytes(), tag.IconGuid.Value, TagIconsContainerName);
            WriteRepository.Update(tag);
            return Storage.GetImageUrl(TagIconsContainerName, tag.IconGuid.Value);
        }

        public void RemoveIcon(long tagId)
        {
            var tag = Get(tagId);
            if (tag.IconGuid.HasValue)
            {
                Storage.DeleteImage(TagIconsContainerName, tag.IconGuid.Value);
            }
            tag.IconGuid = null;
            WriteRepository.Update(tag);
        }

        public override void Validate(Tag model)
        {
            model.Name.Ensure().IsSomething("نام برچسب فراهم نشده است");
            model.EntityTypeGuid.Ensure().IsNotNull().And().IsNotEmptyGuid();
        }

        protected override void BeforeCreation(Tag model, object extraParameters = null)
        {
            model.Guid = Guid.NewGuid();
            model.Show = true;
        }

        public Tag GetByName(string name)
        {
            var tags = GetList(i => i.Name == name);
            if (tags.Count > 1)
            {
                throw new ClientException($"بیش از یک برچسب با نام {name} یافت شد");
            }
            return tags.FirstOrDefault();
        }

        public Tag GetOrCreateAndGet(string entityType, string name)
        {
            var tags = GetList(i => i.Name == name);
            if (tags.Count > 1)
            {
                throw new ClientException($"بیش از یک برچسب با نام {name} یافت شد");
            }
            if (tags.Count == 1)
            {
                return tags.FirstOrDefault();
            }
            var tag = new Tag();
            tag.Name = name;
            tag.UrlKey = name.ToLowercaseDashSeparatedWords();
            Create(entityType, tag);
            return tag;
        }
    }
}
