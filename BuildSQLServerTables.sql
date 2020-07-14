USE [RPW_3030]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Application] DROP CONSTRAINT [FK_Application_Machine]
GO

/****** Object:  Table [dbo].[Application]    Script Date: 07/13/2020 9:36:46 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application]') AND type in (N'U'))
DROP TABLE [dbo].[Application]
GO

ALTER TABLE [dbo].[Service] DROP CONSTRAINT [FK_Service_Machine]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service]') AND type in (N'U'))
DROP TABLE [dbo].[Service]
GO


/****** Object:  Table [dbo].[Machine]    Script Date: 07/13/2020 9:32:31 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Machine]') AND type in (N'U'))
DROP TABLE [dbo].[Machine]
GO


CREATE TABLE [dbo].[Machine](
	[MachineName] [nvarchar](50) NOT NULL,
	[isOSx64] [bit] NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[hasRegistryHack] [bit] NOT NULL,
 CONSTRAINT [PK_Machine] PRIMARY KEY CLUSTERED 
(
	[MachineName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Machine]    Script Date: 07/13/2020 9:32:31 AM ******/

CREATE TABLE [dbo].[Application](
	[ID] int IDENTITY(1,1),
	[RegistryKey] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Version] [nvarchar](50) NULL,
	[Path] [nvarchar](250) NULL,
	[Bitness] [nvarchar](5) NULL,
	[MachineName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Application]  WITH CHECK ADD  CONSTRAINT [FK_Application_Machine] FOREIGN KEY([MachineName])
REFERENCES [dbo].[Machine] ([MachineName])
GO

ALTER TABLE [dbo].[Application] CHECK CONSTRAINT [FK_Application_Machine]
GO


/****** Object:  Table [dbo].[Service]    Script Date: 07/13/2020 9:33:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Service](
	[ServiceID] int IDENTITY(1,1),
	[ServiceName] [varchar](64) NOT NULL,
	[ServiceUser] [varchar](128) NULL,
	[ServiceGroup] [varchar](128) NULL,
	[ServiceTypeID] [uniqueidentifier] NOT NULL,
	[ServiceDescription] [varchar](512) NULL,
	[MachineName] [nvarchar](50) NOT NULL
	 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
	(
		[ServiceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_Machine] FOREIGN KEY([MachineName])
REFERENCES [dbo].[Machine] ([MachineName])
GO

ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_Machine]
GO

