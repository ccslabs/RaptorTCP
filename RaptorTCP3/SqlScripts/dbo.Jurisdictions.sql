USE [Damocles]
GO

/****** Object: Table [dbo].[Jurisdictions] Script Date: 3/6/2015 1:58:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Jurisdictions];


GO
CREATE TABLE [dbo].[Jurisdictions] (
    [JurisdictionId]   INT            IDENTITY (1, 1) NOT NULL,
    [JurisdictionName] NVARCHAR (MAX) NOT NULL,
    [CountryId]        INT            NOT NULL,
    [StateId]          INT            NOT NULL,
    [LanguageId]       INT            NOT NULL
);


