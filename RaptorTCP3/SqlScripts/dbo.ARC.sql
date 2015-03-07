USE [Damocles]
GO

/****** Object: Table [dbo].[ARC] Script Date: 3/6/2015 1:57:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[ARC];


GO
CREATE TABLE [dbo].[ARC] (
    [ARCId] INT IDENTITY (1, 1) NOT NULL,
    [AId]   INT NULL,
    [RId]   INT NULL,
    [CId]   INT NULL
);


