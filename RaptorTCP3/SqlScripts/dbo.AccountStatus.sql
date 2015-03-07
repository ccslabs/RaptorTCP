USE [Damocles]
GO

/****** Object: Table [dbo].[AccountStatus] Script Date: 3/6/2015 1:52:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[AccountStatus];


GO
CREATE TABLE [dbo].[AccountStatus] (
    [Id]         INT        IDENTITY (1, 1) NOT NULL,
    [StatusName] NCHAR (20) NOT NULL
);


