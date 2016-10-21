USE [MbtaTracker]
GO

/****** Object:  Table [GtfsStatic].[Trips]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Trips]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Trips] table';
	DROP TABLE [GtfsStatic].[Trips];
END;


CREATE TABLE [GtfsStatic].[Trips](
	[trip_row_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[route_id] [nvarchar](255) NOT NULL,
	[service_id] [nvarchar](255) NOT NULL,
	[trip_id] [nvarchar](255) NOT NULL,
	[trip_headsign] [nvarchar](255) NULL,
	[trip_shortname] [nvarchar](255) NULL,
	[direction_id] [int] NULL,
	[block_id] [nvarchar](255) NULL,
	[shape_id] [nvarchar](255) NULL,
	[wheelchair_accessible] [int] NULL,
	[bikes_allowed] [int] NULL,
CONSTRAINT [PK_GtfsStatic_Trips] PRIMARY KEY CLUSTERED 
(
	[trip_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [GtfsStatic].[Trips] table';
ELSE
BEGIN
	PRINT 'Error creating [GtfsStatic].[Trips] table; aborting';
	SET NOEXEC ON
END
GO


ALTER TABLE [GtfsStatic].[Trips]
ADD CONSTRAINT [FK_GtfsStatic_Trips_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_GtfsStatic_Trips_Download] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_GtfsStatic_Trips_Download] foreign key; aborting';
	SET NOEXEC ON
END
GO




