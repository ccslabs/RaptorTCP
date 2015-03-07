USE [Damocles]
GO

/****** Object: Table [dbo].[States] Script Date: 3/6/2015 2:01:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[States];


GO
CREATE TABLE [dbo].[States] (
    [StateId]          INT           IDENTITY (1, 1) NOT NULL,
    [StateEnglishName] VARCHAR (250) NOT NULL,
    [StateLocalName]   VARCHAR (250) NOT NULL
);


