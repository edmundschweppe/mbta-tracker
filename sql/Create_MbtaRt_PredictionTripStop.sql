USE [MbtaTracker]
GO

/****** Object:  Table [MbtaRt].[PredictionTripStop]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[MbtaRt].[PredictionTripStop]'))
BEGIN
	PRINT 'Dropping existing [MbtaRt].[PredictionTripStop] table';
	DROP TABLE [MbtaRt].[PredictionTripStop];
END;


CREATE TABLE [MbtaRt].[PredictionTripStop](
	[prediction_trip_stop_row_id] [int] IDENTITY(1,1) NOT NULL,
	[prediction_trip_row_id] [int] NOT NULL,
	[stop_id] [nvarchar](255) NOT NULL,
	[stop_name] [nvarchar](255) NOT NULL,
	[stop_sequence] [int] NOT NULL,
	[sch_arr_dt] [datetime] NOT NULL,
	[sch_dep_dt] [datetime] NOT NULL,
	[pre_dt] [datetime] NOT NULL,
	[pre_away] [int] NOT NULL,

CONSTRAINT [PK_MbtaRt_PredictionTripStop] PRIMARY KEY CLUSTERED 
(
	[prediction_trip_stop_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [MbtaRt].[PredictionTripStop] table';
ELSE
BEGIN
	PRINT 'Error creating [MbtaRt].[PredictionTripStop] table; aborting';
	SET NOEXEC ON
END
GO


ALTER TABLE [MbtaRt].[PredictionTripStop]
ADD CONSTRAINT [FK_MbtaRt_PredictionTripStop_PredictionTrip] 
	FOREIGN KEY ([prediction_trip_row_id])
	REFERENCES [MbtaRt].[PredictionTrip] ([prediction_trip_row_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_MbtaRt_PredictionTripStop_PredictionTrip] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_MbtaRt_PredictionTripStop_PredictionTrip] foreign key; aborting';
	SET NOEXEC ON
END
GO

CREATE NONCLUSTERED INDEX [IX_PredictionTripStop_prediction_trip_row_id]
ON [MbtaRt].[PredictionTripStop] ([prediction_trip_row_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [IX_PredictionTripStop_prediction_trip_row_id] index';
ELSE
BEGIN
	PRINT 'Error creating [IX_PredictionTripStop_prediction_trip_row_id] index; aborting';
	SET NOEXEC ON
END
GO

