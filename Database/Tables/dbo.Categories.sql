CREATE TABLE [dbo].[Categories]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Categories_Guid] DEFAULT (newid()),
[EntityTypeGuid] [uniqueidentifier] NOT NULL,
[Title] [nvarchar] (100) NOT NULL,
[Code] [nvarchar] (100) NULL,
[IconGuid] [uniqueidentifier] NULL,
[IconSvg] [nvarchar] (max) NULL,
[ParentCategoryId] [bigint] NULL,
[Description] [nvarchar] (max) NULL,
[Order] [int] NOT NULL,
[Show] [bit] NOT NULL,
[ItemsCount] [int] NULL,
[UrlKey] [nvarchar] (400) NULL,
[Level] [int] NULL,
[IsLeaf] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [IX_Categories_Unique_ParentCategoryId_And_Title] UNIQUE NONCLUSTERED  ([ParentCategoryId], [Title]) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Categories_Unique_UrlKey] ON [dbo].[Categories] ([UrlKey]) WHERE ([UrlKey] IS NOT NULL) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [FK_Categories_ParentCategoryId_Categories_Id] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[Categories] ([Id])
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET ANSI_PADDING ON
GO
SET ANSI_WARNINGS ON
GO
SET ARITHABORT ON
GO
SET CONCAT_NULL_YIELDS_NULL ON
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET QUOTED_IDENTIFIER ON
GO