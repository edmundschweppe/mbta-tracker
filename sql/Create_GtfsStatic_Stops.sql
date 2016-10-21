USE [MbtaTracker]
GO

/****** Object:  Table [GtfsStatic].[Stops]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Stops]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Stops] table';
	DROP TABLE [GtfsStatic].[Stops];
END;


CREATE TABLE [GtfsStatic].[Stops](
	[stop_row_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[stop_id] [nvarchar](255) NOT NULL,
	[stop_code] [nvarchar](255) NULL,
	[stop_name] [nvarchar](255) NULL,
	[stop_desc] [nvarchar](255) NULL,
	[stop_lat_txt] [nvarchar](255) NULL,
	[stop_lon_txt] [nvarchar](255) NULL,
	[zone_id] [nvarchar](255) NULL,
	[stop_url] [nvarchar](255) NULL,
	[location_type] [int] NULL,
	[parent_station] [nvarchar](255) NULL,
	[stop_timezone] [nvarchar](255) NULL,
	[wheelchair_boarding] [int] NULL,
CONSTRAINT [PK_GtfsStatic_Stops] PRIMARY KEY CLUSTERED 
(
	[stop_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [GtfsStatic].[Stops] table';
ELSE
BEGIN
	PRINT 'Error creating [GtfsStatic].[Stops] table; aborting';
	SET NOEXEC ON
END

GO


ALTER TABLE [GtfsStatic].[Stops]
ADD CONSTRAINT [FK_GtfsStatic_Stops_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_GtfsStatic_Stops_Download] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_GtfsStatic_Stops_Download] foreign key; aborting';
	SET NOEXEC ON
END
GO




