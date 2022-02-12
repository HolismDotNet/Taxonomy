// namespace Taxonomy;

// public class HierarchyItemBusiness : Business<HierarchyItem, HierarchyItem>
// {
//     protected override Write<HierarchyItem> Write => Repository.HierarchyItem;

//     protected override Read<HierarchyItem> Read => Repository.HierarchyItem;

//     private static Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>> entitiesInfoAugmenter = new Dictionary<Guid, Func<List<Guid>, Dictionary<Guid, object>>>();

//     public static Action<HierarchyItem> OnHierarchyToggled;

//     public void RegisterEnittyInfoAugmenter(string entityType, Func<List<Guid>, Dictionary<Guid, object>> augmenter)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         if (entitiesInfoAugmenter.ContainsKey(entityTypeGuid))
//         {
//             entitiesInfoAugmenter[entityTypeGuid] = augmenter;
//         }
//         else
//         {
//             entitiesInfoAugmenter.Add(entityTypeGuid, augmenter);
//         }
//     }

//     public void UpdateOrder(long hierarchyItemId, int newOrder)
//     {
//         var hierarchyItem = Write.Get(hierarchyItemId);
//         hierarchyItem.Order = newOrder;
//         Update(hierarchyItem);
//     }

//     public List<HierarchyItemNode> GetItemHierarchies(string entityType, Guid entityGuid)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         var hierarchyIds = Read.All.Where(i => i.EntityGuid == entityGuid).Select(i => i.HierarchyId).ToList();
//         var hierarchies = new HierarchyBusiness().GetHierarchy(entityType);
//         var hierarchyItems = new List<HierarchyItemNode>();
//         foreach (var hierarchy in hierarchies)
//         {
//             var hierarchyItem = new HierarchyItemNode();
//             hierarchyItem.Children = new List<HierarchyItemNode>();
//             hierarchyItem.IconUrl = hierarchy.IconUrl;
//             hierarchyItem.IconSvg = hierarchy.IconSvg;
//             hierarchyItem.HierarchyId = hierarchy.Id;
//             hierarchyItem.Title = hierarchy.Title;
//             if (hierarchyIds.Contains(hierarchyItem.HierarchyId))
//             {
//                 hierarchyItem.IsInThisHierarchy = true;
//             }
//             hierarchyItems.Add(hierarchyItem);
//             GetChildrenHierarchyItems(hierarchyIds, hierarchy, hierarchyItem);
//         }
//         return hierarchyItems;
//     }

//     public int GetCountOfItemsInHierarchy(Hierarchy hierarchy)
//     {
//         var count = Read.All.Count(i => i.HierarchyId == hierarchy.Id);
//         return count;
//     }

//     public List<HierarchyItem> GetAllItems(long hierarchyId)
//     {
//         var hierarchy = new HierarchyBusiness().Get(hierarchyId);
//         var allItems = Read.All.Where(i => i.HierarchyId == hierarchyId).OrderBy(i => i.Order).ThenBy(i => i.Id).ToList();
//         if (entitiesInfoAugmenter.ContainsKey(hierarchy.EntityTypeGuid))
//         {
//             var entityGuids = allItems.Select(i => i.EntityGuid).ToList();
//             var entityInfoList = entitiesInfoAugmenter[hierarchy.EntityTypeGuid](entityGuids);
//             var hierarchyItemsWithEntityInfo = allItems.Where(i => entityInfoList.ContainsKey(i.EntityGuid)).ToList();
//             foreach (var hierarchyItem in hierarchyItemsWithEntityInfo)
//             {
//                 ExpandoObjectExtensions.AddProperty(hierarchyItem.RelatedItems, new EntityTypeBusiness().GetName(hierarchy.EntityTypeGuid), entityInfoList[hierarchyItem.EntityGuid]);
//             }
//         }
//         return allItems;
//     }

//     private void GetChildrenHierarchyItems(List<long> hierarchyIds, HierarchyNode hierarchy, HierarchyItemNode hierarchyItem)
//     {
//         var hierarchyItemNodes = new List<HierarchyItemNode>();
//         foreach (var child in hierarchy.Children)
//         {
//             var childHierarchyItem = new HierarchyItemNode();
//             childHierarchyItem.Children = new List<HierarchyItemNode>();
//             childHierarchyItem.IconUrl = child.IconUrl;
//             childHierarchyItem.IconSvg = hierarchy.IconSvg;
//             childHierarchyItem.HierarchyId = child.Id;
//             childHierarchyItem.Title = child.Title;
//             if (hierarchyIds.Contains(childHierarchyItem.HierarchyId))
//             {
//                 childHierarchyItem.IsInThisHierarchy = true;
//             }
//             hierarchyItem.Children.Add(childHierarchyItem);
//             GetChildrenHierarchyItems(hierarchyIds, child, childHierarchyItem);
//         }
//     }

//     public ListResult<Guid> GetEntityGuids(long hierarchyId, int pageNumber, List<Guid> excludedEntityGuids)
//     {
//         CheckExcludedEntitiesCount(excludedEntityGuids);
//         var listParameters = ListParameters.Create();
//         listParameters.PageNumber = pageNumber;
//         listParameters.AddFilter<HierarchyItem>(i => i.HierarchyId, hierarchyId.ToString());
//         listParameters.AddSort<HierarchyItem>(i => i.Order, SortDirection.Ascending);
//         listParameters.AddSort<HierarchyItem>(i => i.Id, SortDirection.Ascending);
//         var entityGuids = Write
//             .All
//             .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
//             .ApplyListParametersAndGetTotalCount(listParameters)
//             .Convert<HierarchyItem, Guid>(i => i.EntityGuid);
//         return entityGuids;
//     }

//     private void CheckExcludedEntitiesCount(List<Guid> excludedEntityGuids)
//     {
//         if (excludedEntityGuids.Count > 100)
//         {
//             throw new ClientException("Excluding more than 100 items will slow down the system logarithmically. Please solve this problem.");
//         }
//     }

//     public ListResult<Guid> GetEntityGuids(ListParameters listParameters, List<Guid> excludedEntityGuids)
//     {
//         CheckExcludedEntitiesCount(excludedEntityGuids);
//         var entityGuids = Write
//             .All
//             .Where(i => !excludedEntityGuids.Contains(i.EntityGuid))
//             .ApplyListParametersAndGetTotalCount(listParameters)
//             .Convert<HierarchyItem, Guid>(i => i.EntityGuid);
//         return entityGuids;
//     }

//     public void ToggleHierarchy(string entityType, long hierarchyId, Guid entityGuid)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         entityGuid.Ensure().IsNotEmpty();
//         hierarchyId.Ensure().IsGreaterThanZero();
//         var hierarchyItem = Write.All.FirstOrDefault(i => i.EntityGuid == entityGuid && i.HierarchyId == hierarchyId);
//         if (hierarchyItem == null)
//         {
//             hierarchyItem = new HierarchyItem();
//             hierarchyItem.EntityGuid = entityGuid;
//             hierarchyItem.HierarchyId = hierarchyId;
//             hierarchyItem.Order = 1;
//             Write.Create(hierarchyItem);
//         }
//         else
//         {
//             Write.Delete(hierarchyItem);
//         }
//         new HierarchyBusiness().CountItemsInHierarchy(hierarchyId);
//         OnHierarchyToggled?.Invoke(hierarchyItem);
//     }

//     public void PutInHierarchy(string entityType, long hierarchyId, Guid entityGuid)
//     {
//         if (IsInHierarchy(entityType, entityGuid, hierarchyId))
//         {
//             return;
//         }
//         ToggleHierarchy(entityType, hierarchyId, entityGuid);
//     }

//     public void RemoveEntity(string entityType, Guid entityGuid)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         var query = $@"
//             delete
//             from {Write.TableName}
//             where EntityGuid = '{entityGuid}'
//             ";
//         Write.Run(query);
//     }

//     public void RemoveOrphanEntities(string entityType, List<Guid> entityGuids)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         var orphanHierarchyItems = Write.All.Where(i => !entityGuids.Contains(i.EntityGuid)).ToList();
//         foreach (var orphanHierarchyItem in orphanHierarchyItems)
//         {
//             Write.Delete(orphanHierarchyItem);
//         }
//     }

//     public List<long> GetItemHierarchyIds(string entityType, Guid entityGuid)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         var hierarchyItems = Read.All.Where(i => i.EntityGuid == entityGuid).Select(i => i.HierarchyId).Distinct().ToList();
//         return hierarchyItems;
//     }

//     public bool IsInHierarchy(string entityType, Guid entityGuid, long hierarchyId)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         var hierarchyItem = Read.All.Any(i => i.EntityGuid == entityGuid && i.HierarchyId == hierarchyId);
//         return hierarchyItem;
//     }
// }
