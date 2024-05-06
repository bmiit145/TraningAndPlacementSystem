SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStudentInterview](
	[id] [int] NULL,
	[student_id] [int] NULL,
	[interview_id] [int] NULL,
	[status] [int] NULL,
	[remark] [varchar](255) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblStudentInterview]  WITH CHECK ADD FOREIGN KEY([interview_id])
REFERENCES [dbo].[Interview] ([interview_id])
GO
ALTER TABLE [dbo].[tblStudentInterview]  WITH CHECK ADD FOREIGN KEY([student_id])
REFERENCES [dbo].[StudentProfile] ([id])
GO
