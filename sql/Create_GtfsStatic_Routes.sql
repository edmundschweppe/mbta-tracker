USE [MbtaTracker]
GO

/****** Object:  Table [GtfsStatic].[Gtfs.Routes]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsStatic].[Routes]'))
BEGIN
	PRINT 'Dropping existing [GtfsStatic].[Routes] table';
	DROP TABLE [GtfsStatic].[Routes];
END;


CREATE TABLE [GtfsStatic].[Routes](
	[route_row_id] [int] IDENTITY(1,1) NOT NULL,
	[download_id] [int] NOT NULL,
	[route_id] [nvarchar](50) NOT NULL,
	[agency_id] [nvarchar](50) NULL,
	[route_short_name] [nvarchar](250) NULL,
	[route_long_name] [nvarchar](250) NULL,
	[route_desc] [nvarchar](250) NULL,
	[route_type] [nvarchar](1) NULL,
	[route_url] [nvarchar](255) NULL,
	[route_color] [nvarchar](6) NULL,
	[route_text_color] [nvarchar](6) NULL,
CONSTRAINT [PK_GtfsStatic_Routes] PRIMARY KEY CLUSTERED 
(
	[route_row_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [GtfsStatic].[Routes] table';
ELSE
BEGIN
	PRINT 'Error creating [GtfsStatic].[Routes] table; aborting';
	SET NOEXEC ON
END
GO

ALTER TABLE [GtfsStatic].[Routes]
ADD CONSTRAINT [FK_GtfsStatic_Routes_Download] 
	FOREIGN KEY ([download_id])
	REFERENCES [GtfsStatic].[Download] ([download_id]);
IF @@ERROR = 0
	PRINT 'Successfully created [FK_GtfsStatic_Routes_Download] foreign key';
ELSE
BEGIN
	PRINT 'Error creating [FK_GtfsStatic_Routes_Download] foreign key; aborting';
	SET NOEXEC ON
END
GO




