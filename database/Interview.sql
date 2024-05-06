SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interview](
	[interview_id] [int] IDENTITY(1,1) NOT NULL,
	[company_hiring_id] [int] NULL,
	[interview_date] [date] NULL,
	[interview_time] [time](7) NULL,
	[venue] [varchar](255) NULL,
	[interview_status] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Interview] ADD PRIMARY KEY CLUSTERED 
(
	[interview_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Interview] ADD  CONSTRAINT [DEFAULT_Interview_interview_status]  DEFAULT ((0)) FOR [interview_status]
GO
ALTER TABLE [dbo].[Interview]  WITH CHECK ADD FOREIGN KEY([company_hiring_id])
REFERENCES [dbo].[CompanyHiring] ([id])
GO
