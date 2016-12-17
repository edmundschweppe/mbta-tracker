USE [MbtaTracker]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[GtfsRt].[FeedMessage]'))
BEGIN
	PRINT 'Dropping existing [GtfsRt].[FeedMessage] table';
	DROP TABLE [GtfsRt].[FeedMessage];
END;


CREATE TABLE [GtfsRt].[FeedMessage](
	[feed_msg_id] [int] IDENTITY(1,1) NOT NULL,
	[feed_hdr_gtfsrt_version] [nvarchar](100) NOT NULL,
	[FeedMessage_date] [datetime] NOT NULL,
 CONSTRAINT [PK_GtfsRt_FeedMessage] PRIMARY KEY CLUSTERED 
(
	[FeedMessage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


