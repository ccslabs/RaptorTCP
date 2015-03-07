USE [Damocles]
GO

/****** Object: Table [dbo].[Criminal] Script Date: 3/6/2015 1:58:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Criminal];


GO
CREATE TABLE [dbo].[Criminal] (
    [CId]             INT IDENTITY (1, 1) NOT NULL,
    [UId]             INT NOT NULL,
    [CrimeId]         INT NOT NULL,
    [JurisidictionId] INT NOT NULL
);


