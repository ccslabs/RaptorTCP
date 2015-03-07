USE [Damocles]
GO

/****** Object: Table [dbo].[ContentObjects] Script Date: 3/6/2015 1:58:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[ContentObjects];


GO
CREATE TABLE [dbo].[ContentObjects] (
    [ContentObjectId] INT             IDENTITY (1, 1) NOT NULL,
    [ContentPath]     VARBINARY (MAX) NOT NULL,
    [ARCId]           INT             NULL
);


