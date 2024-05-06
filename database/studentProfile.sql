SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentProfile](
	[id] [int] NOT NULL,
	[enro] [bigint] NULL,
	[fname] [varchar](50) NULL,
	[lname] [varchar](50) NULL,
	[email] [varchar](max) NULL,
	[contact_no] [varchar](10) NULL,
	[CGPA] [float] NULL,
	[resume] [varchar](max) NULL,
	[marks9] [float] NULL,
	[marks10] [float] NULL,
	[marks11] [float] NULL,
	[marks12] [float] NULL,
	[is_approved] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentProfile] ADD  CONSTRAINT [PK_StudentProfile] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentProfile]  WITH CHECK ADD  CONSTRAINT [FK_StudentProfile_users] FOREIGN KEY([id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[StudentProfile] CHECK CONSTRAINT [FK_StudentProfile_users]
GO
