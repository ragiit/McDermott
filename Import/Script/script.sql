USE [McDermott]
GO
SET IDENTITY_INSERT [dbo].[Locations] ON 

INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (1, NULL, 1, N'WH', N'Internal Location')
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (2, 1, 1, N'STOCK', N'Internal Location')
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (3, 1, 1, N'Pharmacy', N'Internal Location')
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (4, 1, 1, N'Lab', N'Internal Location')
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (6, 1, 1, N'Medis', N'Internal Location')
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (8, 1, 1, N'MCU', N'Internal Location')
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type]) VALUES (9, NULL, 1, N'Dental & Oral Clinic', N'Internal Location')
SET IDENTITY_INSERT [dbo].[Locations] OFF
GO
