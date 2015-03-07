USE [Damocles]
GO

/****** Object: Table [dbo].[Countries] Script Date: 3/6/2015 1:58:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Countries];


GO
CREATE TABLE [dbo].[Countries] (
    [CountryId]          INT           IDENTITY (1, 1) NOT NULL,
    [CountryNameEnglish] VARCHAR (250) NOT NULL,
    [CountryNameLocal]   VARCHAR (250) NOT NULL,
    [CountryTimeZone]    INT           NOT NULL,
    [StatesId]           INT           NOT NULL
);


