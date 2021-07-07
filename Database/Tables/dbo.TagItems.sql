CREATE TABLE [dbo].[TagItems]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[TagId] [bigint] NOT NULL,
[EntityTypeGuid] [uniqueidentifier] NOT NULL,
[EntityGuid] [uniqueidentifier] NOT NULL,
[Order] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TagItems] ADD CONSTRAINT [PK_TagItems] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TagItems] ADD CONSTRAINT [IX_TagItems_Unique_EntityGuid_And_EntityTypeGuid_And_TagId] UNIQUE NONCLUSTERED  ([TagId], [EntityTypeGuid], [EntityGuid]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TagItems] ADD CONSTRAINT [FK_TagItems_TagId_Tags_Id] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
GO
