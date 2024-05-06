SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Playlists](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[description] [varchar](255) NULL,
	[courseId] [int] NOT NULL,
	[createdDate] [date] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Playlists] ADD PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Playlists]  WITH CHECK ADD  CONSTRAINT [FK_Playlist_Courses] FOREIGN KEY([courseId])
REFERENCES [dbo].[Courses] ([id])
GO
ALTER TABLE [dbo].[Playlists] CHECK CONSTRAINT [FK_Playlist_Courses]
GO
