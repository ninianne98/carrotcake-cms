
IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_CalendarEventProfile' and column_name = 'IsHoliday') BEGIN

	ALTER TABLE [dbo].[carrot_CalendarEventProfile] ADD [IsHoliday] bit NULL
	ALTER TABLE [dbo].[carrot_CalendarEventProfile] ADD [IsAnnualHoliday] bit NULL
	
END

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_CalendarEvent' and column_name = 'EventStartTime') BEGIN

	ALTER TABLE [dbo].[carrot_CalendarEvent] ADD [EventStartTime] [time](7) NULL
	ALTER TABLE [dbo].[carrot_CalendarEvent] ADD [EventEndTime] [time](7) NULL
	
END

IF NOT EXISTS( select * from information_schema.columns 
		where table_name = 'carrot_CalendarEventProfile' and column_name = 'RecursEvery') BEGIN

	ALTER TABLE [dbo].[carrot_CalendarEventProfile] ADD [RecursEvery] [int] NULL
	
END

GO


UPDATE [carrot_CalendarEventProfile]
SET [IsHoliday] = ISNULL([IsHoliday], 0)
WHERE IsNUll([IsHoliday], 0) = 0

UPDATE [carrot_CalendarEventProfile]
SET [IsAnnualHoliday] = ISNULL([IsAnnualHoliday], 0)
WHERE IsNUll([IsAnnualHoliday], 0) = 0

ALTER TABLE [dbo].[carrot_CalendarEventProfile] 
	ALTER COLUMN  [IsHoliday] [bit] NOT NULL

ALTER TABLE [dbo].[carrot_CalendarEventProfile] 
	ALTER COLUMN  [IsAnnualHoliday] [bit] NOT NULL


UPDATE [carrot_CalendarEventProfile]
SET [RecursEvery] = ISNULL([RecursEvery], 1)
WHERE IsNUll([RecursEvery], -100) = -100

ALTER TABLE [dbo].[carrot_CalendarEventProfile] 
	ALTER COLUMN  [RecursEvery] [int] NOT NULL

GO


--====================================

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vw_carrot_CalendarEvent]
AS 


SELECT ces.SiteID, ces.CalendarEventProfileID, ces.CalendarFrequencyID, ces.CalendarEventCategoryID, ces.EventStartDate, ces.EventEndDate, 
      ces.EventStartTime, ces.EventEndTime, ces.EventTitle, ces.EventRepeatPattern, ces.EventDetail as EventSeriesDetail, ces.IsCancelledPublic, 
      ces.IsAllDayEvent, ces.IsPublic, ces.IsCancelled AS IsCancelledSeries, ce.IsCancelled AS IsCancelledEvent, ces.IsHoliday, ces.IsAnnualHoliday, ces.RecursEvery,
      ce.CalendarEventID, ce.EventDate, ce.EventDetail, cef.FrequencyValue, cef.FrequencyName, cef.FrequencySortOrder, 
      ce.EventStartTime as EventStartTimeOverride, ce.EventEndTime as EventEndTimeOverride, cec.CategoryFGColor, cec.CategoryBGColor, cec.CategoryName
FROM dbo.carrot_CalendarEventProfile AS ces 
INNER JOIN dbo.carrot_CalendarEvent AS ce ON ces.CalendarEventProfileID = ce.CalendarEventProfileID 
INNER JOIN dbo.carrot_CalendarFrequency AS cef ON ces.CalendarFrequencyID = cef.CalendarFrequencyID 
INNER JOIN dbo.carrot_CalendarEventCategory AS cec ON ces.CalendarEventCategoryID = cec.CalendarEventCategoryID 
		AND ces.SiteID = cec.SiteID


GO

ALTER VIEW [dbo].[vw_carrot_CalendarEventProfile]
AS 


SELECT ces.SiteID, ces.CalendarEventProfileID, ces.CalendarFrequencyID, ces.CalendarEventCategoryID, ces.EventStartDate, ces.EventStartTime, ces.EventEndDate, 
      ces.EventEndTime, ces.EventTitle, ces.EventRepeatPattern, ces.EventDetail, ces.IsCancelled, ces.IsCancelledPublic, ces.IsHoliday, ces.IsAnnualHoliday, ces.RecursEvery,
      ces.IsAllDayEvent, ces.IsPublic, cef.FrequencyValue, cef.FrequencyName, cef.FrequencySortOrder, cec.CategoryFGColor, cec.CategoryBGColor, cec.CategoryName
FROM dbo.carrot_CalendarEventProfile AS ces 
INNER JOIN dbo.carrot_CalendarFrequency AS cef ON ces.CalendarFrequencyID = cef.CalendarFrequencyID 
INNER JOIN dbo.carrot_CalendarEventCategory AS cec ON ces.CalendarEventCategoryID = cec.CalendarEventCategoryID 
		AND ces.SiteID = cec.SiteID


GO
