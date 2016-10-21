USE [MbtaTracker]
GO

/****** Object:  Table [GtfsStatic].[Calendar_Dates]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET NOEXEC OFF
GO

IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Calendar_Dates]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Calendar_Dates] table';
	DROP TABLE [GtfsStatic].[Calendar_Dates];
END;


CREATE TABLE [GtfsStatic].[Calendar_Dates](
	[calendar_dates_row_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[service_id] VARCHAR(255) NOT NULL,
	[exception_date] [datetime] NULL,
	[exception_type] [int] NULL,
CONSTRAINT [PK_GtfsStatic_Calendar_Dates] PRIMARY KEY CLUSTERED 
(
	[calendar_dates_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [GtfsStatic].[Calendar_Dates] table';
ELSE
BEGIN
	PRINT 'Error creating [GtfsStatic].[Calendar_Dates] table; aborting';
	SET NOEXEC ON
END
GO


GO


ALTER TABLE [GtfsStatic].[Calendar_Dates]
ADD CONSTRAINT [FK_GtfsStatic_Calendar_Dates_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);
GO

IF @@ERROR = 0
	PRINT 'Successfully created [FK_GtfsStatic_Calendar_Dates_Download] key';
ELSE
BEGIN
	PRINT 'Error creating [FK_GtfsStatic_Calendar_Dates_Download] key; aborting';
	SET NOEXEC ON;
END




