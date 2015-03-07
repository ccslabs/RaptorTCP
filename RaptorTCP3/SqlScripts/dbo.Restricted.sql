USE [Damocles]
GO

/****** Object: Table [dbo].[Restricted] Script Date: 3/6/2015 2:00:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Restricted];


GO
CREATE TABLE [dbo].[Restricted] (
    [RId]            INT IDENTITY (1, 1) NOT NULL,
    [UId]            INT NOT NULL,
    [RestrictedToId] INT NOT NULL,
    [JurisdictionId] INT NOT NULL
);


