﻿ALTER TABLE [dbo].[tblCalendar]
    ADD CONSTRAINT [tblCalendar_PK] PRIMARY KEY CLUSTERED ([CalendarID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

