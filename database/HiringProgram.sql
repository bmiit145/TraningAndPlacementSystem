SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HiringProgram](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[program_name] [varchar](255) NOT NULL,
	[description] [varchar](255) NULL,
	[start_date] [date] NULL,
	[end_date] [date] NULL,
	[course_id] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HiringProgram] ADD PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HiringProgram]  WITH CHECK ADD FOREIGN KEY([course_id])
REFERENCES [dbo].[Courses] ([id])
GO
