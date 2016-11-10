USE [MbtaTracker]
GO

/****** Object:  Table [MbtaRt].[PredictionTrip]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[MbtaRt].[PredictionTrip]'))
BEGIN
	PRINT 'Dropping existing [MbtaRt].[PredictionTrip] table';
	DROP TABLE [MbtaRt].[PredictionTrip];
END;


CREATE TABLE [MbtaRt].[PredictionTrip](
	[prediction_trip_row_id] [int] IDENTITY(1,1) NOT NULL,
	[prediction_id] [int] NOT NULL,
	[route_id] [nvarchar](255) NOT NULL,
	[direction_id] [nvarchar](1) NULL,
	[trip_id] [nvarchar](255) NOT NULL,
	[trip_name] [nvarchar](255) NOT NULL,
	[trip_headsign] [nvarchar](255) NULL,

CONSTRAINT [PK_MbtaRt_PredictionTrip] PRIMARY KEY CLUSTERED 
(
	[prediction_trip_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [MbtaRt].[PredictionTrip] table';
ELSE
BEGIN
	PRINT 'Error creating [MbtaRt].[PredictionTrip] table; aborting';
	SET NOEXEC ON
END
GO


ALTER TABLE [MbtaRt].[PredictionTrip]
ADD CONSTRAINT [FK_MbtaRt_PredictionTrip_Predictions] 
	FOREIGN KEY ([prediction_id])
	REFERENCES [MbtaRt].[Predictions] ([prediction_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_MbtaRt_PredictionTrip_Predictions] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_MbtaRt_PredictionTrip_Predictions] foreign key; aborting';
	SET NOEXEC ON
END
GO


CREATE NONCLUSTERED INDEX [IX_MbtaRt_PredictionTrip_prediction_id]
ON [MbtaRt].[PredictionTrip] ([prediction_id])
INCLUDE ([route_id],[trip_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [IX_MbtaRt_PredictionTrip_prediction_id] index';
ELSE
BEGIN
	PRINT 'Error creating Successfully created [IX_MbtaRt_PredictionTrip_prediction_id] index';
	SET NOEXEC ON
END
GO
