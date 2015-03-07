USE [Damocles]
GO

/****** Object: Table [dbo].[Settings] Script Date: 3/6/2015 2:00:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Settings];


GO
CREATE TABLE [dbo].[Settings] (
    [SettingsId]                 INT IDENTITY (1, 1) NOT NULL,
    [NumberOfNewsItemsToDisplay] INT NULL
);


