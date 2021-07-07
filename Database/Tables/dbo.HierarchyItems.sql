CREATE TABLE [dbo].[HierarchyItems]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[HierarchyId] [bigint] NOT NULL,
[EntityTypeGuid] [uniqueidentifier] NOT NULL,
[EntityGuid] [uniqueidentifier] NOT NULL,
[Order] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HierarchyItems] ADD CONSTRAINT [PK_HierarchyItems] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HierarchyItems] ADD CONSTRAINT [IX_HierarchyItems_Unique_HierarchyId_And_EntityGuid_And_EntityTypeGuid] UNIQUE NONCLUSTERED  ([HierarchyId], [EntityTypeGuid], [EntityGuid]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HierarchyItems] ADD CONSTRAINT [FK_HierarchyItems_HierarchyId_Hierarchies_Id] FOREIGN KEY ([HierarchyId]) REFERENCES [dbo].[Hierarchies] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
GO
