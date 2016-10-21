USE [MbtaTracker]
GO

/****** Object:  Table [dbo].[Gtfs.Feed_Info]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Feed_Info]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Feed_Info] table';
	DROP TABLE [GtfsStatic].[Feed_Info];
END;


CREATE TABLE [GtfsStatic].[Feed_Info](
	[feed_info_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[feed_publisher_name] [nvarchar](250) NULL,
	[feed_publisher_url] [nvarchar](250) NULL,
	[feed_lang] [nvarchar](50) NULL,
	[feed_start_date_txt] [nvarchar](8) NULL,
	[feed_end_date_txt] [nvarchar](8) NULL,
	[feed_start_date] [datetime] NULL,
	[feed_end_date] [datetime] NULL,
	[feed_version] [nvarchar](255) NULL,
CONSTRAINT [PK_GtfsStatic_Feed_Info] PRIMARY KEY CLUSTERED 
(
	[feed_info_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
GO

ALTER TABLE [GtfsStatic].[Feed_Info]
ADD CONSTRAINT [FK_GtfsStatic_Feed_Info_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);
GO




