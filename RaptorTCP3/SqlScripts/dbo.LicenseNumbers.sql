USE [Damocles]
GO

/****** Object: Table [dbo].[LicenseNumbers] Script Date: 3/6/2015 1:59:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[LicenseNumbers];


GO
CREATE TABLE [dbo].[LicenseNumbers] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [LicenseNumber] NCHAR (10)     NOT NULL,
    [emailAddress]  NVARCHAR (MAX) NULL
);


