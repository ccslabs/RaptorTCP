USE [Damocles]
GO

/****** Object: Table [dbo].[News] Script Date: 3/6/2015 2:00:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[News];


GO
CREATE TABLE [dbo].[News] (
    [NewsId]        INT            IDENTITY (1, 1) NOT NULL,
    [UserId]        INT            NULL,
    [CreatedOnDate] ROWVERSION     NOT NULL,
    [NewsSummary]   NCHAR (500)    NULL,
    [NewsContent]   NVARCHAR (MAX) NULL,
    [NewsTitle]     NVARCHAR (50)  NULL,
    [NewsSource]    NVARCHAR (MAX) NULL,
    [Image]         NVARCHAR (MAX) NULL
);


