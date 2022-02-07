// namespace Taxonomy;

// public class HierarchyBusiness : Business<HierarchyView, Hierarchy>
// {
//     public const string HierarchyIconsContainerName = "hierarchyicons";

//     protected override Repository<Hierarchy> WriteRepository => Repository.Hierarchy;

//     protected override ReadRepository<HierarchyView> ReadRepository => Repository.HierarchyView;

//     public static List<Action<HierarchyNode>> HierarchyNodeAugmenters = new List<Action<HierarchyNode>>();

//     public static List<Action<List<HierarchyNode>>> HierarchyNodesAugmenters = new List<Action<List<HierarchyNode>>>();

//     private static Dictionary<string, List<HierarchyNode>> entityTypeHierarchiesHierarchy = new Dictionary<string, List<HierarchyNode>>();

//     private static List<SimpleHierarchyNode> simpleHierarchiesHierarchy;

//     private static List<Hierarchy> hierarchies;

//     private static Dictionary<string, Hierarchy> hierarchiesTitleDictionary;

//     public Hierarchy Create(string entityType, Hierarchy hierarchy)
//     {
//         var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//         hierarchy.EntityTypeGuid = entityTypeGuid;
//         return Create(hierarchy);
//     }

//     public static List<Hierarchy> Hierarchies
//     {
//         get
//         {
//             if (hierarchies == null) 
//             {
//                 hierarchies = Repository.Hierarchy.All.Where(i => i.Show == true).ToList();
//             }
//             return hierarchies;
//         }
//     }

//     public static Dictionary<string, Hierarchy> HierarchiesTitleDictionary
//     {
//         get
//         {
//             if (hierarchiesTitleDictionary == null)
//             {
//                 hierarchiesTitleDictionary = Hierarchies.ToDictionary(i => i.Title, i => i);
//             }
//             return hierarchiesTitleDictionary;
//         }
//     }

//     public static List<Hierarchy> RootHierarchies
//     {
//         get
//         {
//             var rootHierarchies = Hierarchies.Where(i => i.ParentId == null).ToList();
//             return rootHierarchies;
//         }
//     }

//     public void CountItemsInHierarchies()
//     {
//         var hierarchies = GetAll();
//         foreach (var hierarchy in hierarchies)
//         {
//             hierarchy.ItemsCount = new HierarchyItemBusiness().GetCountOfItemsInHierarchy(hierarchy);
//         }
//         WriteRepository.BulkUpdate(hierarchies);
//     }

//     public int GetTotalCategorizedItemsCount(string entityType)
//     {
//         CountItemsInHierarchies();
//         if (entityType.IsSomething())
//         {
//             var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//             var count = ReadRepository.All.Where(i => i.EntityTypeGuid == entityTypeGuid).Sum(i => i.ItemsCount) ?? 0;
//             return count;
//         }
//         else
//         {
//             var count = ReadRepository.All.Sum(i => i.ItemsCount) ?? 0;
//             return count;
//         }
//     }

//     public void CountItemsInHierarchy(long hierarchyId)
//     {
//         var hierarchy = Get(hierarchyId);
//         hierarchy.ItemsCount = new HierarchyItemBusiness().GetCountOfItemsInHierarchy(hierarchy);
//         Update(hierarchy);
//     }

//     // public static Hierarchy GetHierarchy(string name)
//     // {
//     //     if (HierarchiesTitleDictionary.ContainsKey(name))
//     //     {
//     //         return HierarchiesTitleDictionary[name];
//     //     }
//     //     return null;
//     // }

//     public static bool HasSubhierarchies(long hierarchyId)
//     {
//         var hasSubhierarchies = Hierarchies.Any(i => i.ParentId == hierarchyId);
//         return hasSubhierarchies;
//     }

//     public static List<Hierarchy> GetSubhierarchies(long parentId)
//     {
//         var subhierarchies = Hierarchies.Where(i => i.ParentId == parentId).ToList();
//         return subhierarchies;
//     }

//     protected override void ModifyItemBeforeReturning(Hierarchy item)
//     {
//         item.RelatedItems.IconUrl = GetIconUrl(item);
//         item.RelatedItems.HasDefaultIcon = !item.IconGuid.HasValue;
//         item.RelatedItems.HasChildren = ReadRepository.All.Any(i => i.ParentId == item.Id);
//         base.ModifyItemBeforeReturning(item);
//     }

//     private dynamic GetIconUrl(Hierarchy hierarchy)
//     {
//         var iconUrl = Storage.GetImageUrl(HierarchyIconsContainerName, hierarchy.IconGuid.HasValue ? hierarchy.IconGuid.Value : Guid.Empty);
//         return iconUrl;
//     }

//     public List<Hierarchy> GetList(long? parentId = null)
//     {
//         var hierarchies = ReadRepository.All.Where(i => i.ParentId == parentId).ToList();
//         ModifyListBeforeReturning(hierarchies);
//         return hierarchies;
//     }

//     public void ChangeTitle(long id, string title)
//     {
//         var hierarchy = Get(id);
//         hierarchy.Title = title;
//         WriteRepository.Update(hierarchy);
//     }

//     public void ChangeDescription(long id, string description)
//     {
//         var hierarchy = Get(id);
//         hierarchy.Description = description;
//         WriteRepository.Update(hierarchy);
//     }

//     public List<HierarchyNode> CacheAndGetHierarchy(List<Hierarchy> hierarchies, string entityType = null)
//     {
//         var tempentityType = entityType;
//         if (tempentityType == null)
//         {
//             tempentityType = "null";
//         }
//         if (entityTypeHierarchiesHierarchy.ContainsKey(tempentityType))
//         {
//             return entityTypeHierarchiesHierarchy[tempentityType];
//         }
//         entityTypeHierarchiesHierarchy.Add(tempentityType, GetHierarchy(entityType, hierarchies));
//         return entityTypeHierarchiesHierarchy[tempentityType];
//     }

//     public List<HierarchyNode> CacheAndGetHierarchy(string entityType = null)
//     {
//         var tempentityType = entityType;
//         if (tempentityType == null)
//         {
//             tempentityType = "null";
//         }
//         if (entityTypeHierarchiesHierarchy.ContainsKey(tempentityType))
//         {
//             return entityTypeHierarchiesHierarchy[tempentityType];
//         }
//         entityTypeHierarchiesHierarchy.Add(tempentityType, GetHierarchy(entityType));
//         return entityTypeHierarchiesHierarchy[tempentityType];
//     }

//     public List<HierarchyNode> GetRootHierarchies(string entityType = null)
//     {
//         var allHierarchies = ReadRepository.All.Where(i => i.ParentId == null).ToList();
//         if (entityType.IsSomething())
//         {
//             var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//             allHierarchies = allHierarchies.Where(i => i.EntityTypeGuid == entityTypeGuid).ToList();
//         }
//         return ConvertToHierarchyNodes(allHierarchies, null);
//     }

//     public List<HierarchyNode> GetHierarchies(long? parentId = null)
//     {
//         var allHierarchies = ReadRepository.All.Where(i => i.ParentId == parentId).ToList();
//         HierarchyNode parentHierarchy = null;
//         if (parentId.HasValue)
//         {
//             parentHierarchy = ConvertToHierarchyNode(GetOrNull(parentId.Value), null);
//         }
//         return ConvertToHierarchyNodes(allHierarchies, parentHierarchy);
//     }

//     private List<HierarchyNode> ConvertToHierarchyNodes(List<Hierarchy> allHierarchies, HierarchyNode parentHierarchy)
//     {
//         var hierarchyNodes = allHierarchies.Select(i => ConvertToHierarchyNode(i, parentHierarchy)).ToList();
//         foreach (var hierarchyNode in hierarchyNodes)
//         {
//             foreach (var augmenter in HierarchyNodeAugmenters)
//             {
//                 augmenter.Invoke(hierarchyNode);
//             }
//         }
//         foreach (var augmenter in HierarchyNodesAugmenters)
//         {
//             augmenter.Invoke(hierarchyNodes);
//         }
//         return hierarchyNodes;
//     }

//     public List<HierarchyNode> GetHierarchy(string entityType = null)
//     {
//         var query = ReadRepository.All;
//         var allHierarchies = query.ToList();
//         return GetHierarchy(entityType, allHierarchies);
//     }

//     private List<HierarchyNode> GetHierarchy(string entityType, List<Hierarchy> hierarchies)
//     {
//         var allHierarchies = hierarchies;
//         if (entityType.IsSomething())
//         {
//             var entityTypeGuid = new EntityTypeBusiness().GetGuid(entityType);
//             allHierarchies = allHierarchies.Where(i => i.EntityTypeGuid == entityTypeGuid).ToList();
//         }
//         var rootHierarchies = allHierarchies.Where(i => i.ParentId == null).Select(i => ConvertToHierarchyNode(i, null)).ToList();
//         foreach (var rootHierarchy in rootHierarchies)
//         {
//             foreach (var augmenter in HierarchyNodeAugmenters)
//             {
//                 augmenter.Invoke(rootHierarchy);
//             }
//             rootHierarchy.Children = GetChildrenHierarchies(allHierarchies, rootHierarchy);
//         }
//         foreach (var augmenter in HierarchyNodesAugmenters)
//         {
//             augmenter.Invoke(rootHierarchies);
//         }
//         return rootHierarchies;
//     }

//     private HierarchyNode ConvertToHierarchyNode(Hierarchy hierarchy, HierarchyNode parentHierarchy)
//     {
//         var hierarchyNode = new HierarchyNode();
//         hierarchyNode.Id = hierarchy.Id;
//         hierarchyNode.Guid = hierarchy.Guid;
//         hierarchyNode.Title = hierarchy.Title;
//         hierarchyNode.Description = hierarchy.Description;
//         hierarchyNode.IconUrl = GetIconUrl(hierarchy);
//         hierarchyNode.IconSvg = hierarchy.IconSvg;
//         hierarchyNode.HasDefaultIcon = !hierarchy.IconGuid.HasValue;
//         hierarchyNode.ParentId = parentHierarchy?.Id;
//         hierarchyNode.ItemsCount = hierarchy.ItemsCount;
//         hierarchyNode.Slug = hierarchy.Slug;
//         hierarchyNode.Level = parentHierarchy == null ? 1 : (parentHierarchy.Level + 1);
//         hierarchyNode.Children = new List<HierarchyNode>();
//         return hierarchyNode;
//     }

//     private List<HierarchyNode> GetChildrenHierarchies(List<Hierarchy> allHierarchies, HierarchyNode parentHierarchy)
//     {
//         var childrenHierarchies = allHierarchies.Where(i => i.ParentId == parentHierarchy.Id).Select(i => ConvertToHierarchyNode(i, parentHierarchy)).ToList();
//         foreach (var childHierarchy in childrenHierarchies)
//         {
//             foreach (var augmenter in HierarchyNodeAugmenters)
//             {
//                 augmenter.Invoke(childHierarchy);
//             }
//             childHierarchy.Children = GetChildrenHierarchies(allHierarchies, childHierarchy);
//         }
//         return childrenHierarchies;
//     }

//     public List<SimpleHierarchyNode> GetSimpleHierarchy()
//     {
//         if (simpleHierarchiesHierarchy != null)
//         {
//             return simpleHierarchiesHierarchy;
//         }
//         var allHierarchies = ReadRepository.All.ToList();
//         var rootHierarchies = allHierarchies.Where(i => i.ParentId == null).Select(i => new SimpleHierarchyNode
//         {
//             Id = i.Id,
//             Title = i.Title,
//             ParentId = null,
//             Children = new List<SimpleHierarchyNode>()
//         }).ToList();
//         foreach (var rootHierarchy in rootHierarchies)
//         {
//             rootHierarchy.Children = GetSimpleChildrenHierarchies(allHierarchies, rootHierarchy);
//         }
//         simpleHierarchiesHierarchy = rootHierarchies;
//         return rootHierarchies;
//     }

//     private List<SimpleHierarchyNode> GetSimpleChildrenHierarchies(List<Hierarchy> allHierarchies, SimpleHierarchyNode parentHierarchy)
//     {
//         var childrenHierarchies = allHierarchies.Where(i => i.ParentId == parentHierarchy.Id).Select(i => new SimpleHierarchyNode
//         {
//             Id = i.Id,
//             Title = i.Title,
//             ParentId = parentHierarchy.Id,
//             Children = new List<SimpleHierarchyNode>()
//         }).ToList(); ;
//         foreach (var childHierarchy in childrenHierarchies)
//         {
//             childHierarchy.Children = GetSimpleChildrenHierarchies(allHierarchies, childHierarchy);
//         }
//         return childrenHierarchies;
//     }

//     public string ChangeIcon(long hierarchyId, byte[] bytes)
//     {
//         var hierarchy = Get(hierarchyId);
//         if (hierarchy.IconGuid.HasValue)
//         {
//             Storage.DeleteImage(HierarchyIconsContainerName, hierarchy.IconGuid.Value);
//         }
//         var thumbnail = ImageHelper.MakeImageThumbnail(TaxonomyConfig.HierarchyThumbnailWidth, null, bytes);
//         hierarchy.IconGuid = Guid.NewGuid();
//         Storage.UploadImage(thumbnail.GetBytes(), hierarchy.IconGuid.Value, HierarchyIconsContainerName);
//         WriteRepository.Update(hierarchy);
//         return Storage.GetImageUrl(HierarchyIconsContainerName, hierarchy.IconGuid.Value);
//     }

//     public void RemoveIcon(long hierarchyId)
//     {
//         var hierarchy = Get(hierarchyId);
//         if (hierarchy.IconGuid.HasValue)
//         {
//             Storage.DeleteImage(HierarchyIconsContainerName, hierarchy.IconGuid.Value);
//         }
//         hierarchy.IconGuid = null;
//         WriteRepository.Update(hierarchy);
//     }

//     public override void Validate(Hierarchy model)
//     {
//         model.Title.Ensure().IsSomething("Title is not provided");
//         if (model.ParentId.HasValue)
//         {
//             var parentHierarchy = Get(model.ParentId.Value);
//             if (parentHierarchy == null)
//             {
//                 throw new ClientException("Parent hierarchy does not exist");
//             }
//         }
//         model.EntityTypeGuid.Ensure().IsNotEmpty();
//     }

//     protected override void PreCreation(Hierarchy model)
//     {
//         model.Guid = Guid.NewGuid();
//         model.Show = true;
//     }

//     public Hierarchy GetByTitle(string title)
//     {
//         return GetByTitle(title, null);
//     }

//     public Hierarchy GetByTitle(string title, long? parentId = null)
//     {
//         var hierarchies = GetList(i => i.Title == title);
//         if (parentId.HasValue)
//         {
//             hierarchies = hierarchies.Where(i => i.ParentId == parentId.Value).ToList();
//         }
//         if (hierarchies.Count > 1)
//         {
//             throw new ClientException($"More than one hierarchy is found with this title");
//         }
//         return hierarchies.FirstOrDefault();
//     }

//     public Hierarchy GetByCode(string code)
//     {
//         var hierarchies = GetList(i => i.Code == code);
//         if (hierarchies.Count > 1)
//         {
//             throw new ClientException($"More than one hierarchy is found with this code");
//         }
//         return hierarchies.FirstOrDefault();
//     }

//     public void UpdateIsLeaf()
//     {
//         var hierarchies = GetAll();
//         foreach (var hierarchy in hierarchies)
//         {
//             hierarchy.IsLeaf = GetList(i => i.ParentId == hierarchy.Id).Count == 0;
//             Update(hierarchy);
//         }
//     }

//     public List<Guid> OrderGuids(List<Guid> guids)
//     {
//         var hierarchies = GetHierarchiesInOrder(guids);
//         var orderedGuids = hierarchies.Select(i => i.Guid).ToList();
//         return orderedGuids;
//     }

//     public List<Hierarchy> GetHierarchiesInOrder(List<Guid> guids)
//     {
//         var hierarchies = GetList(guids);
//         var rootHierarchies = hierarchies.Where(i => i.ParentId == null || !hierarchies.Any(x => i.ParentId == x.Id)).ToList();
//         var orderedIds = new List<long>();
//         orderedIds.AddRange(rootHierarchies.Select(i => i.Id).OrderBy(i => i).ToList());
//         foreach (var rootHierarchy in rootHierarchies)
//         {
//             AddChildHierarchies(rootHierarchy, hierarchies, orderedIds);
//         }
//         hierarchies = hierarchies.OrderBy(i => orderedIds.IndexOf(i.Id)).ToList();
//         return hierarchies;
//     }

//     private void AddChildHierarchies(Hierarchy rootHierarchy, List<Hierarchy> hierarchies, List<long> orderedIds)
//     {
//         var childHierarchies = hierarchies.Where(i => i.ParentId == rootHierarchy.Id).OrderBy(i => i.Id).ToList();
//         orderedIds.AddRange(childHierarchies.Select(i => i.Id).OrderBy(i => i).ToList());
//         foreach (var childHierarchy in childHierarchies)
//         {
//             AddChildHierarchies(childHierarchy, hierarchies, orderedIds);
//         }
//     }
// }
