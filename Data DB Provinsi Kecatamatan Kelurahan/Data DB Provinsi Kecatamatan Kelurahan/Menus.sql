
SET IDENTITY_INSERT Menus ON
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Configuration', N'<i class="fa-solid fa-gear nav-icon"></i>', NULL, 7, NULL, NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'User', N'<i class="far fa-solid fa-user"></i>', N'Configuration', 1, NULL, N'config/user', NULL, '2024-01-24 02:24:59.5090381', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Group', N'<i class="far fa-solid fa-users"></i>', N'Configuration', 2, NULL, N'config/group', NULL, '2024-01-24 02:24:38.7660439', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Menu', N'<i class="fa-solid fa-bars"></i>', N'Configuration', 4, N'<li class="nav-item">', N'config/menu', NULL, '2024-01-24 02:24:41.6071169', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'Email Setting', N'<i class="fa-solid fa-envelope"></i>', N'Configuration', 5, NULL, N'config/email-setting', NULL, '2024-01-30 03:22:03.9060248', N'Ganggar', '2024-03-06 10:41:02.0002064');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'Email Template', N'<i class="fa-regular fa-envelope"></i>', N'Configuration', 4, NULL, N'config/email-template', NULL, '2024-01-24 02:24:36.0636577', N'Ganggar', '2024-03-06 10:41:09.1051720');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'Company', N'<i class="fa-solid fa-building"></i>', N'Configuration', 6, NULL, N'config/companies', NULL, '2024-01-24 02:24:23.0546426', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'Country', N'<i class="fa-solid fa-earth-americas"></i>', N'Configuration', 7, NULL, N'config/country', NULL, '2024-01-24 02:24:27.6593541', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Province', N'<i class="fa-solid fa-city"></i>', N'Configuration', 8, N'<li class="nav-item">', N'config/province', NULL, '2024-01-24 02:24:49.2879576', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'City', N'<i class="fa-solid fa-city"></i>', N'Configuration', 9, N'<li class="nav-item">', N'config/city', NULL, '2024-01-24 02:24:15.1160427', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'District', N'<i class="fa-solid fa-city"></i>', N'Configuration', 10, N'<li class="nav-item">', N'config/district', NULL, '2024-01-24 02:24:30.4999620', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'Sub-District', N'<i class="fa-solid fa-city"></i>', N'Configuration', 11, N'<li class="nav-item">', N'config/village', NULL, '2024-01-24 02:24:56.5840183', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'Religion', N'<i class="fa-solid fa-hands-praying"></i>', N'Configuration', 12, N'<li class="nav-item">', N'config/religion', NULL, '2024-01-24 02:24:51.7029724', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'Occupational', N'<i class="fa-solid fa-user-doctor"></i>', N'Configuration', 13, N'<li class="nav-item">', N'config/occupational', NULL, '2024-01-24 02:24:44.2854431', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, N'Transaction', N'<i class="fa-solid fa-money-check nav-icon"></i>', NULL, 1, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, N'General Consultation Service', NULL, N'Transaction', 1, N'<li class="nav-item">', N'transaction/general-consultan-service', NULL, NULL, N'Argi Purwanto', '2024-02-05 17:33:53.4210056');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, N'Accident Management', NULL, N'Transaction', 2, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, N'Telemedicine', NULL, N'Transaction', 3, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, N'Medical Check Up & Certificate Management', NULL, N'Transaction', 4, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, N'Education & Awareness Program', NULL, N'Transaction', 5, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (21, N'Maternity Check Up', NULL, N'Transaction', 6, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (22, N'Immunization & Vaccination Program', NULL, N'Transaction', 7, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (23, N'Cronic Diseases Management', NULL, N'Transaction', 9, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (24, N'Wellness Management', NULL, N'Transaction', 9, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (26, N'Queue Counter', NULL, N'Queue', 2, N'<li class="nav-item">', N'queue/queue-counter', NULL, NULL, N'Argi P', '2024-02-16 14:51:25.9207652');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (27, N'Queue Display', NULL, N'Queue', 3, N'<li class="nav-item">', N'queue/queue-display', NULL, NULL, N'Argi P', '2024-02-16 14:52:13.9867606');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (28, N'Reporting & Analytic', NULL, N'Transaction', 13, N'<li class="nav-item">', NULL, NULL, '2024-01-29 09:10:55.1275517', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (29, N'Patient', N'<i class="fa-solid fa-hospital-user nav-icon"></i>', NULL, 2, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (30, N'Patient Data', NULL, N'Patient', 1, N'<li class="nav-item">', N'patient/patient-data', NULL, '2024-01-24 07:05:24.7905418', N'Administrator', '2024-02-22 11:49:13.1538452');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (31, N'Family Relation', NULL, N'Patient', 2, N'<li class="nav-item">', N'patient/family-relation', NULL, '2024-01-29 08:18:07.9479786', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (32, N'Pharmacy', N'<i class="fa-solid fa-prescription-bottle-medical nav-icon"></i>', NULL, 3, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (34, N'Prescription', NULL, N'Pharmacy', 1, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (35, N'Prescription Template', NULL, N'Pharmacy', 2, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (36, N'Signa', NULL, N'Pharmacy', 3, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (37, N'Instructions', NULL, N'Pharmacy', 4, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (38, N'Active Component', NULL, N'Pharmacy', 5, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (39, N'Reporting Pharmacy', NULL, N'Pharmacy', 6, N'<li class="nav-item">

                        <NavLink href="Reporting" class="nav-link">

                            <i class="far fa-circle nav-icon"></i>

                            <p>Reporting</p>

                        </NavLink>

                    </li>', N'Reporting Pharmacy', NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (40, N'Inventory', N'<i class="fa-solid fa-box nav-icon"></i>', NULL, 4, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (41, N'Product', NULL, N'Inventory', 1, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (42, N'Product Category', NULL, N'Inventory', 2, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (43, N'Batch & Expired', NULL, N'Inventory', 3, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (44, N'Buffer Stock', NULL, N'Inventory', 4, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (45, N'Equipment & Consumable Management', NULL, N'Inventory', 5, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (46, N'Receiving Transaction', NULL, N'Inventory', 6, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (47, N'Inventory Adjusment', NULL, N'Inventory', 7, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (48, N'Stock Move', NULL, N'Inventory', 8, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (49, N'UoM', NULL, N'Inventory', 9, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (50, N'Uom Convertion', NULL, N'Inventory', 10, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (51, N'Reporting Inventory', NULL, N'Inventory', NULL, NULL, N'Reporting Inventory', NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (52, N'Employee', N'<i class="fa-solid fa-user nav-icon"></i>', NULL, 5, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (53, N'Employee Data', N'<i class="fa-solid fa-users"></i>', N'Employee', 1, N'<li class="nav-item">', N'employee/employees', NULL, NULL, N'Argi Purwanto', '2024-02-07 10:34:12.9824357');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (54, N'Sick Leave Management', N'<i class="fa-solid fa-viruses"></i>', N'Employee', 2, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (55, N'Claim Management', N'<i class="fa-solid fa-heart-circle-exclamation"></i>', N'Employee', 3, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (56, N'Department', N'<i class="fa-solid fa-building-user"></i>', N'Employee', 4, N'<li class="nav-item">', N'employee/department', NULL, '2024-01-25 06:40:07.3777916', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (57, N'Job Potition', N'<i class="fa-regular fa-id-card"></i>', N'Employee', 5, N'<li class="nav-item">', N'employee/job-position', NULL, '2024-01-25 06:38:55.3929407', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (58, N'Medical', N'<i class="fa-solid fa-notes-medical nav-icon"></i>', NULL, 6, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, N'Argi P', '2024-02-16 09:56:37.4368231');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (59, N'Practitioner', N'<i class="fa-solid fa-user-nurse"></i>', N'Medical', 1, N'<li class="nav-item">', N'medical/practitioner', NULL, NULL, N'Argi P', '2024-02-07 13:32:08.0604775');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (60, N'Doctor Scheduled', NULL, N'Medical', 2, N'<li class="nav-item">', N'medical/doctor-scheduled', NULL, NULL, N'Argi Purwanto', '2024-02-02 13:42:52.1299321');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (61, N'Practitioner License Number & Expired', NULL, N'Medical', 3, N'<li class="nav-item">', NULL, NULL, '2024-01-23 03:49:55.1872683', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (62, N'Insurance', NULL, N'Medical', 4, N'<li class="nav-item">', N'medical/insurance', NULL, '2024-01-26 03:52:57.7403764', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (63, N'Speciality', NULL, N'Medical', 5, N'<li class="nav-item">', N'medical/speciality', NULL, '2024-01-30 03:23:37.1435744', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (64, N'Diagnosis', NULL, N'Medical', 6, N'<li class="nav-item">', N'medical/diagnosis', NULL, '2024-01-26 09:49:39.2649833', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (65, N'Procedure', NULL, N'Medical', 7, N'<li class="nav-item">', N'medical/procedure', NULL, '2024-01-29 07:35:30.0861530', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (66, N'Chronic Diagnosis', NULL, N'Medical', 9, N'<li class="nav-item">', N'medical/cronis-category', NULL, '2024-01-26 08:47:44.6396330', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (67, N'Health Center', NULL, N'Medical', 10, N'<li class="nav-item">', N'medical/health-center', NULL, '2024-01-24 10:58:30.6638804', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (68, N'Building & Location', NULL, N'Medical', 11, N'<li class="nav-item">', N'medical/building', NULL, '2024-01-25 03:20:15.8164850', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (69, N'Disease category', NULL, N'Medical', 8, N'<li class="nav-item">', N'medical/disease-category', NULL, '2024-01-26 08:46:16.7014936', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (71, N'Service', NULL, N'Medical', 13, NULL, N'medical/service', NULL, '2024-01-25 07:54:55.8281004', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (72, N'Location', N'<i class="fa-solid fa-location-dot"></i>', N'Medical', 12, NULL, N'medical/location', NULL, '2024-01-29 04:47:45.7620716', NULL, NULL);
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (73, N'Doctor Schedule Slot', NULL, N'Medical', 2, NULL, N'medical/doctor-schedule-slot', N'Argi Purwanto', '2024-02-02 17:02:49.7902515', N'Argi Purwanto', '2024-02-05 16:09:08.0266483');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (74, N'Insurance Police', NULL, N'Patient', 3, NULL, N'patient/insurance-policy', N'Administrator', '2024-02-13 09:57:02.0760875', N'Administrator', '2024-02-15 17:59:18.3753545');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (76, N'Queue', NULL, NULL, 1, NULL, NULL, N'Argi P', '2024-02-16 14:47:04.8991689', N'Argi P', '2024-02-16 14:47:18.2368276');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (77, N'Kiosk Departement', NULL, N'Queue', 4, NULL, N'queue/kiosk-departement', N'Argi P', '2024-02-16 15:02:37.7604096', N'Argi P', '2024-02-20 11:35:14.7783636');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (78, N'Configuration Kiosk', NULL, N'Queue', 5, NULL, N'queue/configuration-kiosk', N'Argi P', '2024-02-16 15:08:51.2828635', N'Argi P', '2024-02-22 17:17:44.3477107');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1075, N'Nursing Diagnosis', NULL, N'Medical', 14, NULL, N'medical/nursing-diagnosis', N'Argi P', '2024-02-19 17:48:45.5780786', N'Administrator', '2024-02-21 17:31:31.4013276');
GO
INSERT INTO [Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1076, N'Template Page', NULL, NULL, 0, NULL, N'template-page', N'Nurse', '2024-03-04 10:29:53.9863389', NULL, NULL);
GO
SET IDENTITY_INSERT Menus OFF
GO