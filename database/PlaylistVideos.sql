SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaylistVideos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[link] [varchar](255) NOT NULL,
	[title] [varchar](max) NULL,
	[playlistId] [int] NULL,
	[courseId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[PlaylistVideos] ADD PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PlaylistVideos]  WITH CHECK ADD FOREIGN KEY([playlistId])
REFERENCES [dbo].[Playlists] ([id])
GO
ALTER TABLE [dbo].[PlaylistVideos]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistVideos_Courses] FOREIGN KEY([courseId])
REFERENCES [dbo].[Courses] ([id])
GO
ALTER TABLE [dbo].[PlaylistVideos] CHECK CONSTRAINT [FK_PlaylistVideos_Courses]
GO
