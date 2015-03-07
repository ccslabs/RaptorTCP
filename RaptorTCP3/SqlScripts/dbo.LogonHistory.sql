USE [Damocles]
GO

/****** Object: Table [dbo].[LogonHistory] Script Date: 3/6/2015 2:00:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[LogonHistory];


GO
CREATE TABLE [dbo].[LogonHistory] (
    [LogonHistoryId] INT      IDENTITY (1, 1) NOT NULL,
    [UserId]         INT      NOT NULL,
    [LoggedOnDate]   DATETIME NOT NULL,
    [LoggedOffDate]  DATETIME NULL
);


