USE [Damocles]
GO

/****** Object: Table [dbo].[Languages] Script Date: 3/6/2015 1:59:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Languages];


GO
CREATE TABLE [dbo].[Languages] (
    [LanguageId]           INT           IDENTITY (1, 1) NOT NULL,
    [LanguageEnglishName]  VARCHAR (250) NOT NULL,
    [LanguageLocalName]    VARCHAR (250) NOT NULL,
    [TranslationAvailable] BIT           NOT NULL
);


