USE [Damocles]
GO

/****** Object: Table [dbo].[CrimeTypes] Script Date: 3/6/2015 1:58:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[CrimeTypes];


GO
CREATE TABLE [dbo].[CrimeTypes] (
    [CrimeId]   INT           IDENTITY (1, 1) NOT NULL,
    [CrimeName] VARCHAR (150) NOT NULL,
    [CId]       INT           NOT NULL
);


