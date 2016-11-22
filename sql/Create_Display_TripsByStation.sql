USE [MbtaTracker]
GO


/****** Object:  Table [Display].[TripsByStation]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO



IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[Display].[TripsByStation]'))
BEGIN
	PRINT 'Dropping existing [Display].[TripsByStation] table';
	DROP TABLE [Display].[TripsByStation];
END;


CREATE TABLE [Display].[TripsByStation](
	[trips_by_station_id] [int] IDENTITY(1,1) NOT NULL,
	[route_id] [nvarchar](255) NOT NULL,
	[route_name] [nvarchar](255) NOT NULL,
	[trip_id] [nvarchar](255) NOT NULL,
	[trip_shortname] [nvarchar](255) NOT NULL,
	[trip_headsign] [nvarchar](255) NOT NULL,
	[trip_direction] [int] null,
	[vehicle_id] [nvarchar](255) null,
	[stop_id] [nvarchar](255) NOT NULL,
	[stop_name] [nvarchar](255) NOT NULL,
	[sched_dep_dt] [datetime] not null,
	[pred_dt] [datetime] null,
	[pred_away] [int] null,
CONSTRAINT [PK_Display_TripsByStation] PRIMARY KEY CLUSTERED 
(
	[trips_by_station_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [Display].[TripsByStation] table';
ELSE
BEGIN
	PRINT 'Error creating [Display].[TripsByStation] table; aborting';
	SET NOEXEC ON
END
GO


CREATE NONCLUSTERED INDEX [IX_Display_TripsByStation_route_id]
ON [Display].[TripsByStation] ([route_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [IX_Display_TripsByStation_route_id] index';
ELSE
BEGIN
	PRINT 'Error creating [IX_Display_TripsByStation_route_id] index; aborting';
	SET NOEXEC ON
END
GO

CREATE NONCLUSTERED INDEX [IX_Display_TripsByStation_stop_id]
ON [Display].[TripsByStation] ([stop_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [IX_Display_TripsByStation_route_id] index';
ELSE
BEGIN
	PRINT 'Error creating [IX_Display_TripsByStation_route_id] index; aborting';
	SET NOEXEC ON
END
GO

