USE [MbtaTracker]
GO

/****** Object:  Table [MbtaRt].[Predictions]    Script Date: 9/12/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


SET NOEXEC OFF
GO


IF EXISTS(select * from sys.tables where object_id = OBJECT_ID('[MbtaRt].[Predictions]'))
BEGIN
	PRINT 'Dropping existing [MbtaRt].[Predictions] table';
	DROP TABLE [MbtaRt].[Predictions];
END;


CREATE TABLE [MbtaRt].[Predictions](
	[prediction_id] [int] IDENTITY(1,1) NOT NULL,
	[prediction_time] [datetime] NOT NULL,
	[prediction_json] [nvarchar](max) null,
CONSTRAINT [PK_MbtaRt_Predictions] PRIMARY KEY CLUSTERED 
(
	[prediction_id] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;
IF @@ERROR = 0
	PRINT 'Successfully created [MbtaRt].[Predictions] table';
ELSE
BEGIN
	PRINT 'Error creating [MbtaRt].[Predictions] table; aborting';
	SET NOEXEC ON
END
GO




