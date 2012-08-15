/****** Object:  Table [dbo].[tblCalendar]    Script Date: 08/14/2012 21:59:26 ******/

CREATE TABLE [dbo].[tblCalendar](
	[CalendarID] [uniqueidentifier] NOT NULL,
	[EventDate] [datetime] NULL,
	[EventTitle] [varchar](255) NULL,
	[EventDetail] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[SiteID] [uniqueidentifier] NULL,
 CONSTRAINT [tblCalendar_PK_UC1] PRIMARY KEY CLUSTERED 
(
	[CalendarID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[tblCalendar] ADD  CONSTRAINT [DF_tblCalendar_CalendarID]  DEFAULT (newid()) FOR [CalendarID]
GO
