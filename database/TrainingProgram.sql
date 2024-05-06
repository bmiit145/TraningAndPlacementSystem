SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingProgram](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[description] [varchar](255) NULL,
	[course_id] [int] NULL,
	[link] [varchar](255) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TrainingProgram] ADD PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TrainingProgram]  WITH CHECK ADD FOREIGN KEY([course_id])
REFERENCES [dbo].[Courses] ([id])
GO
