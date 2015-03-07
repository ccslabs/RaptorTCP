SET IDENTITY_INSERT [dbo].[AccountStatus] ON
INSERT INTO [dbo].[AccountStatus] ([Id], [StatusName]) VALUES (1, N'Suspended           ')
INSERT INTO [dbo].[AccountStatus] ([Id], [StatusName]) VALUES (2, N'Unverified          ')
INSERT INTO [dbo].[AccountStatus] ([Id], [StatusName]) VALUES (3, N'Verified            ')
INSERT INTO [dbo].[AccountStatus] ([Id], [StatusName]) VALUES (4, N'Under Investigation ')
INSERT INTO [dbo].[AccountStatus] ([Id], [StatusName]) VALUES (5, N'Closed              ')
SET IDENTITY_INSERT [dbo].[AccountStatus] OFF
