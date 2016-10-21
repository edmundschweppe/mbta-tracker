USE [MbtaTracker]
GO

/****** Object:  Table [GtfsStatic].[Stop_Times]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Stop_Times]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Stop_Times] table';
	DROP TABLE [GtfsStatic].[Stop_Times];
END;


CREATE TABLE [GtfsStatic].[Stop_Times](
	[stop_time_row_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[trip_id] [nvarchar](255) NOT NULL,
	[arrival_time_txt] [nvarchar](255) NULL,
	[departure_time_txt] [nvarchar](255) NULL,
	[stop_id] [nvarchar](255) NOT NULL,
	[stop_sequence] [int] NULL,
	[stop_headsign] [nvarchar](255) NULL,
	[pickup_type] [int] NULL,
	[drop_off_type] [int] NULL,
	[shape_dist_traveled_txt] [nvarchar](255) NULL,
	[timepoint] [nvarchar](1) NULL,
CONSTRAINT [PK_GtfsStatic_Stop_Times] PRIMARY KEY CLUSTERED 
(
	[stop_time_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [GtfsStatic].[Stop_Times] table';
ELSE
BEGIN
	PRINT 'Error creating [GtfsStatic].[Stop_Times] table; aborting';
	SET NOEXEC ON
END
GO


ALTER TABLE [GtfsStatic].[Stop_Times]
ADD CONSTRAINT [FK_GtfsStatic_Stop_Times_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_GtfsStatic_Stop_Times_Download] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_GtfsStatic_Stop_Times_Download] foreign key; aborting';
	SET NOEXEC ON
END
GO




