USE [Damocles]
GO

/****** Object: Table [dbo].[RestrictionTypes] Script Date: 3/6/2015 2:00:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[RestrictionTypes];


GO
CREATE TABLE [dbo].[RestrictionTypes] (
    [RestrictionId]      INT          IDENTITY (1, 1) NOT NULL,
    [Restriction Reason] VARCHAR (50) NOT NULL
);


