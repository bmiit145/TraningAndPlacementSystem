SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentInterview](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[student_id] [int] NULL,
	[interview_id] [int] NULL,
	[status] [int] NULL,
	[remark] [varchar](255) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentInterview] ADD  CONSTRAINT [PK_StudentInterview] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentInterview]  WITH CHECK ADD FOREIGN KEY([interview_id])
REFERENCES [dbo].[Interview] ([interview_id])
GO
ALTER TABLE [dbo].[StudentInterview]  WITH CHECK ADD FOREIGN KEY([student_id])
REFERENCES [dbo].[StudentProfile] ([id])
GO
