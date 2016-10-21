USE [MbtaTracker]
GO

/****** Object:  Table [MbtaRt].[PredictionTripVehicle]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[MbtaRt].[PredictionTripVehicle]'))
BEGIN
	PRINT 'Dropping existing [MbtaRt].[PredictionTripVehicle] table';
	DROP TABLE [MbtaRt].[PredictionTripVehicle];
END;


CREATE TABLE [MbtaRt].[PredictionTripVehicle](
	[prediction_trip_vehicle_row_id] [int] IDENTITY(1,1) NOT NULL,
	[prediction_trip_row_id] [int] NOT NULL,
	[vehicle_id] [nvarchar](255) NOT NULL,
	[vehicle_lat] [float] NOT NULL,
	[vehicle_lon] [float] NOT NULL,
	[vehicle_bearing] [float] NULL,
	[vehicle_speed] [float] NULL,
	[vehicle_timestamp] [datetime] NOT NULL,
CONSTRAINT [PK_MbtaRt_PredictionTripVehicle] PRIMARY KEY CLUSTERED 
(
	[prediction_trip_vehicle_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [MbtaRt].[PredictionTripVehicle] table';
ELSE
BEGIN
	PRINT 'Error creating [MbtaRt].[PredictionTripVehicle] table; aborting';
	SET NOEXEC ON
END
GO


ALTER TABLE [MbtaRt].[PredictionTripVehicle]
ADD CONSTRAINT [FK_MbtaRt_PredictionTripVehicle_PredictionTrip] 
	FOREIGN KEY ([prediction_trip_row_id])
	REFERENCES [MbtaRt].[PredictionTrip] ([prediction_trip_row_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_MbtaRt_PredictionTripVehicle_PredictionTrip] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_MbtaRt_PredictionTripVehicle_PredictionTrip] foreign key; aborting';
	SET NOEXEC ON
END
GO


