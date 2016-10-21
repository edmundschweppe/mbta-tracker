USE [MbtaTracker]
GO

/****** Object:  Table [GtfsStatic].[Calendar]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Calendar]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Calendar] table';
	DROP TABLE [GtfsStatic].[Calendar];
END;


CREATE TABLE [GtfsStatic].[Calendar](
	[calendar_row_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[service_id] VARCHAR(255) NOT NULL,
	[monday] [int] NULL,
	[tuesday] [int] NULL,
	[wednesday] [int] NULL,
	[thursday] [int] NULL,
	[friday] [int] NULL,
	[saturday] [int] NULL,
	[sunday] [int] NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
CONSTRAINT [PK_GtfsStatic_Calendar] PRIMARY KEY CLUSTERED 
(
	[calendar_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [GtfsStatic].[Calendar] table';
ELSE
BEGIN
	PRINT 'Error creating [GtfsStatic].[Calendar] table; aborting';
	SET NOEXEC ON
END
GO


ALTER TABLE [GtfsStatic].[Calendar]
ADD CONSTRAINT [FK_GtfsStatic_Calendar_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);

IF @@ERROR = 0
	PRINT 'Successfully created [FK_GtfsStatic_Calendar_Download] key';
ELSE
BEGIN
	PRINT 'Error creating [FK_GtfsStatic_Calendar_Download] key; aborting';
	SET NOEXEC ON;
END

GO




