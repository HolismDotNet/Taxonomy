CREATE TABLE [dbo].[CategoryItems]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[CategoryId] [bigint] NOT NULL,
[EntityTypeGuid] [uniqueidentifier] NOT NULL,
[EntityGuid] [uniqueidentifier] NOT NULL,
[Order] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CategoryItems] ADD CONSTRAINT [PK_CategoryItems] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CategoryItems] ADD CONSTRAINT [IX_CategoryItems_Unique_CategoryId_And_EntityGuid_And_EntityTypeGuid] UNIQUE NONCLUSTERED  ([CategoryId], [EntityTypeGuid], [EntityGuid]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CategoryItems] ADD CONSTRAINT [FK_CategoryItems_CategoryId_Categories_Id] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
GO
