USE [Damocles]
GO

/****** Object: Table [dbo].[Allowed] Script Date: 3/6/2015 1:57:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Allowed];


GO
CREATE TABLE [dbo].[Allowed] (
    [AId]            INT IDENTITY (1, 1) NOT NULL,
    [Uid]            INT NOT NULL,
    [JurisidctionId] INT NOT NULL
);


