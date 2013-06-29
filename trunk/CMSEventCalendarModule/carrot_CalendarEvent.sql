
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_CalendarFrequency](
	[CalendarFrequencyID] [uniqueidentifier] NOT NULL,
	[FrequencySortOrder] [int] NOT NULL,	
	[FrequencyValue] [varchar](64) NOT NULL,
	[FrequencyName] [varchar](128) NOT NULL,
 CONSTRAINT [PK_carrot_CalendarFrequency] PRIMARY KEY CLUSTERED 
(
	[CalendarFrequencyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[carrot_CalendarFrequency] ADD  CONSTRAINT [DF_carrot_CalendarFrequency_CalendarFrequencyID]  DEFAULT (newid()) FOR [CalendarFrequencyID]
GO


IF not exists (select * from [dbo].[carrot_CalendarFrequency]) BEGIN

	INSERT [dbo].[carrot_CalendarFrequency] ([FrequencySortOrder], [FrequencyValue], [FrequencyName]) VALUES (1, N'Once', N'Once')
	INSERT [dbo].[carrot_CalendarFrequency] ([FrequencySortOrder], [FrequencyValue], [FrequencyName]) VALUES (2, N'Daily', N'Daily')
	INSERT [dbo].[carrot_CalendarFrequency] ([FrequencySortOrder], [FrequencyValue], [FrequencyName]) VALUES (3, N'Weekly', N'Weekly')
	INSERT [dbo].[carrot_CalendarFrequency] ([FrequencySortOrder], [FrequencyValue], [FrequencyName]) VALUES (4, N'Monthly', N'Monthly')
	INSERT [dbo].[carrot_CalendarFrequency] ([FrequencySortOrder], [FrequencyValue], [FrequencyName]) VALUES (5, N'Yearly', N'Yearly')

END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_CalendarEventCategory](
	[CalendarEventCategoryID] [uniqueidentifier] NOT NULL,
	[CategoryFGColor] [varchar](32) NOT NULL,
	[CategoryBGColor] [varchar](32) NOT NULL,
	[CategoryName] [varchar](128) NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,	
 CONSTRAINT [PK_carrot_CalendarEventCategory] PRIMARY KEY CLUSTERED 
(
	[CalendarEventCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[carrot_CalendarEventCategory] ADD  CONSTRAINT [DF_carrot_CalendarEventCategory_CalendarEventCategoryID]  DEFAULT (newid()) FOR [CalendarEventCategoryID]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_CalendarEventProfile](
	[CalendarEventProfileID] [uniqueidentifier] NOT NULL,
	[CalendarFrequencyID] [uniqueidentifier] NOT NULL,
	[CalendarEventCategoryID] [uniqueidentifier] NOT NULL,
	[EventStartDate] [datetime] NOT NULL,
	[EventStartTime] [time](7) NULL,
	[EventEndDate] [datetime] NOT NULL,
	[EventEndTime] [time](7) NULL,
	[EventTitle] [varchar](256) NULL,
	[EventDetail] [varchar](max) NULL,
	[EventRepeatPattern] [int] NULL,
	[IsAllDayEvent] [bit] NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[IsCancelled] [bit] NOT NULL,
	[IsCancelledPublic] [bit] NOT NULL,
	[SiteID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_carrot_CalendarEventProfile] PRIMARY KEY CLUSTERED 
(
	[CalendarEventProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[carrot_CalendarEventProfile] ADD  CONSTRAINT [DF_carrot_CalendarEvent_CalendarEventProfileID]  DEFAULT (newid()) FOR [CalendarEventProfileID]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[carrot_CalendarEvent](
	[CalendarEventID] [uniqueidentifier] NOT NULL,
	[CalendarEventProfileID] [uniqueidentifier] NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[EventDetail] [varchar](max) NULL,
	[IsCancelled] [bit] NOT NULL,
 CONSTRAINT [PK_carrot_CalendarEvent] PRIMARY KEY CLUSTERED 
(
	[CalendarEventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[carrot_CalendarEvent] ADD  CONSTRAINT [DF_carrot_CalendarEvent_CalendarEventID]  DEFAULT (newid()) FOR [CalendarEventID]
GO



ALTER TABLE [dbo].[carrot_CalendarEventProfile]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CalendarEventProfile_carrot_CalendarEventCategory] FOREIGN KEY([CalendarEventCategoryID])
REFERENCES [dbo].[carrot_CalendarEventCategory] ([CalendarEventCategoryID])
GO
ALTER TABLE [dbo].[carrot_CalendarEventProfile] CHECK CONSTRAINT [FK_carrot_CalendarEventProfile_carrot_CalendarEventCategory]
GO

ALTER TABLE [dbo].[carrot_CalendarEventProfile]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CalendarEventProfile_carrot_CalendarFrequency] FOREIGN KEY([CalendarFrequencyID])
REFERENCES [dbo].[carrot_CalendarFrequency] ([CalendarFrequencyID])
GO
ALTER TABLE [dbo].[carrot_CalendarEventProfile] CHECK CONSTRAINT [FK_carrot_CalendarEventProfile_carrot_CalendarFrequency]
GO


ALTER TABLE [dbo].[carrot_CalendarEvent]  WITH CHECK ADD  CONSTRAINT [FK_carrot_CalendarEvent_carrot_CalendarEventProfile] FOREIGN KEY([CalendarEventProfileID])
REFERENCES [dbo].[carrot_CalendarEventProfile] ([CalendarEventProfileID])
GO
ALTER TABLE [dbo].[carrot_CalendarEvent] CHECK CONSTRAINT [FK_carrot_CalendarEvent_carrot_CalendarEventProfile]
GO


--====================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_carrot_CalendarEvent]
AS 


SELECT ces.SiteID, ces.CalendarEventProfileID, ces.CalendarFrequencyID, ces.CalendarEventCategoryID, ces.EventStartDate, ces.EventStartTime, ces.EventEndDate, 
      ces.EventEndTime, ces.EventTitle, ces.EventRepeatPattern, ces.EventDetail as EventSeriesDetail, ces.IsCancelledPublic, ces.IsCancelled AS IsCancelledSeries, 
      ces.IsAllDayEvent, ces.IsPublic, ce.CalendarEventID, ce.EventDate, ce.EventDetail, ce.IsCancelled AS IsCancelledEvent, 
      cef.FrequencyValue, cef.FrequencyName, cef.FrequencySortOrder, cec.CategoryFGColor, cec.CategoryBGColor, cec.CategoryName
FROM carrot_CalendarEventProfile AS ces 
INNER JOIN carrot_CalendarEvent AS ce ON ces.CalendarEventProfileID = ce.CalendarEventProfileID 
INNER JOIN carrot_CalendarFrequency AS cef ON ces.CalendarFrequencyID = cef.CalendarFrequencyID 
INNER JOIN carrot_CalendarEventCategory AS cec ON ces.CalendarEventCategoryID = cec.CalendarEventCategoryID 
		AND ces.SiteID = cec.SiteID


GO

CREATE VIEW [dbo].[vw_carrot_CalendarEventProfile]
AS 


SELECT ces.SiteID, ces.CalendarEventProfileID, ces.CalendarFrequencyID, ces.CalendarEventCategoryID, ces.EventStartDate, ces.EventStartTime, ces.EventEndDate, 
      ces.EventEndTime, ces.EventTitle, ces.EventRepeatPattern, ces.EventDetail, ces.IsCancelled, ces.IsCancelledPublic, 
      ces.IsAllDayEvent, ces.IsPublic, cef.FrequencyValue, cef.FrequencyName, cef.FrequencySortOrder, cec.CategoryFGColor, cec.CategoryBGColor, cec.CategoryName
FROM carrot_CalendarEventProfile AS ces 
INNER JOIN carrot_CalendarFrequency AS cef ON ces.CalendarFrequencyID = cef.CalendarFrequencyID 
INNER JOIN carrot_CalendarEventCategory AS cec ON ces.CalendarEventCategoryID = cec.CalendarEventCategoryID 
		AND ces.SiteID = cec.SiteID


GO
