﻿SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserId], [Username], [UserPasswordHash], [RegisteredDate], [CountryId], [StateId], [JurisidictionId], [LanguagesId], [IsOnline], [AccountStatusId], [LicenseNumber], [emailAddress]) VALUES (1003, N'dave@ccs-labs.com', N'097dfd905dfa0e078883b7afcf7e653dde569bb1ed2ce3384d9c9ed7b85741d6e8d1b1a356318805d3c8b31b36a9916936d005d8134fb015d0392cf75cd7fa24', N'2015-03-05 22:44:18', 3, 2, 4, 1, 1, 2, N't2015zzz1                ', N'dave@ccs-labs.com')
INSERT INTO [dbo].[Users] ([UserId], [Username], [UserPasswordHash], [RegisteredDate], [CountryId], [StateId], [JurisidictionId], [LanguagesId], [IsOnline], [AccountStatusId], [LicenseNumber], [emailAddress]) VALUES (1006, N'system@ccs-labs.com', N'097dfd905dfa0e078883b7afcf7e653dde569bb1ed2ce3384d9c9ed7b85741d6e8d1b1a356318805d3c8b31b36a9916936d005d8134fb015d0392cf75cd7fa24', N'2015-03-05 22:44:18', 3, 2, 4, 1, 1, 2, N'000000000                ', N'system@ccs-labs.com')
SET IDENTITY_INSERT [dbo].[Users] OFF