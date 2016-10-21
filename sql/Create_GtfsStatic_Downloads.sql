USE [MbtaTracker]
GO

/****** Object:  Table [dbo].[Gtfs.Dowload]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Download]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Download] table';
	DROP TABLE [GtfsStatic].[Download];
END;


CREATE TABLE [GtfsStatic].[Download](
	[download_id] [int] IDENTITY(1,1) NOT NULL,
	[download_file_name] [nvarchar](100) NOT NULL,
	[download_date] [datetime] NOT NULL,
 CONSTRAINT [PK_GtfsStatic_Download] PRIMARY KEY CLUSTERED 
(
	[download_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


