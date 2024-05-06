SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyHiring](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[company_id] [int] NULL,
	[hiring_id] [int] NULL,
	[max_apply] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CompanyHiring] ADD PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CompanyHiring]  WITH CHECK ADD FOREIGN KEY([company_id])
REFERENCES [dbo].[CompanyProfile] ([company_id])
GO
ALTER TABLE [dbo].[CompanyHiring]  WITH CHECK ADD FOREIGN KEY([hiring_id])
REFERENCES [dbo].[HiringProgram] ([id])
GO
