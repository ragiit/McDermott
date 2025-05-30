USE [DbMcDermott]
GO
/****** Object:  Table [dbo].[Accidents]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accidents](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultanServiceId] [bigint] NOT NULL,
	[SafetyPersonnelId] [bigint] NOT NULL,
	[DateOfOccurrence] [datetime2](7) NOT NULL,
	[DateOfFirstTreatment] [datetime2](7) NOT NULL,
	[RibbonSpecialCase] [bit] NOT NULL,
	[Sent] [nvarchar](max) NULL,
	[EmployeeClass] [nvarchar](max) NULL,
	[EstimatedDisability] [nvarchar](max) NULL,
	[AreaOfYard] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[EmployeeDescription] [nvarchar](max) NULL,
	[AccidentLocation] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury1] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury2] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury3] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury4] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury5] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury6] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury7] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury8] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury9] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury10] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury11] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury12] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury13] [nvarchar](max) NOT NULL,
	[SelectedEmployeeCauseOfInjury14] [nvarchar](max) NOT NULL,
	[EmployeeCauseOfInjury1] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury2] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury3] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury4] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury5] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury6] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury7] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury8] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury9] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury10] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury11] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury12] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury13] [nvarchar](max) NULL,
	[EmployeeCauseOfInjury14] [nvarchar](max) NULL,
	[SelectedNatureOfInjury1] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury2] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury3] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury4] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury5] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury6] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury7] [nvarchar](max) NOT NULL,
	[SelectedNatureOfInjury8] [nvarchar](max) NOT NULL,
	[NatureOfInjury1] [nvarchar](max) NULL,
	[NatureOfInjury2] [nvarchar](max) NULL,
	[NatureOfInjury3] [nvarchar](max) NULL,
	[NatureOfInjury4] [nvarchar](max) NULL,
	[NatureOfInjury5] [nvarchar](max) NULL,
	[NatureOfInjury6] [nvarchar](max) NULL,
	[NatureOfInjury7] [nvarchar](max) NULL,
	[NatureOfInjury8] [nvarchar](max) NULL,
	[SelectedPartOfBody1] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody2] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody3] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody4] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody5] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody6] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody7] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody8] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody9] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody10] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody11] [nvarchar](max) NOT NULL,
	[SelectedPartOfBody12] [nvarchar](max) NOT NULL,
	[PartOfBody1] [nvarchar](max) NULL,
	[PartOfBody2] [nvarchar](max) NULL,
	[PartOfBody3] [nvarchar](max) NULL,
	[PartOfBody4] [nvarchar](max) NULL,
	[PartOfBody5] [nvarchar](max) NULL,
	[PartOfBody6] [nvarchar](max) NULL,
	[PartOfBody7] [nvarchar](max) NULL,
	[PartOfBody8] [nvarchar](max) NULL,
	[PartOfBody9] [nvarchar](max) NULL,
	[PartOfBody10] [nvarchar](max) NULL,
	[PartOfBody11] [nvarchar](max) NULL,
	[PartOfBody12] [nvarchar](max) NULL,
	[SelectedTreatment1] [nvarchar](max) NOT NULL,
	[SelectedTreatment2] [nvarchar](max) NOT NULL,
	[SelectedTreatment3] [nvarchar](max) NOT NULL,
	[SelectedTreatment4] [nvarchar](max) NOT NULL,
	[SelectedTreatment5] [nvarchar](max) NOT NULL,
	[SelectedTreatment6] [nvarchar](max) NOT NULL,
	[SelectedTreatment7] [nvarchar](max) NOT NULL,
	[Treatment1] [nvarchar](max) NULL,
	[Treatment2] [nvarchar](max) NULL,
	[Treatment3] [nvarchar](max) NULL,
	[Treatment4] [nvarchar](max) NULL,
	[Treatment5] [nvarchar](max) NULL,
	[Treatment6] [nvarchar](max) NULL,
	[Treatment7] [nvarchar](max) NULL,
	[EmployeeId] [bigint] NULL,
	[DepartmentId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Accidents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActiveComponentMedicamentGroupDetail]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveComponentMedicamentGroupDetail](
	[ActiveComponentId] [bigint] NOT NULL,
	[MedicamentGroupDetailsId] [bigint] NOT NULL,
 CONSTRAINT [PK_ActiveComponentMedicamentGroupDetail] PRIMARY KEY CLUSTERED 
(
	[ActiveComponentId] ASC,
	[MedicamentGroupDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActiveComponents]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveComponents](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UomId] [bigint] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[AmountOfComponent] [nvarchar](max) NULL,
	[ConcoctionLineId] [bigint] NULL,
	[MedicamentId] [bigint] NULL,
	[PrescriptionId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ActiveComponents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Allergies]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Allergies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KdAllergy] [nvarchar](max) NOT NULL,
	[NmAllergy] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Allergies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Awarenesses]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Awarenesses](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KdSadar] [nvarchar](max) NOT NULL,
	[NmSadar] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Awarenesses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BpjsClassifications]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BpjsClassifications](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BpjsClassifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BPJSIntegrations]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BPJSIntegrations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[InsurancePolicyId] [bigint] NULL,
	[NoKartu] [nvarchar](max) NULL,
	[Nama] [nvarchar](max) NULL,
	[HubunganKeluarga] [nvarchar](max) NULL,
	[Sex] [nvarchar](max) NULL,
	[TglLahir] [datetime2](7) NULL,
	[TglMulaiAktif] [datetime2](7) NULL,
	[TglAkhirBerlaku] [datetime2](7) NULL,
	[GolDarah] [nvarchar](max) NULL,
	[NoHP] [nvarchar](max) NULL,
	[NoKTP] [nvarchar](max) NULL,
	[PstProl] [nvarchar](max) NULL,
	[PstPrb] [nvarchar](max) NULL,
	[Aktif] [bit] NOT NULL,
	[KetAktif] [nvarchar](max) NULL,
	[Tunggakan] [int] NOT NULL,
	[KdProviderPstKdProvider] [nvarchar](max) NULL,
	[KdProviderPstNmProvider] [nvarchar](max) NULL,
	[KdProviderGigiKdProvider] [nvarchar](max) NULL,
	[KdProviderGigiNmProvider] [nvarchar](max) NULL,
	[JnsKelasNama] [nvarchar](max) NULL,
	[JnsKelasKode] [nvarchar](max) NULL,
	[JnsPesertaNama] [nvarchar](max) NULL,
	[JnsPesertaKode] [nvarchar](max) NULL,
	[AsuransiKdAsuransi] [nvarchar](max) NULL,
	[AsuransiNmAsuransi] [nvarchar](max) NULL,
	[AsuransiNoAsuransi] [nvarchar](max) NULL,
	[AsuransiCob] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BPJSIntegrations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BuildingLocations]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuildingLocations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BuildingId] [bigint] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BuildingLocations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HealthCenterId] [bigint] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Buildings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProvinceId] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassTypes]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ClassTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CityId] [bigint] NULL,
	[ProvinceId] [bigint] NULL,
	[CountryId] [bigint] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](200) NULL,
	[Website] [nvarchar](500) NULL,
	[VAT] [nvarchar](200) NULL,
	[Street1] [nvarchar](max) NULL,
	[Street2] [nvarchar](max) NULL,
	[Zip] [nvarchar](200) NULL,
	[CurrencyId] [bigint] NULL,
	[Logo] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConcoctionLines]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConcoctionLines](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ConcoctionId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[ActiveComponentId] [nvarchar](max) NULL,
	[UomId] [bigint] NULL,
	[MedicamentDosage] [bigint] NULL,
	[MedicamentUnitOfDosage] [bigint] NULL,
	[Dosage] [bigint] NULL,
	[TotalQty] [bigint] NULL,
	[AvaliableQty] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ConcoctionLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Concoctions]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Concoctions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PharmacyId] [bigint] NULL,
	[MedicamentGroupId] [bigint] NULL,
	[PractitionerId] [bigint] NULL,
	[DrugFormId] [bigint] NULL,
	[DrugRouteId] [bigint] NULL,
	[ConcoctionQty] [bigint] NULL,
	[DrugDosageId] [bigint] NULL,
	[MedicamenName] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Concoctions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Counters]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Counters](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ServiceId] [bigint] NULL,
	[ServiceKId] [bigint] NULL,
	[PhysicianId] [bigint] NULL,
	[Status] [nvarchar](max) NULL,
	[QueueDisplayId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Counters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](5) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CronisCategories]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CronisCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_CronisCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Degrees]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Degrees](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Degrees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [bigint] NULL,
	[ParentDepartmentId] [bigint] NULL,
	[ManagerId] [bigint] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[DepartmentCategory] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetailQueueDisplays]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailQueueDisplays](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KioskQueueId] [bigint] NULL,
	[ServicekId] [bigint] NULL,
	[ServiceId] [bigint] NULL,
	[NumberQueue] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DetailQueueDisplays] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diagnoses]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diagnoses](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[DiseaseCategoryId] [bigint] NULL,
	[CronisCategoryId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Diagnoses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DiseaseCategories]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DiseaseCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[ParentCategory] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DiseaseCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Districts]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Districts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CityId] [bigint] NOT NULL,
	[ProvinceId] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Districts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DoctorScheduleDetails]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorScheduleDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DoctorScheduleId] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[DayOfWeek] [nvarchar](200) NOT NULL,
	[WorkFrom] [time](7) NOT NULL,
	[WorkTo] [time](7) NOT NULL,
	[Quota] [bigint] NOT NULL,
	[UpdateToBpjs] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DoctorScheduleDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DoctorSchedules]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorSchedules](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[ServiceId] [bigint] NOT NULL,
	[PhysicionIds] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DoctorSchedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DoctorScheduleSlots]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DoctorScheduleSlots](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DoctorScheduleId] [bigint] NOT NULL,
	[PhysicianId] [bigint] NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[WorkFrom] [time](7) NOT NULL,
	[WorkTo] [time](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DoctorScheduleSlots] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DrugDosages]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DrugDosages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DrugRouteId] [bigint] NULL,
	[Frequency] [nvarchar](max) NOT NULL,
	[TotalQtyPerDay] [real] NOT NULL,
	[Days] [real] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DrugDosages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DrugRoutes]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DrugRoutes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Route] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_DrugRoutes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSettings]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](300) NULL,
	[Sequence] [bigint] NULL,
	[Smpt_Debug] [bit] NULL,
	[Smtp_Encryption] [nvarchar](max) NULL,
	[Smtp_Host] [nvarchar](200) NULL,
	[Smtp_Pass] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[Smtp_Port] [nvarchar](max) NULL,
	[Smtp_User] [nvarchar](200) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_EmailSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplates](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](200) NULL,
	[From] [nvarchar](200) NULL,
	[ById] [bigint] NULL,
	[To] [nvarchar](200) NULL,
	[ToPartnerId] [bigint] NULL,
	[Cc] [nvarchar](200) NULL,
	[ReplayTo] [nvarchar](max) NULL,
	[Schendule] [datetime2](7) NULL,
	[Message] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[DocumentContent] [varbinary](max) NULL,
	[EmailFromId] [bigint] NULL,
	[TypeEmail] [bigint] NULL,
 CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Families]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Families](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[ParentRelation] [nvarchar](max) NULL,
	[ChildRelation] [nvarchar](max) NULL,
	[Relation] [nvarchar](200) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Families] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FormDrugs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FormDrugs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_FormDrugs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genders]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genders](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Genders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralConsultanCPPTs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralConsultanCPPTs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultanServiceId] [bigint] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_GeneralConsultanCPPTs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralConsultanMedicalSupports]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralConsultanMedicalSupports](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultanServiceId] [bigint] NULL,
	[PractitionerLabEximinationId] [bigint] NULL,
	[LabEximinationName] [nvarchar](max) NULL,
	[LabResulLabExaminationtId] [bigint] NULL,
	[LabResulLabExaminationtIds] [nvarchar](max) NULL,
	[LabEximinationAttachment] [nvarchar](max) NULL,
	[PractitionerRadiologyEximinationId] [bigint] NULL,
	[RadiologyEximinationName] [nvarchar](max) NULL,
	[RadiologyEximinationAttachment] [nvarchar](max) NULL,
	[PractitionerAlcoholEximinationId] [bigint] NULL,
	[AlcoholEximinationName] [nvarchar](max) NULL,
	[AlcoholEximinationAttachment] [nvarchar](max) NULL,
	[AlcoholNegative] [bit] NULL,
	[AlcoholPositive] [bit] NULL,
	[PractitionerDrugEximinationId] [bigint] NULL,
	[DrugEximinationName] [nvarchar](max) NULL,
	[DrugEximinationAttachment] [nvarchar](max) NULL,
	[DrugNegative] [bit] NULL,
	[DrugPositive] [bit] NULL,
	[AmphetaminesNegative] [bit] NULL,
	[AmphetaminesPositive] [bit] NULL,
	[BenzodiazepinesNegative] [bit] NULL,
	[BenzodiazepinesPositive] [bit] NULL,
	[CocaineMetabolitesNegative] [bit] NULL,
	[CocaineMetabolitesPositive] [bit] NULL,
	[OpiatesNegative] [bit] NULL,
	[OpiatesPositive] [bit] NULL,
	[MethamphetaminesNegative] [bit] NULL,
	[MethamphetaminesPositive] [bit] NULL,
	[THCCannabinoidMarijuanaNegative] [bit] NULL,
	[THCCannabinoidMarijuanaPositive] [bit] NULL,
	[OtherExaminationAttachment] [nvarchar](max) NULL,
	[ECGAttachment] [nvarchar](max) NULL,
	[IsOtherExaminationECG] [bit] NOT NULL,
	[OtherExaminationTypeECG] [nvarchar](max) NULL,
	[OtherExaminationRemarkECG] [nvarchar](max) NULL,
	[PractitionerECGId] [bigint] NULL,
	[IsNormalRestingECG] [bit] NOT NULL,
	[IsSinusRhythm] [bit] NOT NULL,
	[IsSinusBradycardia] [bit] NOT NULL,
	[IsSinusTachycardia] [bit] NOT NULL,
	[EmployeeId] [bigint] NULL,
	[IsFirstTimeEnteringConfinedSpace] [bit] NOT NULL,
	[EnteringConfinedSpaceCount] [bigint] NOT NULL,
	[IsDefectiveSenseOfSmell] [bit] NOT NULL,
	[IsAsthmaOrLungAilment] [bit] NOT NULL,
	[IsBackPainOrLimitationOfMobility] [bit] NOT NULL,
	[IsClaustrophobia] [bit] NOT NULL,
	[IsDiabetesOrHypoglycemia] [bit] NOT NULL,
	[IsEyesightProblem] [bit] NOT NULL,
	[IsFaintingSpellOrSeizureOrEpilepsy] [bit] NOT NULL,
	[IsHearingDisorder] [bit] NOT NULL,
	[IsHeartDiseaseOrDisorder] [bit] NOT NULL,
	[IsHighBloodPressure] [bit] NOT NULL,
	[IsLowerLimbsDeformity] [bit] NOT NULL,
	[IsMeniereDiseaseOrVertigo] [bit] NOT NULL,
	[RemarksMedicalHistory] [nvarchar](max) NULL,
	[DateMedialHistory] [datetime2](7) NULL,
	[SignatureEmployeeId] [bigint] NULL,
	[SignatureEmployeeImagesMedicalHistory] [varbinary](max) NULL,
	[SignatureEmployeeImagesMedicalHistoryBase64] [nvarchar](max) NULL,
	[Wt] [bigint] NULL,
	[Bp] [bigint] NULL,
	[Height] [bigint] NULL,
	[Pulse] [bigint] NULL,
	[ChestCircumference] [bigint] NULL,
	[AbdomenCircumference] [bigint] NULL,
	[RespiratoryRate] [bigint] NULL,
	[Temperature] [bigint] NULL,
	[IsConfinedSpace] [bit] NOT NULL,
	[Eye] [nvarchar](max) NULL,
	[EarNoseThroat] [nvarchar](max) NULL,
	[Cardiovascular] [nvarchar](max) NULL,
	[Respiratory] [nvarchar](max) NULL,
	[Abdomen] [nvarchar](max) NULL,
	[Extremities] [nvarchar](max) NULL,
	[Musculoskeletal] [nvarchar](max) NULL,
	[Neurologic] [nvarchar](max) NULL,
	[SpirometryTest] [nvarchar](max) NULL,
	[RespiratoryFitTest] [nvarchar](max) NULL,
	[Size] [bigint] NULL,
	[Comment] [nvarchar](max) NULL,
	[Recommendeds] [nvarchar](max) NOT NULL,
	[DateEximinedbyDoctor] [datetime2](7) NULL,
	[SignatureEximinedDoctor] [varbinary](max) NULL,
	[SignatureEximinedDoctorBase64] [nvarchar](max) NULL,
	[Recommended] [nvarchar](max) NULL,
	[ExaminedPhysicianId] [bigint] NULL,
	[HR] [bigint] NOT NULL,
	[IsOtherECG] [bit] NOT NULL,
	[OtherDesc] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[LabTestId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_GeneralConsultanMedicalSupports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralConsultanServices]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralConsultanServices](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KioskQueueId] [bigint] NULL,
	[PatientId] [bigint] NULL,
	[InsurancePolicyId] [bigint] NULL,
	[ServiceId] [bigint] NULL,
	[PratitionerId] [bigint] NULL,
	[ClassTypeId] [bigint] NULL,
	[Method] [nvarchar](max) NULL,
	[AdmissionQueue] [nvarchar](max) NULL,
	[Payment] [nvarchar](max) NULL,
	[TypeRegistration] [nvarchar](max) NULL,
	[HomeStatus] [nvarchar](max) NULL,
	[TypeMedical] [nvarchar](max) NULL,
	[ScheduleTime] [nvarchar](max) NULL,
	[IsAlertInformationSpecialCase] [bit] NOT NULL,
	[IsSickLeave] [bit] NOT NULL,
	[IsMaternityLeave] [bit] NOT NULL,
	[StartDateSickLeave] [datetime2](7) NULL,
	[EndDateSickLeave] [datetime2](7) NULL,
	[StartMaternityLeave] [datetime2](7) NULL,
	[EndMaternityLeave] [datetime2](7) NULL,
	[RegistrationDate] [datetime2](7) NULL,
	[AppointmentDate] [datetime2](7) NULL,
	[WorkFrom] [time](7) NULL,
	[WorkTo] [time](7) NULL,
	[SerialNo] [nvarchar](max) NULL,
	[ReferVerticalKhususCategoryName] [nvarchar](max) NULL,
	[ReferVerticalKhususCategoryCode] [nvarchar](max) NULL,
	[ReferVerticalSpesialisParentSpesialisName] [nvarchar](max) NULL,
	[ReferVerticalSpesialisParentSpesialisCode] [nvarchar](max) NULL,
	[ReferVerticalSpesialisParentSubSpesialisName] [nvarchar](max) NULL,
	[ReferVerticalSpesialisParentSubSpesialisCode] [nvarchar](max) NULL,
	[ReferReason] [nvarchar](max) NULL,
	[IsSarana] [bit] NULL,
	[ReferVerticalSpesialisSaranaName] [nvarchar](max) NULL,
	[ReferVerticalSpesialisSaranaCode] [nvarchar](max) NULL,
	[PPKRujukanName] [nvarchar](max) NULL,
	[PPKRujukanCode] [nvarchar](max) NULL,
	[ReferDateVisit] [datetime2](7) NULL,
	[MedexType] [nvarchar](max) NULL,
	[IsMcu] [bit] NOT NULL,
	[IsBatam] [bit] NOT NULL,
	[IsOutsideBatam] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[StatusMCU] [int] NOT NULL,
	[McuExaminationDocs] [nvarchar](max) NULL,
	[McuExaminationBase64] [nvarchar](max) NULL,
	[AccidentExaminationDocs] [nvarchar](max) NULL,
	[AccidentExaminationBase64] [nvarchar](max) NULL,
	[Weight] [float] NOT NULL,
	[Height] [float] NOT NULL,
	[RR] [bigint] NOT NULL,
	[Temp] [bigint] NOT NULL,
	[HR] [bigint] NOT NULL,
	[PainScale] [bigint] NOT NULL,
	[Systolic] [bigint] NOT NULL,
	[DiastolicBP] [bigint] NOT NULL,
	[SpO2] [bigint] NOT NULL,
	[Diastole] [bigint] NOT NULL,
	[WaistCircumference] [bigint] NOT NULL,
	[BMIIndex] [float] NOT NULL,
	[BMIIndexString] [nvarchar](max) NOT NULL,
	[BMIState] [nvarchar](max) NOT NULL,
	[ClinicVisitTypes] [nvarchar](max) NOT NULL,
	[AwarenessId] [bigint] NULL,
	[E] [bigint] NOT NULL,
	[V] [bigint] NOT NULL,
	[M] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[RiskOfFallingDetail] [nvarchar](max) NULL,
	[ScrinningTriageScale] [nvarchar](max) NULL,
	[InformationFrom] [nvarchar](max) NULL,
	[RiskOfFalling] [nvarchar](max) NULL,
	[ProjectId] [bigint] NULL,
 CONSTRAINT [PK_GeneralConsultanServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralConsultantClinicalAssesments]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralConsultantClinicalAssesments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultanServiceId] [bigint] NULL,
	[Weight] [float] NOT NULL,
	[Height] [float] NOT NULL,
	[RR] [bigint] NOT NULL,
	[Temp] [bigint] NOT NULL,
	[HR] [bigint] NOT NULL,
	[PainScale] [bigint] NOT NULL,
	[Systolic] [bigint] NOT NULL,
	[DiastolicBP] [bigint] NOT NULL,
	[SpO2] [bigint] NOT NULL,
	[Sistole] [bigint] NOT NULL,
	[Diastole] [bigint] NOT NULL,
	[WaistCircumference] [bigint] NOT NULL,
	[BMIIndex] [float] NOT NULL,
	[BMIIndexString] [nvarchar](max) NOT NULL,
	[BMIState] [nvarchar](max) NOT NULL,
	[ClinicVisitTypes] [nvarchar](max) NULL,
	[AwarenessId] [bigint] NULL,
	[E] [bigint] NOT NULL,
	[V] [bigint] NOT NULL,
	[M] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_GeneralConsultantClinicalAssesments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GeneralConsultationLogs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralConsultationLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultanServiceId] [bigint] NULL,
	[ProcedureRoomId] [bigint] NULL,
	[UserById] [bigint] NULL,
	[Status] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_GeneralConsultationLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupMenus]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMenus](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[MenuId] [bigint] NOT NULL,
	[Create] [bit] NULL,
	[Read] [bit] NULL,
	[Update] [bit] NULL,
	[Delete] [bit] NULL,
	[Import] [bit] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_GroupMenus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthCenters]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthCenters](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CityId] [bigint] NULL,
	[ProvinceId] [bigint] NULL,
	[CountryId] [bigint] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Street1] [nvarchar](max) NULL,
	[Street2] [nvarchar](max) NULL,
	[WebsiteLink] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_HealthCenters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InsurancePolicies]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsurancePolicies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[InsuranceId] [bigint] NOT NULL,
	[PolicyNumber] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[Prolanis] [nvarchar](200) NULL,
	[ParticipantName] [nvarchar](200) NULL,
	[NoCard] [nvarchar](200) NULL,
	[NoId] [nvarchar](200) NULL,
	[Sex] [nvarchar](5) NULL,
	[Class] [nvarchar](200) NULL,
	[MedicalRecordNo] [nvarchar](200) NULL,
	[ServicePPKName] [nvarchar](200) NULL,
	[ServicePPKCode] [nvarchar](200) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[NursingClass] [nvarchar](200) NULL,
	[Diagnosa] [nvarchar](200) NULL,
	[Poly] [nvarchar](200) NULL,
	[Doctor] [nvarchar](200) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[CardPrintDate] [datetime2](7) NULL,
	[TmtDate] [datetime2](7) NULL,
	[TatDate] [datetime2](7) NULL,
	[ParticipantStatus] [nvarchar](200) NULL,
	[ServiceType] [nvarchar](200) NULL,
	[ServiceParticipant] [nvarchar](200) NULL,
	[CurrentAge] [datetime2](7) NULL,
	[AgeAtTimeOfService] [datetime2](7) NULL,
	[DinSos] [nvarchar](200) NULL,
	[PronalisPBR] [nvarchar](200) NULL,
	[NoSKTM] [nvarchar](200) NULL,
	[InsuranceNo] [nvarchar](200) NULL,
	[InsuranceName] [nvarchar](200) NULL,
	[ProviderName] [nvarchar](200) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_InsurancePolicies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Insurances]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Insurances](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Type] [nvarchar](max) NULL,
	[IsBPJSKesehatan] [bit] NOT NULL,
	[IsBPJSTK] [bit] NOT NULL,
	[AdminFee] [bigint] NULL,
	[Presentase] [bigint] NULL,
	[AdminFeeMax] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Insurances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryAdjusmentDetails]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryAdjusmentDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StockProductId] [bigint] NULL,
	[TransactionStockId] [bigint] NULL,
	[InventoryAdjusmentId] [bigint] NOT NULL,
	[ProductId] [bigint] NULL,
	[ExpiredDate] [datetime2](7) NOT NULL,
	[TeoriticalQty] [bigint] NOT NULL,
	[Batch] [nvarchar](max) NULL,
	[RealQty] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_InventoryAdjusmentDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InventoryAdjusments]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InventoryAdjusments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Reference] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_InventoryAdjusments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobPositions]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobPositions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [bigint] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_JobPositions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KioskConfigs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KioskConfigs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ServiceIds] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_KioskConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KioskQueues]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KioskQueues](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[KioskId] [bigint] NULL,
	[ServiceId] [bigint] NULL,
	[ServiceKId] [bigint] NULL,
	[QueueNumber] [bigint] NULL,
	[QueueStage] [nvarchar](max) NULL,
	[QueueStatus] [nvarchar](max) NULL,
	[ClassTypeId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_KioskQueues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kiosks]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kiosks](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[NumberType] [nvarchar](max) NULL,
	[BPJS] [nvarchar](max) NULL,
	[StageBpjs] [bit] NULL,
	[PatientId] [bigint] NULL,
	[ServiceId] [bigint] NULL,
	[PhysicianId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Kiosks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabResultDetails]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabResultDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultanMedicalSupportId] [bigint] NOT NULL,
	[Parameter] [nvarchar](max) NULL,
	[NormalRange] [nvarchar](max) NULL,
	[LabUomId] [bigint] NULL,
	[Result] [nvarchar](max) NULL,
	[ResultType] [nvarchar](max) NULL,
	[ResultValueType] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_LabResultDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabTestDetails]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTestDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LabTestId] [bigint] NULL,
	[LabUomId] [bigint] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ResultType] [nvarchar](max) NULL,
	[Parameter] [nvarchar](max) NULL,
	[NormalRangeMale] [nvarchar](max) NULL,
	[NormalRangeFemale] [nvarchar](max) NULL,
	[ResultValueType] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_LabTestDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabTests]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTests](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SampleTypeId] [bigint] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[ResultType] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_LabTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabUoms]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabUoms](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_LabUoms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentLocationId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicamentGroupDetails]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicamentGroupDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MedicamentGroupId] [bigint] NULL,
	[MedicamentId] [bigint] NULL,
	[ActiveComponentId] [nvarchar](max) NULL,
	[SignaId] [bigint] NULL,
	[FrequencyId] [bigint] NULL,
	[UnitOfDosageId] [bigint] NULL,
	[Dosage] [bigint] NULL,
	[QtyByDay] [bigint] NULL,
	[Days] [bigint] NULL,
	[TotalQty] [bigint] NULL,
	[AllowSubtitation] [bit] NULL,
	[MedicaneUnitDosage] [nvarchar](max) NULL,
	[MedicaneDosage] [nvarchar](max) NULL,
	[MedicaneName] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MedicamentGroupDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicamentGroups]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicamentGroups](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[IsConcoction] [bit] NULL,
	[PhycisianId] [bigint] NULL,
	[UoMId] [bigint] NULL,
	[FormDrugId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MedicamentGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medicaments]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicaments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NULL,
	[FrequencyId] [bigint] NULL,
	[RouteId] [bigint] NULL,
	[FormId] [bigint] NULL,
	[UomId] [bigint] NULL,
	[ActiveComponentId] [nvarchar](max) NULL,
	[PregnancyWarning] [bit] NULL,
	[Pharmacologi] [bit] NULL,
	[Weather] [bit] NULL,
	[Food] [bit] NULL,
	[Cronies] [bit] NULL,
	[MontlyMax] [nvarchar](max) NULL,
	[Dosage] [nvarchar](max) NULL,
	[SignaId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Medicaments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Icon] [nvarchar](max) NULL,
	[ParentMenu] [nvarchar](max) NULL,
	[Sequence] [bigint] NULL,
	[Html] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NursingDiagnoses]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NursingDiagnoses](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Problem] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_NursingDiagnoses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Occupationals]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Occupationals](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](300) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Occupationals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientAllergies]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientAllergies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Farmacology] [nvarchar](max) NULL,
	[FarmacologiCode] [nvarchar](max) NOT NULL,
	[Weather] [nvarchar](max) NULL,
	[WeatherCode] [nvarchar](max) NOT NULL,
	[Food] [nvarchar](max) NULL,
	[FoodCode] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientAllergies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientFamilyRelations]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientFamilyRelations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PatientId] [bigint] NOT NULL,
	[FamilyMemberId] [bigint] NOT NULL,
	[FamilyId] [bigint] NULL,
	[Relation] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientFamilyRelations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pharmacies]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pharmacies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PatientId] [bigint] NULL,
	[PractitionerId] [bigint] NULL,
	[PrescriptionLocationId] [bigint] NULL,
	[MedicamentGroupId] [bigint] NULL,
	[ServiceId] [bigint] NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[ReceiptDate] [datetime2](7) NULL,
	[IsWeather] [bit] NOT NULL,
	[IsFarmacologi] [bit] NOT NULL,
	[IsFood] [bit] NOT NULL,
	[Status] [int] NULL,
	[LocationId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Pharmacies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PharmacyLogs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PharmacyLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PharmacyId] [bigint] NULL,
	[UserById] [bigint] NULL,
	[status] [int] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PharmacyLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prescriptions]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prescriptions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PharmacyId] [bigint] NOT NULL,
	[DrugFromId] [bigint] NULL,
	[DrugRouteId] [bigint] NULL,
	[DrugDosageId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[UomId] [bigint] NULL,
	[ActiveComponentId] [nvarchar](max) NULL,
	[DosageFrequency] [nvarchar](max) NULL,
	[Stock] [bigint] NULL,
	[Dosage] [bigint] NULL,
	[GivenAmount] [bigint] NULL,
	[PriceUnit] [bigint] NULL,
	[DrugFormId] [bigint] NULL,
	[SignaId] [bigint] NULL,
	[MedicamentGroupId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Prescriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Procedures]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Procedures](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code_Test] [nvarchar](100) NULL,
	[Classification] [nvarchar](100) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Procedures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategories]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[CostingMethod] [nvarchar](max) NULL,
	[InventoryValuation] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ProductCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[BpjsClassificationId] [bigint] NULL,
	[UomId] [bigint] NULL,
	[ProductCategoryId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[PurchaseUomId] [bigint] NULL,
	[TraceAbility] [bit] NOT NULL,
	[ProductType] [nvarchar](max) NULL,
	[HospitalType] [nvarchar](max) NULL,
	[SalesPrice] [nvarchar](max) NULL,
	[Tax] [nvarchar](max) NULL,
	[Cost] [nvarchar](max) NULL,
	[InternalReference] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Brand] [nvarchar](max) NULL,
	[EquipmentCode] [nvarchar](max) NULL,
	[EquipmentCondition] [nvarchar](max) NULL,
	[LastCalibrationDate] [datetime2](7) NULL,
	[NextCalibrationDate] [datetime2](7) NULL,
	[YearOfPurchase] [datetime2](7) NULL,
	[IsOralMedication] [bit] NOT NULL,
	[IsTopicalMedication] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provinces]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provinces](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Provinces] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QueueDisplays]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QueueDisplays](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[CounterIds] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_QueueDisplays] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReceivingLogs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivingLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceivingId] [bigint] NULL,
	[SourceId] [bigint] NULL,
	[UserById] [bigint] NULL,
	[Status] [int] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ReceivingLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReceivingStockDetails]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivingStockDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceivingStockId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[Qty] [bigint] NULL,
	[Batch] [nvarchar](max) NULL,
	[ExpiredDate] [datetime2](7) NULL,
	[StockId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ReceivingStockDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReceivingStocks]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivingStocks](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DestinationId] [bigint] NULL,
	[SchenduleDate] [datetime2](7) NULL,
	[KodeReceiving] [nvarchar](max) NULL,
	[NumberPurchase] [nvarchar](max) NULL,
	[Reference] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ReceivingStocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Religions]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Religions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Religions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReorderingRules]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReorderingRules](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[ProductId] [nvarchar](max) NOT NULL,
	[MinimumQuantity] [real] NOT NULL,
	[MaximumQuantity] [real] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ReorderingRules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SampleTypes]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SampleTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_SampleTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](5) NOT NULL,
	[Quota] [nvarchar](max) NOT NULL,
	[IsPatient] [bit] NOT NULL,
	[IsKiosk] [bit] NOT NULL,
	[IsMcu] [bit] NOT NULL,
	[ServicedId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SickLeaves]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SickLeaves](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GeneralConsultansId] [bigint] NULL,
	[TypeLeave] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_SickLeaves] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Signas]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Signas](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Signas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specialities]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specialities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Code] [nvarchar](5) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Specialities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockOutLines]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockOutLines](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LinesId] [bigint] NULL,
	[TransactionStockId] [bigint] NULL,
	[CutStock] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_StockOutLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockOutPrescriptions]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockOutPrescriptions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PrescriptionId] [bigint] NULL,
	[TransactionStockId] [bigint] NULL,
	[CutStock] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[StockProductId] [bigint] NULL,
 CONSTRAINT [PK_StockOutPrescriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockProducts]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockProducts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NULL,
	[Qty] [bigint] NULL,
	[SourceId] [bigint] NULL,
	[DestinanceId] [bigint] NULL,
	[UomId] [bigint] NULL,
	[StatusTransaction] [nvarchar](max) NULL,
	[Batch] [nvarchar](max) NULL,
	[Referency] [nvarchar](max) NULL,
	[SerialNumber] [nvarchar](max) NULL,
	[Expired] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_StockProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemParameters]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemParameters](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_SystemParameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionStocks]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionStocks](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SourceTable] [nvarchar](max) NULL,
	[SourcTableId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[Reference] [nvarchar](max) NULL,
	[Batch] [nvarchar](max) NULL,
	[ExpiredDate] [datetime2](7) NULL,
	[LocationId] [bigint] NULL,
	[Quantity] [bigint] NULL,
	[UomId] [bigint] NULL,
	[Validate] [bit] NULL,
	[InventoryAdjusmentId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_TransactionStocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransferStockLogs]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferStockLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TransferStockId] [bigint] NULL,
	[SourceId] [bigint] NULL,
	[DestinationId] [bigint] NULL,
	[Status] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_TransferStockLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransferStockProduct]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferStockProduct](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Batch] [nvarchar](max) NULL,
	[TransferStockId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[QtyStock] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ExpiredDate] [datetime2](7) NULL,
 CONSTRAINT [PK_TransferStockProduct] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransferStocks]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferStocks](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SourceId] [bigint] NULL,
	[DestinationId] [bigint] NULL,
	[SchenduleDate] [datetime2](7) NULL,
	[KodeTransaksi] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[Reference] [nvarchar](max) NULL,
	[StockRequest] [bit] NULL,
	[StockProductId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_TransferStocks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UomCategories]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UomCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_UomCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Uoms]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Uoms](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[UomCategoryId] [bigint] NULL,
	[Type] [nvarchar](max) NULL,
	[BiggerRatio] [real] NULL,
	[Active] [bit] NOT NULL,
	[RoundingPrecision] [real] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Uoms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupId] [bigint] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[GenderId] [bigint] NULL,
	[MartialStatus] [nvarchar](max) NULL,
	[PlaceOfBirth] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[TypeId] [nvarchar](max) NULL,
	[NoId] [nvarchar](max) NULL,
	[ExpiredId] [datetime2](7) NULL,
	[IdCardAddress1] [nvarchar](max) NULL,
	[IdCardAddress2] [nvarchar](max) NULL,
	[IdCardCountryId] [bigint] NULL,
	[IdCardProvinceId] [bigint] NULL,
	[IdCardCityId] [bigint] NULL,
	[IdCardDistrictId] [bigint] NULL,
	[IdCardVillageId] [bigint] NULL,
	[IdCardRtRw] [nvarchar](max) NULL,
	[IdCardZip] [bigint] NULL,
	[DomicileAddress1] [nvarchar](max) NULL,
	[DomicileAddress2] [nvarchar](max) NULL,
	[DomicileCountryId] [bigint] NULL,
	[DomicileProvinceId] [bigint] NULL,
	[DomicileCityId] [bigint] NULL,
	[DomicileDistrictId] [bigint] NULL,
	[DomicileVillageId] [bigint] NULL,
	[DomicileRtRw] [nvarchar](max) NULL,
	[DomicileZip] [bigint] NULL,
	[BiologicalMother] [nvarchar](max) NULL,
	[MotherNIK] [nvarchar](max) NULL,
	[ReligionId] [bigint] NULL,
	[MobilePhone] [nvarchar](max) NULL,
	[HomePhoneNumber] [nvarchar](max) NULL,
	[Npwp] [nvarchar](max) NULL,
	[NoBpjsKs] [nvarchar](max) NULL,
	[NoBpjsTk] [nvarchar](max) NULL,
	[SipNo] [nvarchar](max) NULL,
	[SipFile] [nvarchar](max) NULL,
	[SipExp] [datetime2](7) NULL,
	[StrNo] [nvarchar](max) NULL,
	[StrFile] [nvarchar](max) NULL,
	[StrExp] [datetime2](7) NULL,
	[SpecialityId] [bigint] NULL,
	[UserPhoto] [nvarchar](max) NULL,
	[JobPositionId] [bigint] NULL,
	[DepartmentId] [bigint] NULL,
	[EmergencyName] [nvarchar](max) NULL,
	[EmergencyRelation] [nvarchar](max) NULL,
	[EmergencyEmail] [nvarchar](max) NULL,
	[EmergencyPhone] [nvarchar](max) NULL,
	[BloodType] [nvarchar](max) NULL,
	[NoRm] [nvarchar](max) NULL,
	[DoctorCode] [nvarchar](max) NULL,
	[EmployeeCode] [nvarchar](max) NULL,
	[DegreeId] [bigint] NULL,
	[IsEmployee] [bit] NOT NULL,
	[IsPatient] [bit] NOT NULL,
	[IsUser] [bit] NOT NULL,
	[IsDoctor] [bit] NOT NULL,
	[IsPhysicion] [bit] NOT NULL,
	[IsNurse] [bit] NOT NULL,
	[IsPharmacy] [bit] NOT NULL,
	[IsMcu] [bit] NOT NULL,
	[IsHr] [bit] NOT NULL,
	[PhysicanCode] [nvarchar](max) NOT NULL,
	[IsEmployeeRelation] [bit] NULL,
	[EmployeeType] [nvarchar](max) NULL,
	[EmployeeStatus] [nvarchar](max) NULL,
	[JoinDate] [datetime2](7) NULL,
	[NIP] [nvarchar](450) NULL,
	[Legacy] [nvarchar](450) NULL,
	[SAP] [nvarchar](450) NULL,
	[Oracle] [nvarchar](450) NULL,
	[DoctorServiceIds] [nvarchar](max) NULL,
	[PatientAllergyIds] [nvarchar](max) NULL,
	[SupervisorId] [bigint] NULL,
	[EmailTemplateId] [bigint] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[OccupationalId] [bigint] NULL,
	[CurrentMobile] [nvarchar](max) NULL,
	[FamilyMedicalHistory] [nvarchar](max) NULL,
	[FamilyMedicalHistoryOther] [nvarchar](max) NULL,
	[IsFamilyMedicalHistory] [nvarchar](max) NULL,
	[IsMedicationHistory] [nvarchar](max) NULL,
	[MedicationHistory] [nvarchar](max) NULL,
	[PastMedicalHistory] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Villages]    Script Date: 13/08/2024 10:57:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Villages](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProvinceId] [bigint] NOT NULL,
	[CityId] [bigint] NOT NULL,
	[DistrictId] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[PostalCode] [nvarchar](10) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Villages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ActiveComponents] ON 

INSERT [dbo].[ActiveComponents] ([Id], [UomId], [Name], [AmountOfComponent], [ConcoctionLineId], [MedicamentId], [PrescriptionId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, N'1 x 3', NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-07-18T10:35:09.2198098' AS DateTime2), NULL, NULL)
INSERT [dbo].[ActiveComponents] ([Id], [UomId], [Name], [AmountOfComponent], [ConcoctionLineId], [MedicamentId], [PrescriptionId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, NULL, N'Bodrex', NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-04-24T15:23:41.6307572' AS DateTime2), N'Administrator', CAST(N'2024-04-25T13:28:31.7168588' AS DateTime2))
INSERT [dbo].[ActiveComponents] ([Id], [UomId], [Name], [AmountOfComponent], [ConcoctionLineId], [MedicamentId], [PrescriptionId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, NULL, N'Panadol', NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-04-25T13:35:11.2610491' AS DateTime2), NULL, NULL)
INSERT [dbo].[ActiveComponents] ([Id], [UomId], [Name], [AmountOfComponent], [ConcoctionLineId], [MedicamentId], [PrescriptionId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, NULL, N'Puyer  Bintang 7', NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-04-25T13:35:42.6673379' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[ActiveComponents] OFF
GO
SET IDENTITY_INSERT [dbo].[Allergies] ON 

INSERT [dbo].[Allergies] ([Id], [KdAllergy], [NmAllergy], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'01', N'Seafood', N'01', N'Administrator', CAST(N'2024-05-27T15:56:27.7231515' AS DateTime2), NULL, NULL)
INSERT [dbo].[Allergies] ([Id], [KdAllergy], [NmAllergy], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'02', N'Gandum', N'01', N'Administrator', CAST(N'2024-05-27T15:57:44.6578123' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Allergies] OFF
GO
SET IDENTITY_INSERT [dbo].[Awarenesses] ON 

INSERT [dbo].[Awarenesses] ([Id], [KdSadar], [NmSadar], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'01', N'Compos mentis', N'Nurse', CAST(N'2024-05-13T10:33:51.3039284' AS DateTime2), NULL, NULL)
INSERT [dbo].[Awarenesses] ([Id], [KdSadar], [NmSadar], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'02', N'Somnolence', N'Nurse', CAST(N'2024-05-13T10:33:51.3039333' AS DateTime2), NULL, NULL)
INSERT [dbo].[Awarenesses] ([Id], [KdSadar], [NmSadar], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'03', N'Sopor', N'Nurse', CAST(N'2024-05-13T10:33:51.3039337' AS DateTime2), NULL, NULL)
INSERT [dbo].[Awarenesses] ([Id], [KdSadar], [NmSadar], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'04', N'Coma', N'Nurse', CAST(N'2024-05-13T10:33:51.3039341' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Awarenesses] OFF
GO
SET IDENTITY_INSERT [dbo].[BpjsClassifications] ON 

INSERT [dbo].[BpjsClassifications] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Alkes', NULL, N'Administrator', CAST(N'2024-04-23T09:46:03.6010146' AS DateTime2), NULL, NULL)
INSERT [dbo].[BpjsClassifications] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Laboratorium', NULL, N'Administrator', CAST(N'2024-04-23T09:46:12.9079284' AS DateTime2), NULL, NULL)
INSERT [dbo].[BpjsClassifications] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Konsultasi', NULL, N'Administrator', CAST(N'2024-04-23T09:46:35.9274356' AS DateTime2), NULL, NULL)
INSERT [dbo].[BpjsClassifications] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'Obat Kronis', NULL, N'Administrator', CAST(N'2024-04-23T09:46:59.9224934' AS DateTime2), NULL, NULL)
INSERT [dbo].[BpjsClassifications] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'Keperawatan', NULL, N'Administrator', CAST(N'2024-04-23T09:48:03.0485559' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[BpjsClassifications] OFF
GO
SET IDENTITY_INSERT [dbo].[Buildings] ON 

INSERT [dbo].[Buildings] ([Id], [HealthCenterId], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, 1, N'General Clinic', N'', N'Administrator', CAST(N'2024-04-03T10:36:48.3374042' AS DateTime2), N'Administrator', CAST(N'2024-04-05T13:52:03.1451395' AS DateTime2))
INSERT [dbo].[Buildings] ([Id], [HealthCenterId], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, 1, N'Villa Clinic', N'', N'Administrator', CAST(N'2024-04-03T10:39:08.9970475' AS DateTime2), N'Administrator', CAST(N'2024-04-03T10:42:32.3788170' AS DateTime2))
INSERT [dbo].[Buildings] ([Id], [HealthCenterId], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, 1, N'Mobile Clinic', N'', N'Administrator', CAST(N'2024-04-03T10:43:07.2230772' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Buildings] OFF
GO
SET IDENTITY_INSERT [dbo].[ClassTypes] ON 

INSERT [dbo].[ClassTypes] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'VIP', N'Administrator', CAST(N'2024-03-08T16:56:59.1461434' AS DateTime2), N'', CAST(N'2024-03-26T10:45:23.7660879' AS DateTime2))
SET IDENTITY_INSERT [dbo].[ClassTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Companies] ON 

INSERT [dbo].[Companies] ([Id], [CityId], [ProvinceId], [CountryId], [Name], [Phone], [Email], [Website], [VAT], [Street1], [Street2], [Zip], [CurrencyId], [Logo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, NULL, NULL, NULL, N'McDermott', NULL, N'info@McDermott.com', N'McDermott.com', N'', N'', N'', N'', NULL, N'', N'Administrator', CAST(N'2024-08-07T00:35:01.9201626' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Companies] OFF
GO
SET IDENTITY_INSERT [dbo].[CronisCategories] ON 

INSERT [dbo].[CronisCategories] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'A00-A09: Penyakit menular bakterial', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[CronisCategories] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'A15-A19: Penyakit menular protozoa', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[CronisCategories] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'E00-E35: Penyakit endokrin', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[CronisCategories] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'E40-E68: Penyakit gizi', NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[CronisCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Departments] ON 

INSERT [dbo].[Departments] ([Id], [CompanyId], [ParentDepartmentId], [ManagerId], [Name], [DepartmentCategory], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 2, NULL, 74, N'Technology', N'Department', N'Administrator', CAST(N'2024-08-07T00:35:28.1865820' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Departments] OFF
GO
SET IDENTITY_INSERT [dbo].[Diagnoses] ON 

INSERT [dbo].[Diagnoses] ([Id], [Name], [Code], [DiseaseCategoryId], [CronisCategoryId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Cholera due to Vibrio cholerae 01, biovar cholerae', N'A00.0', 1, 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Diagnoses] ([Id], [Name], [Code], [DiseaseCategoryId], [CronisCategoryId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Cholera due to Vibrio cholerae 01, biovar el tor', N'A00.1', 2, 2, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Diagnoses] OFF
GO
SET IDENTITY_INSERT [dbo].[DiseaseCategories] ON 

INSERT [dbo].[DiseaseCategories] ([Id], [Name], [ParentCategory], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'(A30-A49) Other bacterial diseases', N'(A30-A49) Other bacterial diseases', NULL, NULL, NULL, NULL)
INSERT [dbo].[DiseaseCategories] ([Id], [Name], [ParentCategory], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'(A50-A64) Infections with a predominantly sexual mode of transmission', N'(A50-A64) Infections with a predominantly sexual mode of transmission', NULL, NULL, NULL, NULL)
INSERT [dbo].[DiseaseCategories] ([Id], [Name], [ParentCategory], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'(A20-A28) Certain zoonotic bacterial diseases', N'(A20-A28) Certain zoonotic bacterial diseases', NULL, CAST(N'2024-04-22T14:03:04.1929199' AS DateTime2), NULL, CAST(N'2024-04-22T14:03:15.9606251' AS DateTime2))
INSERT [dbo].[DiseaseCategories] ([Id], [Name], [ParentCategory], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'(A00-A09) Intestinal infectious diseases', N'(A00-A09) Intestinal infectious diseases', NULL, CAST(N'2024-04-22T14:04:08.1879071' AS DateTime2), NULL, CAST(N'2024-04-22T14:08:58.0879373' AS DateTime2))
INSERT [dbo].[DiseaseCategories] ([Id], [Name], [ParentCategory], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'I Certain infectious and parasitic diseases', NULL, NULL, CAST(N'2024-04-22T14:08:41.1661483' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[DiseaseCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[DoctorScheduleDetails] ON 

INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, 1, N'Monday Morning', N'Monday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969523' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, 1, N'Monday Afternoon', N'Monday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969551' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, 1, N'Tuesday Morning', N'Tuesday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969565' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, 1, N'Tuesday Afternoon', N'Tuesday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969578' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, 1, N'Wednesday morning', N'Wednesday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969607' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, 1, N'Wednesday Afternoon', N'Wednesday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969621' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, 1, N'Thursday morning', N'Thursday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969627' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, 1, N'Thursday Afternoon', N'Thursday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969630' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, 1, N'Friday morning', N'Friday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969635' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, 1, N'Friday Afternoon', N'Friday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969639' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, 1, N'Saturday morning', N'Saturday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969643' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, 1, N'Saturday Afternoon', N'Saturday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969647' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, 1, N'Sunday Moorning', N'Sunday', CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969650' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleDetails] ([Id], [DoctorScheduleId], [Name], [DayOfWeek], [WorkFrom], [WorkTo], [Quota], [UpdateToBpjs], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, 1, N'Sunday Afternoon', N'Sunday', CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 0, N'Administrator', CAST(N'2024-05-27T17:26:32.3969654' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[DoctorScheduleDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[DoctorSchedules] ON 

INSERT [dbo].[DoctorSchedules] ([Id], [Name], [ServiceId], [PhysicionIds], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Jadwal Dokter Umum', 16, N'[48]', N'Administrator', CAST(N'2024-05-27T17:23:50.9460081' AS DateTime2), N'Administrator', CAST(N'2024-05-27T17:26:32.3644608' AS DateTime2))
SET IDENTITY_INSERT [dbo].[DoctorSchedules] OFF
GO
SET IDENTITY_INSERT [dbo].[DoctorScheduleSlots] ON 

INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, 48, CAST(N'2024-05-01T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880806' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, 1, 48, CAST(N'2024-05-01T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880839' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, 1, 48, CAST(N'2024-05-02T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880842' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, 1, 48, CAST(N'2024-05-02T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880845' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, 1, 48, CAST(N'2024-05-03T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880849' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, 1, 48, CAST(N'2024-05-03T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880852' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, 1, 48, CAST(N'2024-05-04T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880856' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, 1, 48, CAST(N'2024-05-04T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880859' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, 1, 48, CAST(N'2024-05-05T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880862' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, 1, 48, CAST(N'2024-05-05T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880868' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, 1, 48, CAST(N'2024-05-06T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880875' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, 1, 48, CAST(N'2024-05-06T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880877' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, 1, 48, CAST(N'2024-05-07T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880881' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, 1, 48, CAST(N'2024-05-07T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880886' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, 1, 48, CAST(N'2024-05-08T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880893' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, 1, 48, CAST(N'2024-05-08T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880896' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, 1, 48, CAST(N'2024-05-09T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880899' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, 1, 48, CAST(N'2024-05-09T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880906' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, 1, 48, CAST(N'2024-05-10T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880912' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, 1, 48, CAST(N'2024-05-10T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880915' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (21, 1, 48, CAST(N'2024-05-11T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880921' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (22, 1, 48, CAST(N'2024-05-11T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880927' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (23, 1, 48, CAST(N'2024-05-12T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880930' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (24, 1, 48, CAST(N'2024-05-12T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880933' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (25, 1, 48, CAST(N'2024-05-13T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880936' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (26, 1, 48, CAST(N'2024-05-13T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880939' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (27, 1, 48, CAST(N'2024-05-14T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880943' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (28, 1, 48, CAST(N'2024-05-14T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880946' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (29, 1, 48, CAST(N'2024-05-15T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880949' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (30, 1, 48, CAST(N'2024-05-15T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880965' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (31, 1, 48, CAST(N'2024-05-16T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880978' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (32, 1, 48, CAST(N'2024-05-16T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880982' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (33, 1, 48, CAST(N'2024-05-17T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880985' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (34, 1, 48, CAST(N'2024-05-17T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880988' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (35, 1, 48, CAST(N'2024-05-18T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880991' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (36, 1, 48, CAST(N'2024-05-18T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880995' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (37, 1, 48, CAST(N'2024-05-19T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5880998' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (38, 1, 48, CAST(N'2024-05-19T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881001' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (39, 1, 48, CAST(N'2024-05-20T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881005' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (40, 1, 48, CAST(N'2024-05-20T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881008' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (41, 1, 48, CAST(N'2024-05-21T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881011' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (42, 1, 48, CAST(N'2024-05-21T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881014' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (43, 1, 48, CAST(N'2024-05-22T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881018' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (44, 1, 48, CAST(N'2024-05-22T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881021' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (45, 1, 48, CAST(N'2024-05-23T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881024' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (46, 1, 48, CAST(N'2024-05-23T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881028' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (47, 1, 48, CAST(N'2024-05-24T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881031' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (48, 1, 48, CAST(N'2024-05-24T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881034' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (49, 1, 48, CAST(N'2024-05-25T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881037' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (50, 1, 48, CAST(N'2024-05-25T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881040' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (51, 1, 48, CAST(N'2024-05-26T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881044' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (52, 1, 48, CAST(N'2024-05-26T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881047' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (53, 1, 48, CAST(N'2024-05-27T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881050' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (54, 1, 48, CAST(N'2024-05-27T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881056' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (55, 1, 48, CAST(N'2024-05-28T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881061' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (56, 1, 48, CAST(N'2024-05-28T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881064' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (57, 1, 48, CAST(N'2024-05-29T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881067' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (58, 1, 48, CAST(N'2024-05-29T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881070' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (59, 1, 48, CAST(N'2024-05-30T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881074' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (60, 1, 48, CAST(N'2024-05-30T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881077' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (61, 1, 48, CAST(N'2024-05-31T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881080' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (62, 1, 48, CAST(N'2024-05-31T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:26:51.5881083' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (63, 1, 48, CAST(N'2024-06-01T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638527' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (64, 1, 48, CAST(N'2024-06-01T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638548' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (65, 1, 48, CAST(N'2024-06-02T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638550' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (66, 1, 48, CAST(N'2024-06-02T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638552' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (67, 1, 48, CAST(N'2024-06-03T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638553' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (68, 1, 48, CAST(N'2024-06-03T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638555' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (69, 1, 48, CAST(N'2024-06-04T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638557' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (70, 1, 48, CAST(N'2024-06-04T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638558' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (71, 1, 48, CAST(N'2024-06-05T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638559' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (72, 1, 48, CAST(N'2024-06-05T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638561' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (73, 1, 48, CAST(N'2024-06-06T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638563' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (74, 1, 48, CAST(N'2024-06-06T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638564' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (75, 1, 48, CAST(N'2024-06-07T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638566' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (76, 1, 48, CAST(N'2024-06-07T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638568' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (77, 1, 48, CAST(N'2024-06-08T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638570' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (78, 1, 48, CAST(N'2024-06-08T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638571' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (79, 1, 48, CAST(N'2024-06-09T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638573' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (80, 1, 48, CAST(N'2024-06-09T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638574' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (81, 1, 48, CAST(N'2024-06-10T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638576' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (82, 1, 48, CAST(N'2024-06-10T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638577' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (83, 1, 48, CAST(N'2024-06-11T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638578' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (84, 1, 48, CAST(N'2024-06-11T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638580' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (85, 1, 48, CAST(N'2024-06-12T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638581' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (86, 1, 48, CAST(N'2024-06-12T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638583' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (87, 1, 48, CAST(N'2024-06-13T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638584' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (88, 1, 48, CAST(N'2024-06-13T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638586' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (89, 1, 48, CAST(N'2024-06-14T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638587' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (90, 1, 48, CAST(N'2024-06-14T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638589' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (91, 1, 48, CAST(N'2024-06-15T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638590' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (92, 1, 48, CAST(N'2024-06-15T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638591' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (93, 1, 48, CAST(N'2024-06-16T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638593' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (94, 1, 48, CAST(N'2024-06-16T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638594' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (95, 1, 48, CAST(N'2024-06-17T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638596' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (96, 1, 48, CAST(N'2024-06-17T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638597' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (97, 1, 48, CAST(N'2024-06-18T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638599' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (98, 1, 48, CAST(N'2024-06-18T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638600' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (99, 1, 48, CAST(N'2024-06-19T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638602' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (100, 1, 48, CAST(N'2024-06-19T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638603' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (101, 1, 48, CAST(N'2024-06-20T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638604' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (102, 1, 48, CAST(N'2024-06-20T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638606' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (103, 1, 48, CAST(N'2024-06-21T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638607' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (104, 1, 48, CAST(N'2024-06-21T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638609' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (105, 1, 48, CAST(N'2024-06-22T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638610' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (106, 1, 48, CAST(N'2024-06-22T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638612' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (107, 1, 48, CAST(N'2024-06-23T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638613' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (108, 1, 48, CAST(N'2024-06-23T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638614' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (109, 1, 48, CAST(N'2024-06-24T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638624' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (110, 1, 48, CAST(N'2024-06-24T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638627' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (111, 1, 48, CAST(N'2024-06-25T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638628' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (112, 1, 48, CAST(N'2024-06-25T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638630' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (113, 1, 48, CAST(N'2024-06-26T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638631' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (114, 1, 48, CAST(N'2024-06-26T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638632' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (115, 1, 48, CAST(N'2024-06-27T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638634' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (116, 1, 48, CAST(N'2024-06-27T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638635' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (117, 1, 48, CAST(N'2024-06-28T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638637' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (118, 1, 48, CAST(N'2024-06-28T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638638' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (119, 1, 48, CAST(N'2024-06-29T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638640' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (120, 1, 48, CAST(N'2024-06-29T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638641' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (121, 1, 48, CAST(N'2024-06-30T00:00:00.0000000' AS DateTime2), CAST(N'08:00:00' AS Time), CAST(N'12:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638643' AS DateTime2), NULL, NULL)
INSERT [dbo].[DoctorScheduleSlots] ([Id], [DoctorScheduleId], [PhysicianId], [StartDate], [WorkFrom], [WorkTo], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (122, 1, 48, CAST(N'2024-06-30T00:00:00.0000000' AS DateTime2), CAST(N'12:00:00' AS Time), CAST(N'18:00:00' AS Time), N'Administrator', CAST(N'2024-05-27T17:27:06.7638644' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[DoctorScheduleSlots] OFF
GO
SET IDENTITY_INSERT [dbo].[DrugDosages] ON 

INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, N'1', 2, 1, N'Administrator', CAST(N'2024-07-18T10:35:25.7311393' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, NULL, N'Sebelum Makan', 2, 1, N'Administrator', CAST(N'2024-04-03T14:42:07.4006670' AS DateTime2), N'Administrator', CAST(N'2024-04-26T13:41:49.7519890' AS DateTime2))
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, NULL, N'Sesudah Makan', 0, 0, N'Administrator', CAST(N'2024-04-03T14:42:21.8968845' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, NULL, N'Tiap Diare', 0, 0, N'Administrator', CAST(N'2024-04-03T14:43:00.9366014' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, NULL, N'Sesudah makan Pagi', 0, 0, N'Administrator', CAST(N'2024-04-03T14:43:25.2899526' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, NULL, N'Sesudah Makan Siang', 0, 0, N'Administrator', CAST(N'2024-04-03T14:43:37.3003283' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, NULL, N'Sesudah Makan Malam', 0, 0, N'Administrator', CAST(N'2024-04-03T14:43:46.4626782' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, 1, N'3 X sehari sesudah makan', 3, 1, N'Administrator', CAST(N'2024-04-18T10:04:10.8899422' AS DateTime2), N'Administrator', CAST(N'2024-04-26T13:41:24.5290609' AS DateTime2))
INSERT [dbo].[DrugDosages] ([Id], [DrugRouteId], [Frequency], [TotalQtyPerDay], [Days], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, NULL, N'Relasi ke MD', 0, 0, N'Administrator', CAST(N'2024-04-29T17:55:00.1958349' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[DrugDosages] OFF
GO
SET IDENTITY_INSERT [dbo].[DrugRoutes] ON 

INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Obat', NULL, N'Administrator', CAST(N'2024-07-18T10:34:37.7604332' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Dental', N'DT', NULL, CAST(N'2024-04-01T07:14:49.5737014' AS DateTime2), N'Administrator', CAST(N'2024-04-01T14:14:49.6509797' AS DateTime2))
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Apply Externally', N'AP', N'Administrator', CAST(N'2024-04-19T14:27:44.9787148' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Dental', N'DT', N'Administrator', CAST(N'2024-04-19T14:27:58.3951560' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'Epidural', N'EP', N'Administrator', CAST(N'2024-04-19T14:28:08.5928739' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'Endotrachial Tube', N'ET', N'Administrator', CAST(N'2024-04-19T14:28:21.3284890' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'Gastrostomy Tube', N'GTT', N'Administrator', CAST(N'2024-04-19T14:28:30.7099733' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'GU Irrigant', N'GU', N'Administrator', CAST(N'2024-04-19T14:28:43.4590122' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Immerse (Soak) Body Part', N'IMR', N'Administrator', CAST(N'2024-04-19T14:28:58.3362458' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'Intra-arterial', N'IA', N'Administrator', CAST(N'2024-04-19T14:29:14.3679717' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'Intrabursal', N'IB', N'Administrator', CAST(N'2024-04-19T14:29:25.5531458' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'Intracardiac', N'IC', N'Administrator', CAST(N'2024-04-19T14:29:39.0793379' AS DateTime2), NULL, NULL)
INSERT [dbo].[DrugRoutes] ([Id], [Route], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'Intracervical (uterus)', N'ICV', N'Administrator', CAST(N'2024-04-19T14:29:49.6575909' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[DrugRoutes] OFF
GO
SET IDENTITY_INSERT [dbo].[EmailSettings] ON 

INSERT [dbo].[EmailSettings] ([Id], [Description], [Sequence], [Smpt_Debug], [Smtp_Encryption], [Smtp_Host], [Smtp_Pass], [Status], [Smtp_Port], [Smtp_User], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'', NULL, NULL, N'SSL/TLS', N'srv42.niagahoster.com', N'nuralimajid', N'Connected!', N'465', N'nuralimajid@matrica.co.id', NULL, CAST(N'2024-08-06T16:57:03.2778769' AS DateTime2), N'Administrator', CAST(N'2024-08-06T23:57:04.8727474' AS DateTime2))
SET IDENTITY_INSERT [dbo].[EmailSettings] OFF
GO
SET IDENTITY_INSERT [dbo].[EmailTemplates] ON 

INSERT [dbo].[EmailTemplates] ([Id], [Subject], [From], [ById], [To], [ToPartnerId], [Cc], [ReplayTo], [Schendule], [Message], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [DocumentContent], [EmailFromId], [TypeEmail]) VALUES (5, N' Surat Keterangan Sakit - %NamePatient%', N'', NULL, N'', NULL, N'["fathulazharis@gmail.com","nuralimajids@gmail.com"]', N'', NULL, N'<p><strong>Yth. Bapak/Ibu HRD,</strong></p><p>Selamat pagi/siang,</p><p>Dengan hormat,</p><p>Bersama ini kami sampaikan surat keterangan sakit untuk karyawan atas nama:</p><ul><li><strong>Nama Lengkap: %NamePatient%</strong></li><li><strong>Jabatan: [Jabatan Karyawan]</strong></li><li><strong>Nomor Induk Karyawan (NIK): [NIK Karyawan]</strong></li></ul><p><strong>Yang bersangkutan telah menjalani pemeriksaan medis di [Nama Rumah Sakit/Klinik] pada tanggal [Tanggal Pemeriksaan] dan disarankan untuk beristirahat selama [Jumlah Hari] hari, terhitung mulai tanggal [Tanggal Mulai Istirahat] hingga [Tanggal Selesai Istirahat].</strong></p><p><strong>Surat keterangan sakit ini dapat dijadikan sebagai bahan pertimbangan Bapak/Ibu. Terlampir surat keterangan sakit resmi dari dokter.</strong></p><p><strong>Atas perhatian dan kerjasamanya, kami ucapkan terima kasih.</strong></p><p><strong>Hormat kami,</strong></p><p><strong>%NameDoctor%</strong></p><p><strong>Dokter</strong></p><p><strong>McDermott</strong></p>', N'draf', N'Administrator', CAST(N'2024-08-06T16:22:04.5824910' AS DateTime2), N'Administrator', CAST(N'2024-08-06T23:57:04.8723518' AS DateTime2), 0x, 1, 1)
INSERT [dbo].[EmailTemplates] ([Id], [Subject], [From], [ById], [To], [ToPartnerId], [Cc], [ReplayTo], [Schendule], [Message], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [DocumentContent], [EmailFromId], [TypeEmail]) VALUES (6, N' Surat Keterangan Sakit - %NamePatient%', N'', NULL, N'', NULL, N'["fathulazharis@gmail.com"]', N'', NULL, N'<p> <strong>Yth. Bapak/Ibu HRD,</strong></p><p>Selamat pagi/siang,</p><p>Dengan hormat,</p><p>Bersama ini kami sampaikan surat keterangan sakit untuk karyawan atas nama:</p><ul><li><strong>Nama Lengkap: %</strong>NamePatient%</li><li><strong>Jabatan:</strong> [Jabatan Karyawan]</li><li><strong>Nomor Induk Karyawan (NIK):</strong> [NIK Karyawan]</li></ul><p>Yang bersangkutan telah menjalani pemeriksaan medis pada tanggal %Dates% dan disarankan untuk beristirahat selama %KataNumber% hari, terhitung mulai tanggal %startDate% hingga %endDate%.</p><p>Surat keterangan sakit ini dapat dijadikan sebagai bahan pertimbangan Bapak/Ibu. Terlampir surat keterangan sakit resmi dari dokter.</p><p>Atas perhatian dan kerjasamanya, kami ucapkan terima kasih.</p><p>Hormat kami,</p><p>%NameDoctor%</p><p>Dokter</p><p>McDermott</p>', N'draf', N'Administrator', CAST(N'2024-08-07T00:01:41.3133410' AS DateTime2), NULL, NULL, 0x, 1, 1)
SET IDENTITY_INSERT [dbo].[EmailTemplates] OFF
GO
SET IDENTITY_INSERT [dbo].[Families] ON 

INSERT [dbo].[Families] ([Id], [Name], [ParentRelation], [ChildRelation], [Relation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'Parent', N'Parent', N'Child', N'Child-Parent', N'Administrator', CAST(N'2024-05-16T13:35:39.9051263' AS DateTime2), N'Administrator', CAST(N'2024-05-16T13:35:47.8666324' AS DateTime2))
INSERT [dbo].[Families] ([Id], [Name], [ParentRelation], [ChildRelation], [Relation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Child', N'Child', N'Parent', N'Parent-Child', N'Administrator', CAST(N'2024-05-16T13:35:47.8472607' AS DateTime2), NULL, NULL)
INSERT [dbo].[Families] ([Id], [Name], [ParentRelation], [ChildRelation], [Relation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'Husband', N'Husband', N'Wife', N'Wife-Husband', N'Administrator', CAST(N'2024-05-16T13:36:13.2550694' AS DateTime2), N'Administrator', CAST(N'2024-05-16T13:36:20.0997727' AS DateTime2))
INSERT [dbo].[Families] ([Id], [Name], [ParentRelation], [ChildRelation], [Relation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'Wife', N'Wife', N'Husband', N'Husband-Wife', N'Administrator', CAST(N'2024-05-16T13:36:20.0831669' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Families] OFF
GO
SET IDENTITY_INSERT [dbo].[FormDrugs] ON 

INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'GAS', N'Gas', N'Administrator', CAST(N'2024-04-19T14:08:58.2110996' AS DateTime2), N'Administrator', CAST(N'2024-04-19T14:09:56.5994523' AS DateTime2))
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'GEL', N'Gel', N'Administrator', CAST(N'2024-04-19T14:09:08.0533119' AS DateTime2), N'Administrator', CAST(N'2024-04-19T14:10:06.8442043' AS DateTime2))
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'GER', N'Gel controlled release', N'Administrator', CAST(N'2024-04-19T14:09:30.9831021' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'PSSR', N'Powder for suspension sustained', N'Administrator', CAST(N'2024-04-19T14:10:23.2044621' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'WIP', N'Wipe', N'Administrator', CAST(N'2024-04-19T14:10:38.5659494' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'ST', N'Sendok Teh', N'Administrator', CAST(N'2024-04-19T14:10:55.7327640' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'SM', N'Sendok Makan', N'Administrator', CAST(N'2024-04-19T14:11:07.1185479' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'AEM', N'Aerosol metered-dose', N'Administrator', CAST(N'2024-04-19T14:11:25.4350072' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'AER', N'Aerosol with propellants', N'Administrator', CAST(N'2024-04-19T14:11:37.1778007' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'BAR', N'Bar chewable', N'Administrator', CAST(N'2024-04-19T14:11:55.5663472' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'TAB', N'Tablets', N'Administrator', CAST(N'2024-08-07T00:50:34.2890851' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'CAPS', N'Capsules', N'Administrator', CAST(N'2024-08-07T00:50:50.4956257' AS DateTime2), NULL, NULL)
INSERT [dbo].[FormDrugs] ([Id], [Code], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'SYR', N'Syrups', N'Administrator', CAST(N'2024-08-07T00:51:16.7704334' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[FormDrugs] OFF
GO
SET IDENTITY_INSERT [dbo].[Genders] ON 

INSERT [dbo].[Genders] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Male', NULL, NULL, NULL, NULL)
INSERT [dbo].[Genders] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Female', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Genders] OFF
GO
SET IDENTITY_INSERT [dbo].[GeneralConsultanServices] ON 

INSERT [dbo].[GeneralConsultanServices] ([Id], [KioskQueueId], [PatientId], [InsurancePolicyId], [ServiceId], [PratitionerId], [ClassTypeId], [Method], [AdmissionQueue], [Payment], [TypeRegistration], [HomeStatus], [TypeMedical], [ScheduleTime], [IsAlertInformationSpecialCase], [IsSickLeave], [IsMaternityLeave], [StartDateSickLeave], [EndDateSickLeave], [StartMaternityLeave], [EndMaternityLeave], [RegistrationDate], [AppointmentDate], [WorkFrom], [WorkTo], [SerialNo], [ReferVerticalKhususCategoryName], [ReferVerticalKhususCategoryCode], [ReferVerticalSpesialisParentSpesialisName], [ReferVerticalSpesialisParentSpesialisCode], [ReferVerticalSpesialisParentSubSpesialisName], [ReferVerticalSpesialisParentSubSpesialisCode], [ReferReason], [IsSarana], [ReferVerticalSpesialisSaranaName], [ReferVerticalSpesialisSaranaCode], [PPKRujukanName], [PPKRujukanCode], [ReferDateVisit], [MedexType], [IsMcu], [IsBatam], [IsOutsideBatam], [Status], [StatusMCU], [McuExaminationDocs], [McuExaminationBase64], [AccidentExaminationDocs], [AccidentExaminationBase64], [Weight], [Height], [RR], [Temp], [HR], [PainScale], [Systolic], [DiastolicBP], [SpO2], [Diastole], [WaistCircumference], [BMIIndex], [BMIIndexString], [BMIState], [ClinicVisitTypes], [AwarenessId], [E], [V], [M], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [RiskOfFallingDetail], [ScrinningTriageScale], [InformationFrom], [RiskOfFalling], [ProjectId]) VALUES (12, NULL, 73, NULL, 16, 48, NULL, NULL, NULL, N'Personal', N'General Consultation', NULL, NULL, NULL, 0, 0, 0, NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'2024-08-08T11:32:30.2069415' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'-', 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, 1, 0, 6, 1, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, N'0', N'-', N'Sick', NULL, 4, 5, 6, N'Administrator', CAST(N'2024-08-08T11:33:00.2599746' AS DateTime2), N'Administrator', CAST(N'2024-08-08T11:35:15.4269628' AS DateTime2), NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[GeneralConsultanServices] OFF
GO
SET IDENTITY_INSERT [dbo].[GroupMenus] ON 

INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17987, 37, 15, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291255' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17988, 37, 16, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291297' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17989, 37, 17, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291301' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17990, 37, 18, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291310' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17991, 37, 19, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291313' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17992, 37, 20, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291322' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17993, 37, 21, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291326' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17994, 37, 22, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291334' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17995, 37, 23, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291341' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17996, 37, 24, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291346' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17997, 37, 26, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291353' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17998, 37, 27, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291359' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17999, 37, 28, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291368' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18000, 37, 29, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291373' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18001, 37, 30, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291407' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18002, 37, 31, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291417' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18003, 37, 58, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291420' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18004, 37, 59, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291429' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18005, 37, 60, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291436' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18006, 37, 62, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291441' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18007, 37, 63, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291450' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18008, 37, 64, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291455' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18009, 37, 65, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291462' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18010, 37, 66, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291469' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18011, 37, 67, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291475' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18012, 37, 68, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291482' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18013, 37, 69, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291487' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18014, 37, 71, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291496' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18015, 37, 73, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291500' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18016, 37, 74, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291508' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18017, 37, 76, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291515' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18018, 37, 78, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291520' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18019, 37, 1075, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291526' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18020, 37, 1076, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291532' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18021, 37, 1077, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291541' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18022, 37, 1078, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291544' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18023, 37, 1079, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291553' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18024, 37, 1082, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291560' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18025, 37, 1083, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291565' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18026, 37, 1088, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291574' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18027, 37, 34, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291577' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18028, 37, 32, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291586' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18029, 37, 54, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291589' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18030, 37, 52, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:03:38.2291598' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18031, 36, 15, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406501' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18032, 36, 16, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406540' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18033, 36, 18, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406543' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18034, 36, 19, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406548' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18035, 36, 20, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406551' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18036, 36, 21, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406554' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18037, 36, 22, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406557' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18038, 36, 23, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406560' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18039, 36, 24, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406563' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18040, 36, 26, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406566' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18041, 36, 27, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406569' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18042, 36, 29, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406572' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18043, 36, 30, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406575' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18044, 36, 31, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406578' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18045, 36, 32, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406597' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18046, 36, 34, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406600' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18047, 36, 35, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406603' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18048, 36, 36, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406606' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18049, 36, 37, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406610' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18050, 36, 40, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406614' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18051, 36, 41, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406617' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18052, 36, 42, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406621' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18053, 36, 46, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406624' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18054, 36, 47, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406627' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18055, 36, 49, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406631' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18056, 36, 50, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406634' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18057, 36, 52, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406637' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18058, 36, 57, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406641' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18059, 36, 58, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406644' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18060, 36, 59, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406647' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18061, 36, 60, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406650' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18062, 36, 62, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406653' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18063, 36, 63, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406656' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18064, 36, 64, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406659' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18065, 36, 65, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406662' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18066, 36, 67, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406665' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18067, 36, 69, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406668' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18068, 36, 71, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406671' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18069, 36, 72, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406674' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18070, 36, 73, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406677' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18071, 36, 74, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406680' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18072, 36, 76, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406683' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18073, 36, 78, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406686' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18074, 36, 1075, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406689' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18075, 36, 1076, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406693' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18076, 36, 1077, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406696' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18077, 36, 1080, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406699' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18078, 36, 1081, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406702' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18079, 36, 1082, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406705' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18080, 36, 1083, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406708' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18081, 36, 1085, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406712' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18082, 36, 1087, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406715' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18083, 36, 1088, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406718' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18084, 36, 1079, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406722' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18085, 36, 1078, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406725' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18086, 36, 1089, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406728' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18087, 36, 11091, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406731' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18088, 36, 11088, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406734' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18089, 36, 11089, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406736' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18090, 36, 11090, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406750' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18091, 36, 11092, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406754' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18092, 36, 11097, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406758' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18093, 36, 54, 1, 1, 1, 1, 1, N'Administrator', CAST(N'2024-05-31T18:04:00.5406761' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18546, 33, 1, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080063' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18547, 33, 2, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080096' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18548, 33, 3, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080101' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18549, 33, 4, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080104' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18550, 33, 5, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080108' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18551, 33, 6, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080111' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18552, 33, 7, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080115' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18553, 33, 8, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080117' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18554, 33, 9, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080121' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18555, 33, 10, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080123' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18556, 33, 11, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080127' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18557, 33, 12, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080131' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18558, 33, 13, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080133' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18559, 33, 14, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080137' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18560, 33, 15, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080158' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18561, 33, 16, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080161' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18562, 33, 17, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080163' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18563, 33, 18, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080165' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18564, 33, 19, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080168' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18565, 33, 20, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080171' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18566, 33, 21, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080174' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18567, 33, 22, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080177' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18568, 33, 23, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080179' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18569, 33, 24, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080182' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18570, 33, 26, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080185' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18571, 33, 27, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080189' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18572, 33, 28, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080191' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18573, 33, 29, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080194' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18574, 33, 30, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080197' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18575, 33, 31, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080200' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18576, 33, 32, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080202' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18577, 33, 34, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080205' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18578, 33, 35, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080209' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18579, 33, 36, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080212' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18580, 33, 37, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080214' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18581, 33, 38, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080217' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18582, 33, 39, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080220' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18583, 33, 40, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080224' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18584, 33, 41, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080226' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18585, 33, 42, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080229' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18586, 33, 46, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080232' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18587, 33, 47, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080234' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18588, 33, 49, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080237' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18589, 33, 50, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080240' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18590, 33, 51, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080243' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18591, 33, 52, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080246' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18592, 33, 53, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080248' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18593, 33, 54, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080251' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18594, 33, 55, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080254' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18595, 33, 56, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080257' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18596, 33, 57, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080260' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18597, 33, 58, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080263' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18598, 33, 59, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080265' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18599, 33, 60, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080268' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18600, 33, 62, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080272' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18601, 33, 63, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080275' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18602, 33, 64, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080277' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18603, 33, 65, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080280' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18604, 33, 66, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080282' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18605, 33, 67, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080284' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18606, 33, 68, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080287' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18607, 33, 69, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080290' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18608, 33, 71, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080292' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18609, 33, 72, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080294' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18610, 33, 73, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080297' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18611, 33, 74, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080299' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18612, 33, 76, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080302' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18613, 33, 77, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080305' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18614, 33, 78, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080307' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18615, 33, 1075, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080309' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18616, 33, 1076, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080312' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18617, 33, 1077, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080315' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18618, 33, 1078, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080318' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18619, 33, 1079, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080321' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18620, 33, 1080, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080324' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18621, 33, 1081, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080326' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18622, 33, 1082, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080329' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18623, 33, 1083, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080332' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18624, 33, 1085, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080335' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18625, 33, 1087, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080337' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18626, 33, 1088, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080340' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18627, 33, 1089, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080343' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18628, 33, 11088, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080346' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18629, 33, 11089, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080349' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18630, 33, 11090, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080351' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18631, 33, 11091, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080354' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18632, 33, 11092, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080356' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18633, 33, 11093, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080359' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18634, 33, 11094, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080361' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18635, 33, 11096, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080364' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18636, 33, 11097, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080366' AS DateTime2), NULL, NULL)
INSERT [dbo].[GroupMenus] ([Id], [GroupId], [MenuId], [Create], [Read], [Update], [Delete], [Import], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18637, 33, 11098, 1, 1, 1, 1, 1, N'Argi Purwanto', CAST(N'2024-06-20T16:50:19.1080369' AS DateTime2), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[GroupMenus] OFF
GO
SET IDENTITY_INSERT [dbo].[Groups] ON 

INSERT [dbo].[Groups] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (33, N'Admin', NULL, CAST(N'2024-03-05T07:35:50.5582230' AS DateTime2), N'Argi Purwanto', CAST(N'2024-06-20T16:50:18.7830247' AS DateTime2))
INSERT [dbo].[Groups] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (36, N'Nurse', N'Administrator', CAST(N'2024-04-24T10:16:38.3138301' AS DateTime2), N'Administrator', CAST(N'2024-05-31T18:04:00.2566836' AS DateTime2))
INSERT [dbo].[Groups] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (37, N'Physician', N'Administrator', CAST(N'2024-04-24T10:41:51.7672207' AS DateTime2), N'Administrator', CAST(N'2024-05-31T18:03:37.7002388' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Groups] OFF
GO
SET IDENTITY_INSERT [dbo].[HealthCenters] ON 

INSERT [dbo].[HealthCenters] ([Id], [CityId], [ProvinceId], [CountryId], [Name], [Type], [Phone], [Mobile], [Email], [Street1], [Street2], [WebsiteLink], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, NULL, NULL, NULL, N'McHealtCare', N'Clinic', NULL, NULL, NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-04-03T10:28:08.4204835' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[HealthCenters] OFF
GO
SET IDENTITY_INSERT [dbo].[Insurances] ON 

INSERT [dbo].[Insurances] ([Id], [Name], [Code], [Type], [IsBPJSKesehatan], [IsBPJSTK], [AdminFee], [Presentase], [AdminFeeMax], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'AIA', N'', N'', 0, 0, NULL, NULL, NULL, N'', CAST(N'2024-01-26T06:41:43.4851670' AS DateTime2), N'', NULL)
INSERT [dbo].[Insurances] ([Id], [Name], [Code], [Type], [IsBPJSKesehatan], [IsBPJSTK], [AdminFee], [Presentase], [AdminFeeMax], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Admedika', N'', N'', 0, 0, NULL, NULL, NULL, N'', CAST(N'2024-01-26T06:41:52.1710070' AS DateTime2), N'Administrator', CAST(N'2024-03-07T12:20:05.9399839' AS DateTime2))
INSERT [dbo].[Insurances] ([Id], [Name], [Code], [Type], [IsBPJSKesehatan], [IsBPJSTK], [AdminFee], [Presentase], [AdminFeeMax], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'BPJS Kesehatan', N'BKS', N'', 1, 0, NULL, NULL, NULL, N'', CAST(N'2024-01-26T06:42:03.2179300' AS DateTime2), N'Argi Purwanto', CAST(N'2024-07-03T13:38:49.3816254' AS DateTime2))
INSERT [dbo].[Insurances] ([Id], [Name], [Code], [Type], [IsBPJSKesehatan], [IsBPJSTK], [AdminFee], [Presentase], [AdminFeeMax], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'BPJS TK', N'TK', N'Negeri', 0, 1, NULL, NULL, NULL, N'Argi Purwanto', CAST(N'2024-07-03T13:38:41.6237456' AS DateTime2), NULL, NULL)
INSERT [dbo].[Insurances] ([Id], [Name], [Code], [Type], [IsBPJSKesehatan], [IsBPJSTK], [AdminFee], [Presentase], [AdminFeeMax], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'BPJS Ketenagakerjaan', N'BPJST', N'Negeri', 0, 1, NULL, NULL, NULL, N'Administrator', CAST(N'2024-07-09T13:58:24.5296979' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Insurances] OFF
GO
SET IDENTITY_INSERT [dbo].[JobPositions] ON 

INSERT [dbo].[JobPositions] ([Id], [DepartmentId], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, NULL, N'Nurse', NULL, CAST(N'2024-03-07T06:54:58.6029549' AS DateTime2), N'', CAST(N'2024-03-25T09:54:47.4035642' AS DateTime2))
INSERT [dbo].[JobPositions] ([Id], [DepartmentId], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, NULL, N'Physicion', N'', CAST(N'2024-03-25T09:54:55.6032212' AS DateTime2), NULL, NULL)
INSERT [dbo].[JobPositions] ([Id], [DepartmentId], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, NULL, N'Staff', N'', CAST(N'2024-03-25T09:55:05.3426903' AS DateTime2), NULL, NULL)
INSERT [dbo].[JobPositions] ([Id], [DepartmentId], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, NULL, N'Manager', NULL, CAST(N'2024-08-07T00:31:06.6325394' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[JobPositions] OFF
GO
SET IDENTITY_INSERT [dbo].[LabTestDetails] ON 

INSERT [dbo].[LabTestDetails] ([Id], [LabTestId], [LabUomId], [Name], [ResultType], [Parameter], [NormalRangeMale], [NormalRangeFemale], [ResultValueType], [Remark], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10015, 23, NULL, N'Urine', NULL, NULL, N'Negatif', N'Negatif', N'Qualitative', NULL, NULL, CAST(N'2024-07-18T11:45:41.2403676' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[LabTestDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[LabTests] ON 

INSERT [dbo].[LabTests] ([Id], [SampleTypeId], [Name], [Code], [ResultType], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (22, 7, N'Hemoglobin', N'HMG', N'Quantitative', N'Administrator', CAST(N'2024-04-25T15:13:10.2497178' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabTests] ([Id], [SampleTypeId], [Name], [Code], [ResultType], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (23, 8, N'Narkoba', N'NR', N'Qualitative', N'Administrator', CAST(N'2024-04-25T15:13:48.6365670' AS DateTime2), NULL, CAST(N'2024-07-18T11:45:46.2525760' AS DateTime2))
SET IDENTITY_INSERT [dbo].[LabTests] OFF
GO
SET IDENTITY_INSERT [dbo].[LabUoms] ON 

INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'%', N'%', N'', CAST(N'2024-03-20T15:34:41.4291075' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'g/dL', N'g/dL', N'', CAST(N'2024-03-20T15:34:53.4254494' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'g/L', N'g/L', N'', CAST(N'2024-03-20T15:35:04.0079448' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Millions/cc', N'Millions/cc', N'', CAST(N'2024-03-20T15:35:14.0510436' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'fl', N'fl', N'', CAST(N'2024-03-20T15:35:23.1668037' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'mmol/L', N'mmol/L', N'', CAST(N'2024-03-20T15:36:31.8724385' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'mmol/24h', N'mmol/24h', N'', CAST(N'2024-03-20T15:36:42.0067153' AS DateTime2), NULL, NULL)
INSERT [dbo].[LabUoms] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'mm/hr', N'mm/hr', N'', CAST(N'2024-03-20T15:37:00.1288413' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[LabUoms] OFF
GO
SET IDENTITY_INSERT [dbo].[Locations] ON 

INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, NULL, NULL, N'WHS', N'Internal Location', N'Administrator', CAST(N'2024-07-18T10:39:00.7376219' AS DateTime2), NULL, NULL)
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, 1, NULL, N'STOCK', N'Internal Location', NULL, CAST(N'2024-07-29T10:19:04.7148775' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.7187792' AS DateTime2))
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, 1, NULL, N'Pharmacy', N'Internal Location', NULL, CAST(N'2024-08-06T18:23:23.5975297' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.6112800' AS DateTime2))
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, 1, NULL, N'Lab', N'Internal Location', NULL, CAST(N'2024-08-06T17:39:36.3454377' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:39:36.3534050' AS DateTime2))
INSERT [dbo].[Locations] ([Id], [ParentLocationId], [CompanyId], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, 3, NULL, N'Lab', N'Internal Location', NULL, CAST(N'2024-07-29T04:21:51.0343466' AS DateTime2), N'Nurse', CAST(N'2024-07-29T11:21:51.0712839' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Locations] OFF
GO
SET IDENTITY_INSERT [dbo].[Medicaments] ON 

INSERT [dbo].[Medicaments] ([Id], [ProductId], [FrequencyId], [RouteId], [FormId], [UomId], [ActiveComponentId], [PregnancyWarning], [Pharmacologi], [Weather], [Food], [Cronies], [MontlyMax], [Dosage], [SignaId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, 10027, 1, 1, 12, 1, N'[1,1]', 0, 0, 0, 0, NULL, NULL, N'10', NULL, N'Administrator', CAST(N'2024-07-23T10:22:22.2927950' AS DateTime2), NULL, CAST(N'2024-08-07T00:51:41.0893293' AS DateTime2))
INSERT [dbo].[Medicaments] ([Id], [ProductId], [FrequencyId], [RouteId], [FormId], [UomId], [ActiveComponentId], [PregnancyWarning], [Pharmacologi], [Weather], [Food], [Cronies], [MontlyMax], [Dosage], [SignaId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, 10028, 1, 1, 12, 1, N'[1,1,1]', 0, 0, 0, 0, NULL, NULL, N'10', NULL, N'Administrator', CAST(N'2024-07-27T15:35:19.8599129' AS DateTime2), NULL, CAST(N'2024-08-07T00:52:15.8768543' AS DateTime2))
INSERT [dbo].[Medicaments] ([Id], [ProductId], [FrequencyId], [RouteId], [FormId], [UomId], [ActiveComponentId], [PregnancyWarning], [Pharmacologi], [Weather], [Food], [Cronies], [MontlyMax], [Dosage], [SignaId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, 10029, 7, 1, 12, 1, N'[1]', 0, 0, 0, 0, NULL, NULL, N'5', NULL, N'Administrator', CAST(N'2024-07-27T15:36:34.7449470' AS DateTime2), NULL, CAST(N'2024-08-07T00:51:30.8754962' AS DateTime2))
INSERT [dbo].[Medicaments] ([Id], [ProductId], [FrequencyId], [RouteId], [FormId], [UomId], [ActiveComponentId], [PregnancyWarning], [Pharmacologi], [Weather], [Food], [Cronies], [MontlyMax], [Dosage], [SignaId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, 10030, 10, 1, 12, 1, N'[1,1,1,1,1]', 0, 0, 0, 0, NULL, NULL, N'2', NULL, N'Administrator', CAST(N'2024-07-27T15:37:45.9526610' AS DateTime2), NULL, CAST(N'2024-08-07T00:53:01.8518597' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Medicaments] OFF
GO
SET IDENTITY_INSERT [dbo].[Menus] ON 

INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Configuration', N'<i class="fa-solid fa-gear nav-icon"></i>', NULL, 7, NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-06-10T11:17:33.5136050' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'User', N'<i class="far fa-solid fa-user"></i>', N'Configuration', 1, NULL, N'config/user', NULL, CAST(N'2024-01-24T02:24:59.5090381' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Group', N'<i class="far fa-solid fa-users"></i>', N'Configuration', 2, NULL, N'config/group', NULL, CAST(N'2024-01-24T02:24:38.7660439' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Menu', N'<i class="fa-solid fa-bars"></i>', N'Configuration', 4, N'<li class="nav-item">', N'config/menu', NULL, CAST(N'2024-01-24T02:24:41.6071169' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'Email Setting', N'<i class="fa-solid fa-envelope"></i>', N'Configuration', 5, NULL, N'config/email-setting', NULL, CAST(N'2024-01-30T03:22:03.9060248' AS DateTime2), N'Ganggar', CAST(N'2024-03-06T10:41:02.0002064' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'Email Template', N'<i class="fa-regular fa-envelope"></i>', N'Configuration', 4, NULL, N'config/email-template', NULL, CAST(N'2024-01-24T02:24:36.0636577' AS DateTime2), N'Ganggar', CAST(N'2024-03-06T10:41:09.1051720' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'Company', N'<i class="fa-solid fa-building"></i>', N'Configuration', 6, NULL, N'config/companies', NULL, CAST(N'2024-01-24T02:24:23.0546426' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'Country', N'<i class="fa-solid fa-earth-americas"></i>', N'Configuration', 7, NULL, N'config/country', NULL, CAST(N'2024-01-24T02:24:27.6593541' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Province', N'<i class="fa-solid fa-city"></i>', N'Configuration', 8, N'<li class="nav-item">', N'config/province', NULL, CAST(N'2024-01-24T02:24:49.2879576' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'City', N'<i class="fa-solid fa-city"></i>', N'Configuration', 9, N'<li class="nav-item">', N'config/city', NULL, CAST(N'2024-01-24T02:24:15.1160427' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'District', N'<i class="fa-solid fa-city"></i>', N'Configuration', 10, N'<li class="nav-item">', N'config/district', NULL, CAST(N'2024-01-24T02:24:30.4999620' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'Sub-District', N'<i class="fa-solid fa-city"></i>', N'Configuration', 11, N'<li class="nav-item">', N'config/village', NULL, CAST(N'2024-01-24T02:24:56.5840183' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'Religion', N'<i class="fa-solid fa-hands-praying"></i>', N'Configuration', 12, N'<li class="nav-item">', N'config/religion', NULL, CAST(N'2024-01-24T02:24:51.7029724' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'Occupational', N'<i class="fa-solid fa-user-doctor"></i>', N'Configuration', 13, N'<li class="nav-item">', N'config/occupational', NULL, CAST(N'2024-01-24T02:24:44.2854431' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, N'Clinic Services', N'<i class="fa-solid fa-money-check nav-icon"></i>', NULL, 1, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, N'Administrator', CAST(N'2024-04-17T13:26:09.1590300' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, N'General Consultation Service', NULL, N'Clinic Services', 1, N'<li class="nav-item">', N'clinic-service/general-consultation-service', NULL, NULL, N'Administrator', CAST(N'2024-04-17T13:27:14.7776634' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, N'Accident', NULL, N'Clinic Services', 4, N'<li class="nav-item">', N'clinic-service/accident', NULL, NULL, N'Argi Purwanto', CAST(N'2024-06-20T16:51:17.6340246' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, N'Telemedicine', NULL, N'Transaction', 3, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, N'Medical Check Up & Certificate Management', NULL, N'Transaction', 4, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, N'Education & Awareness Program', NULL, N'Transaction', 5, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (21, N'Maternity Check Up', NULL, N'Transaction', 6, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (22, N'Immunization & Vaccination Program', NULL, N'Transaction', 7, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (23, N'Cronic Diseases Management', NULL, N'Transaction', 9, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (24, N'Wellness Management', NULL, N'Transaction', 9, N'<li class="nav-item">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (26, N'Queue Counter', NULL, N'Queue', 2, N'<li class="nav-item">', N'queue/queue-counter', NULL, NULL, N'Argi P', CAST(N'2024-02-16T14:51:25.9207652' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (27, N'Queue Display', NULL, N'Queue', 3, N'<li class="nav-item">', N'queue/queue-display', NULL, NULL, N'Argi P', CAST(N'2024-02-16T14:52:13.9867606' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (28, N'Reporting & Analytic', NULL, N'Clinic Services', 13, N'<li class="nav-item">', N'clinic-service/reporting-&-analytic', NULL, CAST(N'2024-01-29T09:10:55.1275517' AS DateTime2), N'Administrator', CAST(N'2024-04-17T17:12:30.1682226' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (29, N'Patient', N'<i class="fa-solid fa-hospital-user nav-icon"></i>', NULL, 2, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (30, N'Patient Data', NULL, N'Patient', 1, N'<li class="nav-item">', N'patient/patient-data', NULL, CAST(N'2024-01-24T07:05:24.7905418' AS DateTime2), N'Administrator', CAST(N'2024-02-22T11:49:13.1538452' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (31, N'Family Relation', NULL, N'Patient', 2, N'<li class="nav-item">', N'patient/family-relation', NULL, CAST(N'2024-01-29T08:18:07.9479786' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (32, N'Pharmacy', N'<i class="fa-solid fa-prescription-bottle-medical nav-icon"></i>', NULL, 3, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (34, N'Prescription', NULL, N'Pharmacy', 1, N'<li class="nav-item">', N'pharmacy/prescription', NULL, NULL, N'Administrator', CAST(N'2024-04-25T17:27:21.7196298' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (35, N'Medicament Group', NULL, N'Pharmacy', 2, N'<li class="nav-item">', N'pharmacy/medicament-group', NULL, NULL, N'Administrator', CAST(N'2024-04-17T16:36:22.5043992' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (36, N'Signa', NULL, N'Pharmacy', 3, N'<li class="nav-item">', N'pharmacy/signa', NULL, NULL, N'Administrator', CAST(N'2024-04-17T16:36:56.2941377' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (37, N'Medicine Dosage', NULL, N'Pharmacy', 4, N'<li class="nav-item">', N'pharmacy/drug-dosage', NULL, NULL, N'Administrator', CAST(N'2024-06-02T13:45:56.8521485' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (38, N'Active Component', NULL, N'Pharmacy', 5, N'<li class="nav-item">', N'pharmacy/active-components', NULL, NULL, N'Administrator', CAST(N'2024-04-17T16:39:12.9893594' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (39, N'Reporting Pharmacy', NULL, N'Pharmacy', 9, N'<li class="nav-item">

                        <NavLink href="Reporting" class="nav-link">

                            <i class="far fa-circle nav-icon"></i>

                            <p>Reporting</p>

                        </NavLink>

                    </li>', N'pharmacy/reporting-pharmacy', NULL, NULL, N'Administrator', CAST(N'2024-04-23T10:19:09.9739607' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (40, N'Inventory', N'<i class="fa-solid fa-box nav-icon"></i>', NULL, 4, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (41, N'Product', NULL, N'Inventory', 1, N'<li class="nav-item">', N'inventory/product', NULL, NULL, N'Dr. Ali Purwanto', CAST(N'2024-04-19T10:38:27.2234227' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (42, N'Product Category', NULL, N'Inventory', 2, N'<li class="nav-item">', N'inventory/product-categories', NULL, NULL, N'Administrator', CAST(N'2024-04-17T17:08:14.5691059' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (46, N'Goods Receipt', NULL, N'Inventory', 5, N'<li class="nav-item">', N'inventory/receiving-transfer', NULL, NULL, N'Administrator', CAST(N'2024-05-22T16:02:10.9410782' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (47, N'Inventory Adjusment', NULL, N'Inventory', 3, N'<li class="nav-item">', N'inventory/inventory-adjusment', NULL, NULL, N'Administrator', CAST(N'2024-06-01T14:14:44.4890768' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (49, N'UoM', NULL, N'Inventory', 7, N'<li class="nav-item">', N'inventory/uoms', NULL, NULL, N'Administrator', CAST(N'2024-05-02T10:11:54.8930112' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (50, N'Uom Category', NULL, N'Inventory', 6, N'<li class="nav-item">', N'inventory/uom-categories', NULL, NULL, N'Administrator', CAST(N'2024-05-02T10:11:42.1194967' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (51, N'Reporting Inventory', NULL, N'Inventory', 9, NULL, N'inventory/reporting -inventory', NULL, NULL, N'Administrator', CAST(N'2024-05-02T10:12:29.8063839' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (52, N'Employee', N'<i class="fa-solid fa-user nav-icon"></i>', NULL, 5, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (53, N'Employee Data', N'<i class="fa-solid fa-users"></i>', N'Employee', 1, N'<li class="nav-item">', N'employee/employees', NULL, NULL, N'Argi Purwanto', CAST(N'2024-02-07T10:34:12.9824357' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (54, N'Sick Leave Management', N'<i class="fa-solid fa-viruses"></i>', N'Employee', 2, N'<li class="nav-item">', N'employee/sick-leave', NULL, NULL, N'Administrator', CAST(N'2024-05-31T18:02:43.4494986' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (55, N'Claim Management', N'<i class="fa-solid fa-heart-circle-exclamation"></i>', N'Employee', 3, N'<li class="nav-item">', NULL, NULL, NULL, N'Administrator', CAST(N'2024-05-22T10:27:47.0214616' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (56, N'Department', N'<i class="fa-solid fa-building-user"></i>', N'Employee', 4, N'<li class="nav-item">', N'employee/department', NULL, CAST(N'2024-01-25T06:40:07.3777916' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (57, N'Job Potition', N'<i class="fa-regular fa-id-card"></i>', N'Employee', 5, N'<li class="nav-item">', N'employee/job-position', NULL, CAST(N'2024-01-25T06:38:55.3929407' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (58, N'Medical', N'<i class="fa-solid fa-notes-medical nav-icon"></i>', NULL, 6, N'<NavLink href="" class="nav-link" Match="NavLinkMatch.Prefix">', NULL, NULL, NULL, N'Argi P', CAST(N'2024-02-16T09:56:37.4368231' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (59, N'Practitioner', N'<i class="fa-solid fa-user-nurse"></i>', N'Medical', 1, N'<li class="nav-item">', N'medical/practitioner', NULL, NULL, N'Argi P', CAST(N'2024-02-07T13:32:08.0604775' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (60, N'Doctor Scheduled', NULL, N'Medical', 2, N'<li class="nav-item">', N'medical/doctor-scheduled', NULL, NULL, N'Argi Purwanto', CAST(N'2024-02-02T13:42:52.1299321' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (62, N'Insurance', NULL, N'Medical', 4, N'<li class="nav-item">', N'medical/insurance', NULL, CAST(N'2024-01-26T03:52:57.7403764' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (63, N'Speciality', NULL, N'Medical', 5, N'<li class="nav-item">', N'medical/speciality', NULL, CAST(N'2024-01-30T03:23:37.1435744' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (64, N'Diagnosis', NULL, N'Medical', 6, N'<li class="nav-item">', N'medical/diagnosis', NULL, CAST(N'2024-01-26T09:49:39.2649833' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (65, N'Procedure', NULL, N'Medical', 7, N'<li class="nav-item">', N'medical/procedure', NULL, CAST(N'2024-01-29T07:35:30.0861530' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (66, N'Chronic Diagnosis', NULL, N'Medical', 9, N'<li class="nav-item">', N'medical/cronis-category', NULL, CAST(N'2024-01-26T08:47:44.6396330' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (67, N'Health Center', NULL, N'Medical', 10, N'<li class="nav-item">', N'medical/health-center', NULL, CAST(N'2024-01-24T10:58:30.6638804' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (68, N'Building & Location', NULL, N'Medical', 11, N'<li class="nav-item">', N'medical/building', NULL, CAST(N'2024-01-25T03:20:15.8164850' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (69, N'Disease category', NULL, N'Medical', 8, N'<li class="nav-item">', N'medical/disease-category', NULL, CAST(N'2024-01-26T08:46:16.7014936' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (71, N'Service', NULL, N'Medical', 13, NULL, N'medical/service', NULL, CAST(N'2024-01-25T07:54:55.8281004' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (72, N'Location', N'<i class="fa-solid fa-location-dot"></i>', N'Inventory', 8, NULL, N'inventory/location', NULL, CAST(N'2024-01-29T04:47:45.7620716' AS DateTime2), N'Administrator', CAST(N'2024-05-02T10:12:10.7140997' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (73, N'Doctor Schedule Slot', NULL, N'Medical', 2, NULL, N'medical/doctor-schedule-slot', N'Argi Purwanto', CAST(N'2024-02-02T17:02:49.7902515' AS DateTime2), N'Argi Purwanto', CAST(N'2024-02-05T16:09:08.0266483' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (74, N'Insurance Policy', NULL, N'Patient', 3, NULL, N'patient/insurance-policy', N'Administrator', CAST(N'2024-02-13T09:57:02.0760875' AS DateTime2), N'Administrator', CAST(N'2024-05-27T15:33:40.5897023' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (76, N'Queue', NULL, NULL, 1, NULL, NULL, N'Argi P', CAST(N'2024-02-16T14:47:04.8991689' AS DateTime2), N'Argi P', CAST(N'2024-02-16T14:47:18.2368276' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (77, N'Kiosk Departement', NULL, N'Queue', 4, NULL, N'queue/kiosk-departement', N'Argi P', CAST(N'2024-02-16T15:02:37.7604096' AS DateTime2), N'Argi P', CAST(N'2024-02-20T11:35:14.7783636' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (78, N'Configuration Kiosk', NULL, N'Queue', 5, NULL, N'queue/configuration-kiosk', N'Argi P', CAST(N'2024-02-16T15:08:51.2828635' AS DateTime2), N'Argi P', CAST(N'2024-02-22T17:17:44.3477107' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1075, N'Nursing Diagnosis', NULL, N'Medical', 14, NULL, N'medical/nursing-diagnosis', N'Argi P', CAST(N'2024-02-19T17:48:45.5780786' AS DateTime2), N'Administrator', CAST(N'2024-02-21T17:31:31.4013276' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1076, N'Template Page', NULL, NULL, 0, NULL, N'template-page', N'Nurse', CAST(N'2024-03-04T10:29:53.9863389' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1077, N'Procedure Room', NULL, N'Clinic Services', 3, NULL, N'clinic-service/procedure-room', N'Administrator', CAST(N'2024-04-17T11:36:56.2139366' AS DateTime2), N'Argi Purwanto', CAST(N'2024-06-20T16:50:55.1729732' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1078, N'BPJS', NULL, NULL, 1, NULL, NULL, N'Administrator', CAST(N'2024-04-17T16:29:25.8550601' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1079, N'BPJS Classification', NULL, N'BPJS', 1, NULL, N'bpjs/bpjs-classification', N'Administrator', CAST(N'2024-04-17T16:31:40.4851845' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1080, N'Drug Form', NULL, N'Pharmacy', 7, NULL, N'pharmacy/drug-form', N'Administrator', CAST(N'2024-04-17T16:55:29.5706858' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1081, N'Drug Route', NULL, N'Pharmacy', 8, NULL, N'pharmacy/drug-routes', N'Administrator', CAST(N'2024-04-17T16:57:09.0987713' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1082, N'Sample Type', NULL, N'Medical', 16, NULL, N'medical/sample-type', N'Administrator', CAST(N'2024-04-17T17:00:32.4706647' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1083, N'Lab Test', NULL, N'Medical', 15, NULL, N'medical/lab-test', N'Administrator', CAST(N'2024-04-17T17:00:58.1686530' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1085, N'UOM Lab', NULL, N'Medical', 17, NULL, N'medical/lab-uom', N'Administrator', CAST(N'2024-04-17T17:02:13.5367558' AS DateTime2), N'Dr. Ali Purwanto', CAST(N'2024-04-19T10:04:18.9439833' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1087, N'Internal Transfer', NULL, N'Inventory', 4, NULL, N'inventory/internal-transfer', N'Administrator', CAST(N'2024-04-17T17:03:21.7742302' AS DateTime2), N'Administrator', CAST(N'2024-05-02T10:11:03.9678205' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1088, N'Kiosk', NULL, N'Queue', 4, NULL, N'queue/kiosk', N'Administrator', CAST(N'2024-04-24T11:15:23.4540646' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1089, N'Sytem Parameter', NULL, N'BPJS', 2, NULL, N'bpjs/system-parameter', N'Administrator', CAST(N'2024-05-02T13:37:10.7596955' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11088, N'BPJS Configuration', NULL, NULL, 1, NULL, N'', N'Administrator', CAST(N'2024-05-08T14:14:59.0244253' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11089, N'physician', NULL, N'BPJS Configuration', 1, NULL, N'bpjs-integration/doctor', N'Administrator', CAST(N'2024-05-08T14:15:43.5141961' AS DateTime2), N'Administrator', CAST(N'2024-05-08T14:19:02.9810070' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11090, N'Poli', NULL, N'BPJS Configuration', 2, NULL, N'bpjs-integration/poli', N'Administrator', CAST(N'2024-05-08T14:17:29.3439754' AS DateTime2), N'Administrator', CAST(N'2024-05-08T14:21:36.0840624' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11091, N'Medical Procedur', NULL, N'BPJS Configuration', 3, NULL, N'bpjs-integration/tindakan', N'Administrator', CAST(N'2024-05-08T14:18:01.1154378' AS DateTime2), N'Administrator', CAST(N'2024-05-08T14:22:04.2734220' AS DateTime2))
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11092, N'Awareness', NULL, N'BPJS Configuration', 4, NULL, N'bpjs-integration/awarness', N'Administrator', CAST(N'2024-05-13T10:32:36.0828643' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11093, N'Diagnosis', NULL, N'BPJS Configuration', 4, NULL, N'bpjs-integration/diagnosis', N'Administrator', CAST(N'2024-05-16T14:31:59.7200730' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11094, N'Provider', NULL, N'BPJS Configuration', 5, NULL, N'bpjs-integration/provider', N'Administrator', CAST(N'2024-05-16T14:32:46.3836596' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11096, N'Drug', NULL, N'BPJS Configuration', 7, NULL, N'bpjs-integration/drug', N'Administrator', CAST(N'2024-05-16T14:34:28.7759255' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11097, N'Allergy', NULL, N'BPJS Configuration', 6, NULL, N'bpjs-integration/allergy', N'Administrator', CAST(N'2024-05-27T15:48:46.0850160' AS DateTime2), NULL, NULL)
INSERT [dbo].[Menus] ([Id], [Name], [Icon], [ParentMenu], [Sequence], [Html], [Url], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11098, N'Medical Checkup', NULL, N'Clinic Services', 2, NULL, N'clinic-service/medical-checkup', N'Argi Purwanto', CAST(N'2024-06-20T16:50:02.8487330' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Menus] OFF
GO
SET IDENTITY_INSERT [dbo].[NursingDiagnoses] ON 

INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Bersihan jalan nafas tidak efektif', N'	D.0001', N'Administrator', CAST(N'2024-03-06T18:27:16.0567566' AS DateTime2), N'', CAST(N'2024-03-25T16:26:17.7759578' AS DateTime2))
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Gangguan Penyapihan Ventilator', N'D.0002', NULL, CAST(N'2024-04-22T13:53:18.6381758' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Gangguan Pertukaran Gas', N'D.0003', NULL, CAST(N'2024-04-22T13:53:44.0653620' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Gangguan Ventilasi Spontan', N'D.0004', NULL, CAST(N'2024-04-22T13:54:01.0738919' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'Pola Nafas Tidak Efektif', N'D.0005', NULL, CAST(N'2024-04-22T13:54:32.1456139' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'Resiko Aspirasi', N'D.0006', NULL, CAST(N'2024-04-22T13:54:46.1705973' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'Penurunan Curah Jantung', N'D.0008', NULL, CAST(N'2024-04-22T13:55:03.8000585' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'Perfusi Perifer Tidak Efektif', N'D.0009', NULL, CAST(N'2024-04-22T13:55:18.1231475' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Resiko Penurunan Curah Jantung', N'D.0011', NULL, CAST(N'2024-04-22T13:55:35.7739987' AS DateTime2), NULL, NULL)
INSERT [dbo].[NursingDiagnoses] ([Id], [Problem], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'Risiko Perdarahan', N'D.0012', NULL, CAST(N'2024-04-22T13:56:13.6459463' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[NursingDiagnoses] OFF
GO
SET IDENTITY_INSERT [dbo].[Occupationals] ON 

INSERT [dbo].[Occupationals] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'ASN', N'', N'Administrator', CAST(N'2024-03-06T17:17:52.7967522' AS DateTime2), NULL, NULL)
INSERT [dbo].[Occupationals] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'TNI/POLRI', N'', N'Administrator', CAST(N'2024-03-06T17:18:02.3334867' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Occupationals] OFF
GO
SET IDENTITY_INSERT [dbo].[Pharmacies] ON 

INSERT [dbo].[Pharmacies] ([Id], [PatientId], [PractitionerId], [PrescriptionLocationId], [MedicamentGroupId], [ServiceId], [PaymentMethod], [ReceiptDate], [IsWeather], [IsFarmacologi], [IsFood], [Status], [LocationId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20017, 72, 48, 3, NULL, 16, N'Personal', CAST(N'2024-08-07T00:44:50.0457780' AS DateTime2), 0, 0, 1, 4, NULL, NULL, CAST(N'2024-08-07T00:46:29.9278827' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.6440961' AS DateTime2))
INSERT [dbo].[Pharmacies] ([Id], [PatientId], [PractitionerId], [PrescriptionLocationId], [MedicamentGroupId], [ServiceId], [PaymentMethod], [ReceiptDate], [IsWeather], [IsFarmacologi], [IsFood], [Status], [LocationId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20018, 73, 48, 3, NULL, 16, N'Personal', CAST(N'2024-08-08T11:35:20.3992251' AS DateTime2), 0, 0, 0, 2, NULL, N'Administrator', CAST(N'2024-08-08T11:41:54.0337127' AS DateTime2), N'Nurse', CAST(N'2024-08-08T11:43:56.8139039' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Pharmacies] OFF
GO
SET IDENTITY_INSERT [dbo].[PharmacyLogs] ON 

INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20056, 20017, 2, 0, N'Administrator', CAST(N'2024-08-07T00:46:33.8067832' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20057, 20017, 2, 1, NULL, CAST(N'2024-08-07T00:58:51.5458151' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20058, 20017, 47, 2, N'Nurse', CAST(N'2024-08-07T01:22:34.7898019' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20059, 20017, 47, 3, N'Nurse', CAST(N'2024-08-07T01:23:14.3410440' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20060, 20017, 47, 4, N'Nurse', CAST(N'2024-08-07T01:23:23.6696071' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20061, 20018, 2, 0, N'Administrator', CAST(N'2024-08-08T11:41:59.6947190' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20062, 20018, 2, 1, N'Administrator', CAST(N'2024-08-08T11:42:59.8479259' AS DateTime2), NULL, NULL)
INSERT [dbo].[PharmacyLogs] ([Id], [PharmacyId], [UserById], [status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20063, 20018, 47, 2, N'Nurse', CAST(N'2024-08-08T11:43:57.2097971' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[PharmacyLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Prescriptions] ON 

INSERT [dbo].[Prescriptions] ([Id], [PharmacyId], [DrugFromId], [DrugRouteId], [DrugDosageId], [ProductId], [UomId], [ActiveComponentId], [DosageFrequency], [Stock], [Dosage], [GivenAmount], [PriceUnit], [DrugFormId], [SignaId], [MedicamentGroupId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20016, 20017, 7, 1, 7, 10029, 1, N'[1]', N'5/Sesudah Makan', 25, 5, 5, 100, NULL, NULL, NULL, N'Administrator', CAST(N'2024-08-07T00:46:30.9888719' AS DateTime2), NULL, NULL)
INSERT [dbo].[Prescriptions] ([Id], [PharmacyId], [DrugFromId], [DrugRouteId], [DrugDosageId], [ProductId], [UomId], [ActiveComponentId], [DosageFrequency], [Stock], [Dosage], [GivenAmount], [PriceUnit], [DrugFormId], [SignaId], [MedicamentGroupId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20017, 20018, 12, 1, 7, 10029, 1, N'[1]', N'5/Sesudah Makan', 20, 5, 5, 100, NULL, NULL, NULL, N'Administrator', CAST(N'2024-08-08T11:41:55.8172494' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Prescriptions] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductCategories] ON 

INSERT [dbo].[ProductCategories] ([Id], [Name], [Code], [CostingMethod], [InventoryValuation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Drug', N'drug', NULL, NULL, N'Administrator', CAST(N'2024-07-18T10:33:56.4505946' AS DateTime2), NULL, NULL)
INSERT [dbo].[ProductCategories] ([Id], [Name], [Code], [CostingMethod], [InventoryValuation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Alkes', N'', N'Standart Price', N'Manual', N'Administrator', CAST(N'2024-04-19T16:56:29.2765885' AS DateTime2), NULL, NULL)
INSERT [dbo].[ProductCategories] ([Id], [Name], [Code], [CostingMethod], [InventoryValuation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Services', N'', N'Standart Price', N'Manual', N'Administrator', CAST(N'2024-04-22T11:10:30.6287918' AS DateTime2), NULL, NULL)
INSERT [dbo].[ProductCategories] ([Id], [Name], [Code], [CostingMethod], [InventoryValuation], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Persediaan obat-obat', N'', N'Standart Price', N'Manual', N'Administrator', CAST(N'2024-04-22T11:11:22.9366490' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[ProductCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [BpjsClassificationId], [UomId], [ProductCategoryId], [CompanyId], [PurchaseUomId], [TraceAbility], [ProductType], [HospitalType], [SalesPrice], [Tax], [Cost], [InternalReference], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [Brand], [EquipmentCode], [EquipmentCondition], [LastCalibrationDate], [NextCalibrationDate], [YearOfPurchase], [IsOralMedication], [IsTopicalMedication]) VALUES (10027, N'Ibuprofen ', 2, 8, 2, NULL, 8, 1, N'Storable Product', N'Medicament', N'100', N'11%', N'13', NULL, NULL, CAST(N'2024-07-29T10:19:04.6928615' AS DateTime2), NULL, CAST(N'2024-08-07T00:51:41.0698194' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, 1, 0)
INSERT [dbo].[Products] ([Id], [Name], [BpjsClassificationId], [UomId], [ProductCategoryId], [CompanyId], [PurchaseUomId], [TraceAbility], [ProductType], [HospitalType], [SalesPrice], [Tax], [Cost], [InternalReference], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [Brand], [EquipmentCode], [EquipmentCondition], [LastCalibrationDate], [NextCalibrationDate], [YearOfPurchase], [IsOralMedication], [IsTopicalMedication]) VALUES (10028, N'Parasetamol ', 3, 8, 2, NULL, 8, 1, N'Storable Product', N'Medicament', N'100', N'11%', N'4', NULL, NULL, CAST(N'2024-07-29T10:19:04.7046684' AS DateTime2), NULL, CAST(N'2024-08-07T00:52:15.8621408' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, 1, 0)
INSERT [dbo].[Products] ([Id], [Name], [BpjsClassificationId], [UomId], [ProductCategoryId], [CompanyId], [PurchaseUomId], [TraceAbility], [ProductType], [HospitalType], [SalesPrice], [Tax], [Cost], [InternalReference], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [Brand], [EquipmentCode], [EquipmentCondition], [LastCalibrationDate], [NextCalibrationDate], [YearOfPurchase], [IsOralMedication], [IsTopicalMedication]) VALUES (10029, N'Amoksisilin', NULL, 1, 1, NULL, 1, 1, N'Storable Product', N'Medicament', N'100', N'11%', N'100', NULL, NULL, CAST(N'2024-08-06T18:23:17.8837875' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.5133877' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, 1, 0)
INSERT [dbo].[Products] ([Id], [Name], [BpjsClassificationId], [UomId], [ProductCategoryId], [CompanyId], [PurchaseUomId], [TraceAbility], [ProductType], [HospitalType], [SalesPrice], [Tax], [Cost], [InternalReference], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [Brand], [EquipmentCode], [EquipmentCondition], [LastCalibrationDate], [NextCalibrationDate], [YearOfPurchase], [IsOralMedication], [IsTopicalMedication]) VALUES (10030, N'fluoxetine', NULL, 1, 1, NULL, 1, 0, N'Storable Product', N'Medicament', N'100', N'11%', N'120', NULL, NULL, CAST(N'2024-08-06T18:23:23.5975227' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.6112804' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[ReceivingLogs] ON 

INSERT [dbo].[ReceivingLogs] ([Id], [ReceivingId], [SourceId], [UserById], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10040, 10015, 2, 2, 0, N'Administrator', CAST(N'2024-07-29T17:16:12.5800498' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingLogs] ([Id], [ReceivingId], [SourceId], [UserById], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10041, 10015, 2, 2, 1, N'Administrator', CAST(N'2024-07-29T17:18:53.1231681' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingLogs] ([Id], [ReceivingId], [SourceId], [UserById], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10042, 10015, 2, 2, 2, N'Administrator', CAST(N'2024-07-29T17:19:04.7333081' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingLogs] ([Id], [ReceivingId], [SourceId], [UserById], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20043, 20016, 3, 2, 0, N'Administrator', CAST(N'2024-08-07T00:38:00.9336604' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingLogs] ([Id], [ReceivingId], [SourceId], [UserById], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20044, 20016, 3, 2, 1, N'Administrator', CAST(N'2024-08-07T00:38:30.2840270' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingLogs] ([Id], [ReceivingId], [SourceId], [UserById], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20045, 20016, 3, 2, 2, N'Administrator', CAST(N'2024-08-07T00:38:35.4892488' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[ReceivingLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[ReceivingStockDetails] ON 

INSERT [dbo].[ReceivingStockDetails] ([Id], [ReceivingStockId], [ProductId], [Qty], [Batch], [ExpiredDate], [StockId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10032, 10015, 10027, 10, N'1122', CAST(N'2024-07-30T00:00:00.0000000' AS DateTime2), NULL, N'Administrator', CAST(N'2024-07-29T17:16:12.3659120' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingStockDetails] ([Id], [ReceivingStockId], [ProductId], [Qty], [Batch], [ExpiredDate], [StockId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10033, 10015, 10027, 20, N'1133', CAST(N'2024-08-05T00:00:00.0000000' AS DateTime2), NULL, N'Administrator', CAST(N'2024-07-29T17:16:12.3659140' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingStockDetails] ([Id], [ReceivingStockId], [ProductId], [Qty], [Batch], [ExpiredDate], [StockId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10034, 10015, 10028, 5, N'1144', CAST(N'2024-08-30T00:00:00.0000000' AS DateTime2), NULL, N'Administrator', CAST(N'2024-07-29T17:16:12.3659142' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingStockDetails] ([Id], [ReceivingStockId], [ProductId], [Qty], [Batch], [ExpiredDate], [StockId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10037, 10015, 10030, 20, NULL, NULL, NULL, N'Administrator', CAST(N'2024-07-29T17:18:46.9240918' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingStockDetails] ([Id], [ReceivingStockId], [ProductId], [Qty], [Batch], [ExpiredDate], [StockId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20038, 20016, 10029, 50, N'AMK002', CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2), NULL, N'Administrator', CAST(N'2024-08-07T00:38:00.7529715' AS DateTime2), NULL, NULL)
INSERT [dbo].[ReceivingStockDetails] ([Id], [ReceivingStockId], [ProductId], [Qty], [Batch], [ExpiredDate], [StockId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20039, 20016, 10030, 70, NULL, NULL, NULL, N'Administrator', CAST(N'2024-08-07T00:38:23.4209276' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[ReceivingStockDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[ReceivingStocks] ON 

INSERT [dbo].[ReceivingStocks] ([Id], [DestinationId], [SchenduleDate], [KodeReceiving], [NumberPurchase], [Reference], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10015, 2, CAST(N'2024-07-29T17:12:31.5950981' AS DateTime2), N'WH-IN/0001', N'POTEST001', NULL, 2, N'Administrator', CAST(N'2024-07-29T17:16:11.5745607' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.7289428' AS DateTime2))
INSERT [dbo].[ReceivingStocks] ([Id], [DestinationId], [SchenduleDate], [KodeReceiving], [NumberPurchase], [Reference], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20016, 3, CAST(N'2024-08-07T00:36:32.7001347' AS DateTime2), N'WH-IN/0001', N'PO003', NULL, 2, N'Administrator', CAST(N'2024-08-07T00:38:00.5908079' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:38:35.4822886' AS DateTime2))
SET IDENTITY_INSERT [dbo].[ReceivingStocks] OFF
GO
SET IDENTITY_INSERT [dbo].[Religions] ON 

INSERT [dbo].[Religions] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Islam', NULL, NULL, NULL, NULL)
INSERT [dbo].[Religions] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Kristen Katolik', NULL, NULL, NULL, NULL)
INSERT [dbo].[Religions] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Kristen Protestan', NULL, NULL, NULL, NULL)
INSERT [dbo].[Religions] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Hindu ', NULL, NULL, NULL, NULL)
INSERT [dbo].[Religions] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'Budha', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Religions] OFF
GO
SET IDENTITY_INSERT [dbo].[SampleTypes] ON 

INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'Blood', NULL, N'', CAST(N'2024-03-20T15:32:57.3609702' AS DateTime2), NULL, NULL)
INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'Urine', NULL, N'', CAST(N'2024-03-20T15:33:01.8246686' AS DateTime2), NULL, NULL)
INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Saliva', NULL, N'', CAST(N'2024-03-20T15:33:11.0115707' AS DateTime2), NULL, NULL)
INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'Sputum', NULL, N'', CAST(N'2024-03-20T15:33:21.2494106' AS DateTime2), NULL, NULL)
INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'Feces', NULL, N'', CAST(N'2024-03-20T15:33:29.0642660' AS DateTime2), NULL, NULL)
INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'Semen', NULL, N'', CAST(N'2024-03-20T15:33:45.5514243' AS DateTime2), NULL, NULL)
INSERT [dbo].[SampleTypes] ([Id], [Name], [Description], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'Tissues', NULL, N'', CAST(N'2024-03-20T15:33:58.1059794' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[SampleTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Services] ON 

INSERT [dbo].[Services] ([Id], [Name], [Code], [Quota], [IsPatient], [IsKiosk], [IsMcu], [ServicedId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'Nurse station', N'', N'', 0, 1, 0, NULL, N'Nurse', CAST(N'2024-05-16T16:00:00.2270830' AS DateTime2), N'Nurse', CAST(N'2024-05-16T16:03:21.2788864' AS DateTime2))
INSERT [dbo].[Services] ([Id], [Name], [Code], [Quota], [IsPatient], [IsKiosk], [IsMcu], [ServicedId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, N'General Clinic', N'001', N'', 1, 1, 0, 14, N'Nurse', CAST(N'2024-05-16T16:04:20.0070819' AS DateTime2), N'Administrator', CAST(N'2024-05-27T17:02:55.0691882' AS DateTime2))
INSERT [dbo].[Services] ([Id], [Name], [Code], [Quota], [IsPatient], [IsKiosk], [IsMcu], [ServicedId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, N'Pharmacy', N'', N'', 1, 1, 0, 14, N'Nurse', CAST(N'2024-05-16T16:29:50.0095074' AS DateTime2), N'Administrator', CAST(N'2024-05-27T17:03:00.6095864' AS DateTime2))
INSERT [dbo].[Services] ([Id], [Name], [Code], [Quota], [IsPatient], [IsKiosk], [IsMcu], [ServicedId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, N'Dental & Oral Clinic', N'002', N'', 1, 1, 0, 14, N'Administrator', CAST(N'2024-05-27T16:16:10.6022153' AS DateTime2), N'Administrator', CAST(N'2024-05-27T17:02:47.1022601' AS DateTime2))
INSERT [dbo].[Services] ([Id], [Name], [Code], [Quota], [IsPatient], [IsKiosk], [IsMcu], [ServicedId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (21, N'Maternal and Child Health Clinic', N'003', N'', 0, 0, 0, NULL, N'Administrator', CAST(N'2024-05-27T16:16:39.6892365' AS DateTime2), N'Administrator', CAST(N'2024-05-27T16:17:52.8470739' AS DateTime2))
INSERT [dbo].[Services] ([Id], [Name], [Code], [Quota], [IsPatient], [IsKiosk], [IsMcu], [ServicedId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (22, N'MCU', N'MCU', N'1', 0, 0, 1, 14, N'Argi Purwanto', CAST(N'2024-07-09T15:53:58.6540496' AS DateTime2), NULL, CAST(N'2024-07-11T15:37:42.7417842' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Services] OFF
GO
SET IDENTITY_INSERT [dbo].[Signas] ON 

INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'1x3 Setelah makan', N'Administrator', CAST(N'2024-07-18T10:34:15.4213880' AS DateTime2), NULL, NULL)
INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'1 x 1', N'Administrator', CAST(N'2024-04-03T14:44:11.5067777' AS DateTime2), N'Administrator', CAST(N'2024-04-03T14:44:30.8171657' AS DateTime2))
INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'2 x 1', N'Administrator', CAST(N'2024-04-03T14:44:36.8871908' AS DateTime2), NULL, NULL)
INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'3 x 1', N'Administrator', CAST(N'2024-04-03T14:44:44.9499307' AS DateTime2), NULL, NULL)
INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (5, N'3 x 250 mg', N'Administrator', CAST(N'2024-04-03T14:45:11.8936718' AS DateTime2), NULL, NULL)
INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (6, N'2 x 1 Tablet', N'Administrator', CAST(N'2024-04-03T14:45:26.9451462' AS DateTime2), NULL, NULL)
INSERT [dbo].[Signas] ([Id], [Name], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'3 x 2', N'Administrator', CAST(N'2024-04-03T14:45:52.3372135' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Signas] OFF
GO
SET IDENTITY_INSERT [dbo].[Specialities] ON 

INSERT [dbo].[Specialities] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Dokter specialis Anak', N'', N'', CAST(N'2024-01-19T07:14:39.8051370' AS DateTime2), N'Argi Purwanto', CAST(N'2024-02-02T15:02:06.2173312' AS DateTime2))
INSERT [dbo].[Specialities] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Dokter spesialis Gigi', N'G001', N'', CAST(N'2024-01-19T07:14:52.8465718' AS DateTime2), N'Argi Purwanto', CAST(N'2024-02-02T15:02:46.4925642' AS DateTime2))
INSERT [dbo].[Specialities] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (3, N'Dokter spesialis kulit', N'DK001', N'', CAST(N'2024-01-19T07:26:10.9773326' AS DateTime2), N'Argi Purwanto', CAST(N'2024-02-02T15:02:32.3033925' AS DateTime2))
INSERT [dbo].[Specialities] ([Id], [Name], [Code], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (4, N'Dokter Umum', N'', N'Administrator', CAST(N'2024-03-08T10:29:09.8777162' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Specialities] OFF
GO
SET IDENTITY_INSERT [dbo].[StockOutPrescriptions] ON 

INSERT [dbo].[StockOutPrescriptions] ([Id], [PrescriptionId], [TransactionStockId], [CutStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [StockProductId]) VALUES (20017, 20016, 20092, 5, N'Nurse', CAST(N'2024-08-07T01:22:52.1656114' AS DateTime2), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[StockOutPrescriptions] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemParameters] ON 

INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'baseURL', N'https://apijkn-dev.bpjs-kesehatan.go.id', N'Administrator', CAST(N'2024-06-10T14:02:15.6065184' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'cons-id', N'15793', N'Administrator', CAST(N'2024-06-10T14:02:15.6065972' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'kdAplikasi', N'095', N'Administrator', CAST(N'2024-06-10T14:02:15.6065979' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'password', N'BPJSKes2024**', N'Administrator', CAST(N'2024-06-10T14:02:15.6065982' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'pcare_code_provider', N'00010001', N'Administrator', CAST(N'2024-06-10T14:02:15.6065985' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, N'secret-key', N'8nDF24C2AD', N'Administrator', CAST(N'2024-06-10T14:02:15.6065988' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, N'serviceName', N'pcare-rest-dev', N'Administrator', CAST(N'2024-06-10T14:02:15.6065991' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, N'user-key', N'6825c31715d8d748d5944f13b39ac431', N'Administrator', CAST(N'2024-06-10T14:02:15.6065994' AS DateTime2), NULL, NULL)
INSERT [dbo].[SystemParameters] ([Id], [Key], [Value], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, N'username', N'dermott', N'Administrator', CAST(N'2024-06-10T14:02:15.6065997' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[SystemParameters] OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionStocks] ON 

INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20082, N'ReceivingStock', 10015, 10027, N'RCV#001', N'1122', CAST(N'2024-07-30T00:00:00.0000000' AS DateTime2), 2, 10, 8, 1, NULL, N'Administrator', CAST(N'2024-07-29T17:18:53.0332372' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.6297855' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20083, N'ReceivingStock', 10015, 10027, N'RCV#001', N'1133', CAST(N'2024-08-05T00:00:00.0000000' AS DateTime2), 2, 20, 8, 1, NULL, N'Administrator', CAST(N'2024-07-29T17:18:53.0820225' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.6980856' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20084, N'ReceivingStock', 10015, 10028, N'RCV#001', N'1144', CAST(N'2024-08-30T00:00:00.0000000' AS DateTime2), 2, 5, 8, 1, NULL, N'Administrator', CAST(N'2024-07-29T17:18:53.0924235' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.7093203' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20085, N'ReceivingStock', 10015, 10030, N'RCV#001', NULL, NULL, 2, 20, 1, 1, NULL, N'Administrator', CAST(N'2024-07-29T17:18:53.0986806' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.7187779' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20086, N'ReceivingStock', 20016, 10029, N'RCV#002', N'AMK002', CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2), 3, 50, 1, 1, NULL, N'Administrator', CAST(N'2024-08-07T00:38:30.1997244' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:38:35.3994659' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20087, N'ReceivingStock', 20016, 10030, N'RCV#002', NULL, NULL, 3, 70, 1, 1, NULL, N'Administrator', CAST(N'2024-08-07T00:38:30.2490338' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:38:35.4686039' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20088, N'TransferStock', 20020, 10029, N'ITR#001', N'AMK002', CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2), 3, -25, 1, 1, NULL, N'Administrator', CAST(N'2024-08-07T00:39:34.4582724' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:39:36.2608797' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20089, N'TransferStock', 20020, 10029, N'ITR#001', N'AMK002', CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2), 4, 25, 1, 1, NULL, N'Administrator', CAST(N'2024-08-07T00:39:34.4765672' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:39:36.3097279' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20090, N'TransferStock', 20020, 10030, N'ITR#001', NULL, NULL, 3, -20, 1, 1, NULL, N'Administrator', CAST(N'2024-08-07T00:39:34.4920127' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:39:36.3349303' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20091, N'TransferStock', 20020, 10030, N'ITR#001', NULL, NULL, 4, 20, 1, 1, NULL, N'Administrator', CAST(N'2024-08-07T00:39:34.5029945' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:39:36.3534031' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20092, N'Prescription', 20016, 10029, N'PHP#001', N'AMK002', CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2), 3, -5, 1, 1, NULL, N'Nurse', CAST(N'2024-08-07T01:22:51.2672996' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.5133844' AS DateTime2))
INSERT [dbo].[TransactionStocks] ([Id], [SourceTable], [SourcTableId], [ProductId], [Reference], [Batch], [ExpiredDate], [LocationId], [Quantity], [UomId], [Validate], [InventoryAdjusmentId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20093, N'ConcoctionLine', 20006, 10030, N'PHCL#001', NULL, NULL, 3, -10, 1, 1, NULL, N'Nurse', CAST(N'2024-08-07T01:23:03.4999501' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.6112769' AS DateTime2))
SET IDENTITY_INSERT [dbo].[TransactionStocks] OFF
GO
SET IDENTITY_INSERT [dbo].[TransferStockLogs] ON 

INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10052, NULL, 2, 3, N'Draft', N'Administrator', CAST(N'2024-07-29T17:39:13.8717368' AS DateTime2), NULL, NULL)
INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10053, NULL, 2, 3, N'Request', N'Administrator', CAST(N'2024-07-29T17:42:42.3699638' AS DateTime2), NULL, NULL)
INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10054, NULL, 2, 3, N'Cancel', N'Nurse', CAST(N'2024-07-30T10:47:30.9714696' AS DateTime2), NULL, NULL)
INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10055, NULL, 2, 3, N'Draft', N'Administrator', CAST(N'2024-07-30T11:17:42.2864995' AS DateTime2), NULL, NULL)
INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20054, 20020, 3, 4, N'Draft', N'Administrator', CAST(N'2024-08-07T00:39:11.0108497' AS DateTime2), NULL, NULL)
INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20055, 20020, 3, 4, N'Ready', N'Administrator', CAST(N'2024-08-07T00:39:34.5443974' AS DateTime2), NULL, NULL)
INSERT [dbo].[TransferStockLogs] ([Id], [TransferStockId], [SourceId], [DestinationId], [Status], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20056, 20020, 3, 4, N'Done', N'Administrator', CAST(N'2024-08-07T00:39:36.3849280' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[TransferStockLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[TransferStockProduct] ON 

INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10028, NULL, NULL, 10030, 5, N'Administrator', CAST(N'2024-07-29T17:39:13.7009132' AS DateTime2), N'Administrator', CAST(N'2024-07-30T10:46:32.7607979' AS DateTime2), NULL)
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10029, N'1144', NULL, 10028, 5, N'Administrator', CAST(N'2024-07-29T17:39:13.7009163' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:40:20.4469313' AS DateTime2), CAST(N'2024-08-30T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10030, N'1133', NULL, 10027, 5, N'Administrator', CAST(N'2024-07-29T17:39:13.7009172' AS DateTime2), N'Administrator', CAST(N'2024-07-30T10:46:09.7516743' AS DateTime2), CAST(N'2024-08-05T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10031, NULL, NULL, 10027, 5, N'Administrator', CAST(N'2024-07-29T17:39:13.7009179' AS DateTime2), N'Administrator', CAST(N'2024-07-30T10:47:13.6160708' AS DateTime2), NULL)
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10032, N'1133', NULL, 10027, 5, N'Administrator', CAST(N'2024-07-30T11:17:42.1491193' AS DateTime2), NULL, NULL, CAST(N'2024-08-05T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10033, NULL, NULL, 10030, 5, N'Administrator', CAST(N'2024-07-30T11:17:42.1491214' AS DateTime2), NULL, NULL, NULL)
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10034, N'1144', NULL, 10028, 5, N'Administrator', CAST(N'2024-07-30T11:17:42.1491217' AS DateTime2), NULL, NULL, CAST(N'2024-08-30T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (10035, N'1122', NULL, 10027, 5, N'Administrator', CAST(N'2024-07-30T11:17:42.1491219' AS DateTime2), NULL, NULL, CAST(N'2024-07-30T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (20032, N'AMK002', 20020, 10029, 25, N'Administrator', CAST(N'2024-08-07T00:39:10.8854448' AS DateTime2), NULL, NULL, CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[TransferStockProduct] ([Id], [Batch], [TransferStockId], [ProductId], [QtyStock], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ExpiredDate]) VALUES (20033, NULL, 20020, 10030, 20, N'Administrator', CAST(N'2024-08-07T00:39:25.3282678' AS DateTime2), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TransferStockProduct] OFF
GO
SET IDENTITY_INSERT [dbo].[TransferStocks] ON 

INSERT [dbo].[TransferStocks] ([Id], [SourceId], [DestinationId], [SchenduleDate], [KodeTransaksi], [Status], [Reference], [StockRequest], [StockProductId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20020, 3, 4, CAST(N'2024-08-07T00:38:47.5874095' AS DateTime2), N'Pharmacy/INT/00001', 5, NULL, 0, NULL, N'Administrator', CAST(N'2024-08-07T00:39:10.7357157' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:39:36.3756294' AS DateTime2))
SET IDENTITY_INSERT [dbo].[TransferStocks] OFF
GO
SET IDENTITY_INSERT [dbo].[UomCategories] ON 

INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Tablet', N'Default Unit', N'Administrator', CAST(N'2024-07-18T10:32:49.2184525' AS DateTime2), N'Administrator', CAST(N'2024-07-18T10:33:00.1915709' AS DateTime2))
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Cream', N'Default Volume', N'Administrator', CAST(N'2024-07-18T10:33:09.0013637' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Unit', NULL, N'Administrator', CAST(N'2024-04-02T11:51:42.4931517' AS DateTime2), N'Administrator', CAST(N'2024-04-02T13:35:55.5662503' AS DateTime2))
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'Weight', N'Default Weigth', N'Administrator', CAST(N'2024-04-02T13:36:16.6058560' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (11, N'Working Time', N'Default Working Time ', N'Administrator', CAST(N'2024-04-02T13:36:30.4753692' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'Length', N'Default Length ', N'Administrator', CAST(N'2024-04-02T13:36:41.0147572' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (13, N'Volume', N'Default Volume', N'Administrator', CAST(N'2024-04-02T13:36:58.1566343' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (14, N'Pack', NULL, N'Administrator', CAST(N'2024-04-02T13:37:33.2302225' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (15, N'Pcs', NULL, N'Administrator', CAST(N'2024-04-02T13:37:38.3949421' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (16, N'Tablet', NULL, N'Administrator', CAST(N'2024-04-02T13:38:01.7567780' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (17, N'Syrup', NULL, N'Administrator', CAST(N'2024-04-02T13:38:08.8608888' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (18, N'Roll', NULL, N'Administrator', CAST(N'2024-04-02T13:38:25.7531500' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (19, N'Strip', NULL, N'Administrator', CAST(N'2024-04-02T13:38:34.6525118' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (20, N'Lusin', NULL, N'Administrator', CAST(N'2024-04-02T13:38:49.0012678' AS DateTime2), NULL, NULL)
INSERT [dbo].[UomCategories] ([Id], [Name], [Type], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (21, N'Satuan', NULL, N'Administrator', CAST(N'2024-04-24T15:22:13.0005040' AS DateTime2), NULL, NULL)
SET IDENTITY_INSERT [dbo].[UomCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Uoms] ON 

INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, N'Unit', 1, N'Bigger than the reference Unit of Measure', 1, 1, 0.01, NULL, CAST(N'2024-08-06T18:23:09.8020742' AS DateTime2), N'Nurse', CAST(N'2024-08-07T01:23:23.6112807' AS DateTime2))
INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (2, N'Pasta', 2, N'Bigger than the reference Unit of Measure', 1, 1, 0.01, N'Administrator', CAST(N'2024-07-18T10:33:33.2463910' AS DateTime2), N'Administrator', CAST(N'2024-07-22T22:40:07.0392503' AS DateTime2))
INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (7, N'Ampul ( 1 )', 9, N'Bigger than the reference Unit of Measure', 1, 1, 0.0001, NULL, CAST(N'2024-07-01T04:24:12.7037552' AS DateTime2), N'Administrator', CAST(N'2024-07-18T09:35:09.9852691' AS DateTime2))
INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (8, N'Ampul ( 5 )', 9, N'Bigger than the reference Unit of Measure', 1, 1, NULL, N'Administrator', CAST(N'2024-04-02T13:43:30.9128897' AS DateTime2), N'Administrator', CAST(N'2024-07-29T17:19:04.7093234' AS DateTime2))
INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (9, N'Tablet ( 1 )', 9, N'Bigger than the reference Unit of Measure', 1, 1, 0.01, NULL, CAST(N'2024-05-28T04:53:51.9201506' AS DateTime2), N'Administrator', CAST(N'2024-05-28T11:53:51.9352801' AS DateTime2))
INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (10, N'Botol( 10 )', 9, N'Bigger than the reference Unit of Measure', 10, 1, 0.01, N'Administrator', CAST(N'2024-04-02T13:46:05.0808134' AS DateTime2), N'Administrator', CAST(N'2024-04-29T11:35:17.4124257' AS DateTime2))
INSERT [dbo].[Uoms] ([Id], [Name], [UomCategoryId], [Type], [BiggerRatio], [Active], [RoundingPrecision], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (12, N'MG (1)', 21, N'Bigger than the reference Unit of Measure', 1, 1, 0.01, NULL, CAST(N'2024-05-14T04:01:37.1729113' AS DateTime2), N'Nurse', CAST(N'2024-05-14T11:01:37.1802446' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Uoms] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (2, 33, N'Administrator', N'admin@mail.com', N'admin@mail.com', N'202cb962ac59075b964b07152d234b70', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'12345', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'15-07-2024-0025', NULL, NULL, NULL, 1, 1, 1, 1, 1, 0, 0, 1, 1, N'', 1, NULL, NULL, NULL, N'ADMIN', N'ADMIN', N'ADMIN', N'ADMIN', NULL, NULL, NULL, NULL, NULL, NULL, N'Administrator', CAST(N'2024-07-15T10:41:20.1517634' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (47, 36, N'Nurse', N'nurse@mail.com', N'nurse@mail.com', N'202cb962ac59075b964b07152d234b70', 2, NULL, NULL, NULL, N'KTP', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'0813831821999', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'-', NULL, NULL, NULL, 0, 0, 1, 1, 0, 1, 1, 0, 0, N'', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'[]', NULL, NULL, NULL, N'Administrator', CAST(N'2024-04-24T10:14:18.5989837' AS DateTime2), N'Administrator', CAST(N'2024-07-15T10:29:09.3311927' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (48, 33, N'Dr. Adi', N'adi@mail.com', N'adi@mail.com', N'202cb962ac59075b964b07152d234b70', NULL, NULL, NULL, NULL, N'KTP', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'081383182119', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'-', NULL, NULL, NULL, 0, 0, 1, 1, 1, 0, 0, 0, 0, N'440939', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'[16]', NULL, NULL, NULL, NULL, CAST(N'2024-08-06T16:46:36.9101281' AS DateTime2), NULL, CAST(N'2024-08-06T23:46:37.0133650' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (64, NULL, N'Dr Julia', N'julia@mail.com', N'julia@mail.com', N'202cb962ac59075b964b07152d234b70', NULL, NULL, NULL, NULL, N'KTP', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'08128329732932', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'-', NULL, NULL, NULL, 0, 0, 0, 1, 1, 0, 0, 0, 0, N'448251', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'[18]', NULL, NULL, NULL, N'Nurse', CAST(N'2024-05-16T16:49:12.0078745' AS DateTime2), N'Administrator', CAST(N'2024-05-27T16:08:22.1724633' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (69, 33, N'Mcu', N'mcu@mail.com', N'mcu@mail.com', N'202cb962ac59075b964b07152d234b70', NULL, NULL, NULL, NULL, N'KTP', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'1233214234', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'-', NULL, NULL, NULL, 0, 0, 1, 0, 0, 0, 0, 1, 0, N'', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'[]', N'[]', NULL, NULL, N'Administrator', CAST(N'2024-07-11T14:41:04.4430027' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (70, 33, N'HR', N'hr@mail.com', N'hr@mail.com', N'202cb962ac59075b964b07152d234b70', NULL, NULL, NULL, NULL, N'KTP', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'3298408335', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'-', NULL, NULL, NULL, 0, 0, 1, 0, 0, 0, 0, 0, 1, N'', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'[]', N'[]', NULL, NULL, N'Administrator', CAST(N'2024-07-11T14:41:37.0959588' AS DateTime2), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (72, NULL, N'Argi Purwanto', N'argipurwanto0@gmail.com', N'argipurwanto0@gmail.com', N'', 1, N'Single', NULL, CAST(N'2003-09-14T00:00:00.0000000' AS DateTime2), N'KTP', N'0010000010000', NULL, N'Poris', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Poris', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'08002322222', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, 1, NULL, NULL, NULL, NULL, NULL, N'07-08-2024-0030', NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, 0, 0, 0, N'', 0, NULL, NULL, NULL, N'0300', N'0100', N'0200', N'0400', N'[]', N'[1]', 74, NULL, N'Administrator', CAST(N'2024-08-07T00:27:13.0060131' AS DateTime2), N'Administrator', CAST(N'2024-08-07T02:29:38.1046723' AS DateTime2), NULL, N'08002322221', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (73, NULL, N'Nur Ali Majid', N'nuralimajids@gmail.com', N'nuralimajids@gmail.com', N'', 1, N'Single', NULL, CAST(N'2024-04-25T00:00:00.0000000' AS DateTime2), N'KTP', N'00200000000000', NULL, N'Mampang', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Mampang', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'08220000022', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, NULL, NULL, NULL, NULL, NULL, NULL, N'07-08-2024-0030', NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, 0, 0, 0, N'', 0, NULL, NULL, NULL, N'0301', N'0101', N'0201', N'0401', N'[]', N'[]', 74, NULL, N'Administrator', CAST(N'2024-08-07T00:29:15.6243366' AS DateTime2), N'Administrator', CAST(N'2024-08-08T11:34:32.9734421' AS DateTime2), NULL, N'08220000023', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Users] ([Id], [GroupId], [Name], [UserName], [Email], [Password], [GenderId], [MartialStatus], [PlaceOfBirth], [DateOfBirth], [TypeId], [NoId], [ExpiredId], [IdCardAddress1], [IdCardAddress2], [IdCardCountryId], [IdCardProvinceId], [IdCardCityId], [IdCardDistrictId], [IdCardVillageId], [IdCardRtRw], [IdCardZip], [DomicileAddress1], [DomicileAddress2], [DomicileCountryId], [DomicileProvinceId], [DomicileCityId], [DomicileDistrictId], [DomicileVillageId], [DomicileRtRw], [DomicileZip], [BiologicalMother], [MotherNIK], [ReligionId], [MobilePhone], [HomePhoneNumber], [Npwp], [NoBpjsKs], [NoBpjsTk], [SipNo], [SipFile], [SipExp], [StrNo], [StrFile], [StrExp], [SpecialityId], [UserPhoto], [JobPositionId], [DepartmentId], [EmergencyName], [EmergencyRelation], [EmergencyEmail], [EmergencyPhone], [BloodType], [NoRm], [DoctorCode], [EmployeeCode], [DegreeId], [IsEmployee], [IsPatient], [IsUser], [IsDoctor], [IsPhysicion], [IsNurse], [IsPharmacy], [IsMcu], [IsHr], [PhysicanCode], [IsEmployeeRelation], [EmployeeType], [EmployeeStatus], [JoinDate], [NIP], [Legacy], [SAP], [Oracle], [DoctorServiceIds], [PatientAllergyIds], [SupervisorId], [EmailTemplateId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [OccupationalId], [CurrentMobile], [FamilyMedicalHistory], [FamilyMedicalHistoryOther], [IsFamilyMedicalHistory], [IsMedicationHistory], [MedicationHistory], [PastMedicalHistory]) VALUES (74, NULL, N'Wahyu Sanaki', N'fathulazharis@gmail.com', N'fathulazharis@gmail.com', N'', 1, N'Married', NULL, CAST(N'1994-06-08T00:00:00.0000000' AS DateTime2), N'KTP', N'011927777722221231', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'08923443562', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL, N'07-08-2024-0029', NULL, NULL, NULL, 1, 1, 0, 0, 0, 0, 0, 0, 0, N'', 0, NULL, NULL, NULL, N'1301', N'1101', N'1201', N'1401', N'[]', N'[]', NULL, NULL, N'Administrator', CAST(N'2024-08-07T00:31:35.6509506' AS DateTime2), N'Administrator', CAST(N'2024-08-07T00:31:56.0754857' AS DateTime2), NULL, N'089234435621', NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[InventoryAdjusments] ADD  DEFAULT (N'') FOR [Reference]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsOralMedication]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsTopicalMedication]
GO
ALTER TABLE [dbo].[Accidents]  WITH CHECK ADD  CONSTRAINT [FK_Accidents_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Accidents] CHECK CONSTRAINT [FK_Accidents_Departments_DepartmentId]
GO
ALTER TABLE [dbo].[Accidents]  WITH CHECK ADD  CONSTRAINT [FK_Accidents_GeneralConsultanServices_GeneralConsultanServiceId] FOREIGN KEY([GeneralConsultanServiceId])
REFERENCES [dbo].[GeneralConsultanServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accidents] CHECK CONSTRAINT [FK_Accidents_GeneralConsultanServices_GeneralConsultanServiceId]
GO
ALTER TABLE [dbo].[Accidents]  WITH CHECK ADD  CONSTRAINT [FK_Accidents_Users_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Accidents] CHECK CONSTRAINT [FK_Accidents_Users_EmployeeId]
GO
ALTER TABLE [dbo].[Accidents]  WITH CHECK ADD  CONSTRAINT [FK_Accidents_Users_SafetyPersonnelId] FOREIGN KEY([SafetyPersonnelId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Accidents] CHECK CONSTRAINT [FK_Accidents_Users_SafetyPersonnelId]
GO
ALTER TABLE [dbo].[ActiveComponentMedicamentGroupDetail]  WITH CHECK ADD  CONSTRAINT [FK_ActiveComponentMedicamentGroupDetail_ActiveComponents_ActiveComponentId] FOREIGN KEY([ActiveComponentId])
REFERENCES [dbo].[ActiveComponents] ([Id])
GO
ALTER TABLE [dbo].[ActiveComponentMedicamentGroupDetail] CHECK CONSTRAINT [FK_ActiveComponentMedicamentGroupDetail_ActiveComponents_ActiveComponentId]
GO
ALTER TABLE [dbo].[ActiveComponentMedicamentGroupDetail]  WITH CHECK ADD  CONSTRAINT [FK_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetails_MedicamentGroupDetailsId] FOREIGN KEY([MedicamentGroupDetailsId])
REFERENCES [dbo].[MedicamentGroupDetails] ([Id])
GO
ALTER TABLE [dbo].[ActiveComponentMedicamentGroupDetail] CHECK CONSTRAINT [FK_ActiveComponentMedicamentGroupDetail_MedicamentGroupDetails_MedicamentGroupDetailsId]
GO
ALTER TABLE [dbo].[ActiveComponents]  WITH CHECK ADD  CONSTRAINT [FK_ActiveComponents_ConcoctionLines_ConcoctionLineId] FOREIGN KEY([ConcoctionLineId])
REFERENCES [dbo].[ConcoctionLines] ([Id])
GO
ALTER TABLE [dbo].[ActiveComponents] CHECK CONSTRAINT [FK_ActiveComponents_ConcoctionLines_ConcoctionLineId]
GO
ALTER TABLE [dbo].[ActiveComponents]  WITH CHECK ADD  CONSTRAINT [FK_ActiveComponents_Medicaments_MedicamentId] FOREIGN KEY([MedicamentId])
REFERENCES [dbo].[Medicaments] ([Id])
GO
ALTER TABLE [dbo].[ActiveComponents] CHECK CONSTRAINT [FK_ActiveComponents_Medicaments_MedicamentId]
GO
ALTER TABLE [dbo].[ActiveComponents]  WITH CHECK ADD  CONSTRAINT [FK_ActiveComponents_Prescriptions_PrescriptionId] FOREIGN KEY([PrescriptionId])
REFERENCES [dbo].[Prescriptions] ([Id])
GO
ALTER TABLE [dbo].[ActiveComponents] CHECK CONSTRAINT [FK_ActiveComponents_Prescriptions_PrescriptionId]
GO
ALTER TABLE [dbo].[ActiveComponents]  WITH CHECK ADD  CONSTRAINT [FK_ActiveComponents_Uoms_UomId] FOREIGN KEY([UomId])
REFERENCES [dbo].[Uoms] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ActiveComponents] CHECK CONSTRAINT [FK_ActiveComponents_Uoms_UomId]
GO
ALTER TABLE [dbo].[BPJSIntegrations]  WITH CHECK ADD  CONSTRAINT [FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId] FOREIGN KEY([InsurancePolicyId])
REFERENCES [dbo].[InsurancePolicies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BPJSIntegrations] CHECK CONSTRAINT [FK_BPJSIntegrations_InsurancePolicies_InsurancePolicyId]
GO
ALTER TABLE [dbo].[BuildingLocations]  WITH CHECK ADD  CONSTRAINT [FK_BuildingLocations_Buildings_BuildingId] FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([Id])
GO
ALTER TABLE [dbo].[BuildingLocations] CHECK CONSTRAINT [FK_BuildingLocations_Buildings_BuildingId]
GO
ALTER TABLE [dbo].[BuildingLocations]  WITH CHECK ADD  CONSTRAINT [FK_BuildingLocations_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[BuildingLocations] CHECK CONSTRAINT [FK_BuildingLocations_Locations_LocationId]
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD  CONSTRAINT [FK_Buildings_HealthCenters_HealthCenterId] FOREIGN KEY([HealthCenterId])
REFERENCES [dbo].[HealthCenters] ([Id])
GO
ALTER TABLE [dbo].[Buildings] CHECK CONSTRAINT [FK_Buildings_HealthCenters_HealthCenterId]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_Provinces_ProvinceId] FOREIGN KEY([ProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_Provinces_ProvinceId]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Cities_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Cities_CityId]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Countries_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Countries_CountryId]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Provinces_ProvinceId] FOREIGN KEY([ProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Provinces_ProvinceId]
GO
ALTER TABLE [dbo].[ConcoctionLines]  WITH CHECK ADD  CONSTRAINT [FK_ConcoctionLines_Concoctions_ConcoctionId] FOREIGN KEY([ConcoctionId])
REFERENCES [dbo].[Concoctions] ([Id])
GO
ALTER TABLE [dbo].[ConcoctionLines] CHECK CONSTRAINT [FK_ConcoctionLines_Concoctions_ConcoctionId]
GO
ALTER TABLE [dbo].[ConcoctionLines]  WITH CHECK ADD  CONSTRAINT [FK_ConcoctionLines_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[ConcoctionLines] CHECK CONSTRAINT [FK_ConcoctionLines_Products_ProductId]
GO
ALTER TABLE [dbo].[ConcoctionLines]  WITH CHECK ADD  CONSTRAINT [FK_ConcoctionLines_Uoms_UomId] FOREIGN KEY([UomId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[ConcoctionLines] CHECK CONSTRAINT [FK_ConcoctionLines_Uoms_UomId]
GO
ALTER TABLE [dbo].[Concoctions]  WITH CHECK ADD  CONSTRAINT [FK_Concoctions_DrugDosages_DrugDosageId] FOREIGN KEY([DrugDosageId])
REFERENCES [dbo].[DrugDosages] ([Id])
GO
ALTER TABLE [dbo].[Concoctions] CHECK CONSTRAINT [FK_Concoctions_DrugDosages_DrugDosageId]
GO
ALTER TABLE [dbo].[Concoctions]  WITH CHECK ADD  CONSTRAINT [FK_Concoctions_DrugRoutes_DrugRouteId] FOREIGN KEY([DrugRouteId])
REFERENCES [dbo].[DrugRoutes] ([Id])
GO
ALTER TABLE [dbo].[Concoctions] CHECK CONSTRAINT [FK_Concoctions_DrugRoutes_DrugRouteId]
GO
ALTER TABLE [dbo].[Concoctions]  WITH CHECK ADD  CONSTRAINT [FK_Concoctions_FormDrugs_DrugFormId] FOREIGN KEY([DrugFormId])
REFERENCES [dbo].[FormDrugs] ([Id])
GO
ALTER TABLE [dbo].[Concoctions] CHECK CONSTRAINT [FK_Concoctions_FormDrugs_DrugFormId]
GO
ALTER TABLE [dbo].[Concoctions]  WITH CHECK ADD  CONSTRAINT [FK_Concoctions_MedicamentGroups_MedicamentGroupId] FOREIGN KEY([MedicamentGroupId])
REFERENCES [dbo].[MedicamentGroups] ([Id])
GO
ALTER TABLE [dbo].[Concoctions] CHECK CONSTRAINT [FK_Concoctions_MedicamentGroups_MedicamentGroupId]
GO
ALTER TABLE [dbo].[Concoctions]  WITH CHECK ADD  CONSTRAINT [FK_Concoctions_Pharmacies_PharmacyId] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacies] ([Id])
GO
ALTER TABLE [dbo].[Concoctions] CHECK CONSTRAINT [FK_Concoctions_Pharmacies_PharmacyId]
GO
ALTER TABLE [dbo].[Concoctions]  WITH CHECK ADD  CONSTRAINT [FK_Concoctions_Users_PractitionerId] FOREIGN KEY([PractitionerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Concoctions] CHECK CONSTRAINT [FK_Concoctions_Users_PractitionerId]
GO
ALTER TABLE [dbo].[Counters]  WITH CHECK ADD  CONSTRAINT [FK_Counters_QueueDisplays_QueueDisplayId] FOREIGN KEY([QueueDisplayId])
REFERENCES [dbo].[QueueDisplays] ([Id])
GO
ALTER TABLE [dbo].[Counters] CHECK CONSTRAINT [FK_Counters_QueueDisplays_QueueDisplayId]
GO
ALTER TABLE [dbo].[Counters]  WITH CHECK ADD  CONSTRAINT [FK_Counters_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[Counters] CHECK CONSTRAINT [FK_Counters_Services_ServiceId]
GO
ALTER TABLE [dbo].[Counters]  WITH CHECK ADD  CONSTRAINT [FK_Counters_Services_ServiceKId] FOREIGN KEY([ServiceKId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[Counters] CHECK CONSTRAINT [FK_Counters_Services_ServiceKId]
GO
ALTER TABLE [dbo].[Counters]  WITH CHECK ADD  CONSTRAINT [FK_Counters_Users_PhysicianId] FOREIGN KEY([PhysicianId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Counters] CHECK CONSTRAINT [FK_Counters_Users_PhysicianId]
GO
ALTER TABLE [dbo].[Departments]  WITH CHECK ADD  CONSTRAINT [FK_Departments_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Departments] CHECK CONSTRAINT [FK_Departments_Companies_CompanyId]
GO
ALTER TABLE [dbo].[Departments]  WITH CHECK ADD  CONSTRAINT [FK_Departments_Departments_ParentDepartmentId] FOREIGN KEY([ParentDepartmentId])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Departments] CHECK CONSTRAINT [FK_Departments_Departments_ParentDepartmentId]
GO
ALTER TABLE [dbo].[Departments]  WITH CHECK ADD  CONSTRAINT [FK_Departments_Users_ManagerId] FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Departments] CHECK CONSTRAINT [FK_Departments_Users_ManagerId]
GO
ALTER TABLE [dbo].[Diagnoses]  WITH CHECK ADD  CONSTRAINT [FK_Diagnoses_CronisCategories_CronisCategoryId] FOREIGN KEY([CronisCategoryId])
REFERENCES [dbo].[CronisCategories] ([Id])
GO
ALTER TABLE [dbo].[Diagnoses] CHECK CONSTRAINT [FK_Diagnoses_CronisCategories_CronisCategoryId]
GO
ALTER TABLE [dbo].[Diagnoses]  WITH CHECK ADD  CONSTRAINT [FK_Diagnoses_DiseaseCategories_DiseaseCategoryId] FOREIGN KEY([DiseaseCategoryId])
REFERENCES [dbo].[DiseaseCategories] ([Id])
GO
ALTER TABLE [dbo].[Diagnoses] CHECK CONSTRAINT [FK_Diagnoses_DiseaseCategories_DiseaseCategoryId]
GO
ALTER TABLE [dbo].[Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_Cities_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Districts] CHECK CONSTRAINT [FK_Districts_Cities_CityId]
GO
ALTER TABLE [dbo].[Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_Provinces_ProvinceId] FOREIGN KEY([ProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[Districts] CHECK CONSTRAINT [FK_Districts_Provinces_ProvinceId]
GO
ALTER TABLE [dbo].[DoctorScheduleDetails]  WITH CHECK ADD  CONSTRAINT [FK_DoctorScheduleDetails_DoctorSchedules_DoctorScheduleId] FOREIGN KEY([DoctorScheduleId])
REFERENCES [dbo].[DoctorSchedules] ([Id])
GO
ALTER TABLE [dbo].[DoctorScheduleDetails] CHECK CONSTRAINT [FK_DoctorScheduleDetails_DoctorSchedules_DoctorScheduleId]
GO
ALTER TABLE [dbo].[DoctorSchedules]  WITH CHECK ADD  CONSTRAINT [FK_DoctorSchedules_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[DoctorSchedules] CHECK CONSTRAINT [FK_DoctorSchedules_Services_ServiceId]
GO
ALTER TABLE [dbo].[DoctorScheduleSlots]  WITH CHECK ADD  CONSTRAINT [FK_DoctorScheduleSlots_DoctorSchedules_DoctorScheduleId] FOREIGN KEY([DoctorScheduleId])
REFERENCES [dbo].[DoctorSchedules] ([Id])
GO
ALTER TABLE [dbo].[DoctorScheduleSlots] CHECK CONSTRAINT [FK_DoctorScheduleSlots_DoctorSchedules_DoctorScheduleId]
GO
ALTER TABLE [dbo].[DoctorScheduleSlots]  WITH CHECK ADD  CONSTRAINT [FK_DoctorScheduleSlots_Users_PhysicianId] FOREIGN KEY([PhysicianId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[DoctorScheduleSlots] CHECK CONSTRAINT [FK_DoctorScheduleSlots_Users_PhysicianId]
GO
ALTER TABLE [dbo].[DrugDosages]  WITH CHECK ADD  CONSTRAINT [FK_DrugDosages_DrugRoutes_DrugRouteId] FOREIGN KEY([DrugRouteId])
REFERENCES [dbo].[DrugRoutes] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[DrugDosages] CHECK CONSTRAINT [FK_DrugDosages_DrugRoutes_DrugRouteId]
GO
ALTER TABLE [dbo].[EmailTemplates]  WITH CHECK ADD  CONSTRAINT [FK_EmailTemplates_EmailSettings_EmailFromId] FOREIGN KEY([EmailFromId])
REFERENCES [dbo].[EmailSettings] ([Id])
GO
ALTER TABLE [dbo].[EmailTemplates] CHECK CONSTRAINT [FK_EmailTemplates_EmailSettings_EmailFromId]
GO
ALTER TABLE [dbo].[EmailTemplates]  WITH CHECK ADD  CONSTRAINT [FK_EmailTemplates_Users_ById] FOREIGN KEY([ById])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[EmailTemplates] CHECK CONSTRAINT [FK_EmailTemplates_Users_ById]
GO
ALTER TABLE [dbo].[GeneralConsultanCPPTs]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId] FOREIGN KEY([GeneralConsultanServiceId])
REFERENCES [dbo].[GeneralConsultanServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GeneralConsultanCPPTs] CHECK CONSTRAINT [FK_GeneralConsultanCPPTs_GeneralConsultanServices_GeneralConsultanServiceId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId] FOREIGN KEY([GeneralConsultanServiceId])
REFERENCES [dbo].[GeneralConsultanServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_GeneralConsultanServices_GeneralConsultanServiceId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_LabTestDetails_LabResulLabExaminationtId] FOREIGN KEY([LabResulLabExaminationtId])
REFERENCES [dbo].[LabTestDetails] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_LabTestDetails_LabResulLabExaminationtId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_LabTests_LabTestId] FOREIGN KEY([LabTestId])
REFERENCES [dbo].[LabTests] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_LabTests_LabTestId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_EmployeeId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerAlcoholEximinationId] FOREIGN KEY([PractitionerAlcoholEximinationId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerAlcoholEximinationId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerDrugEximinationId] FOREIGN KEY([PractitionerDrugEximinationId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerDrugEximinationId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerECGId] FOREIGN KEY([PractitionerECGId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerECGId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerLabEximinationId] FOREIGN KEY([PractitionerLabEximinationId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerLabEximinationId]
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerRadiologyEximinationId] FOREIGN KEY([PractitionerRadiologyEximinationId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanMedicalSupports] CHECK CONSTRAINT [FK_GeneralConsultanMedicalSupports_Users_PractitionerRadiologyEximinationId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_Awarenesses_AwarenessId] FOREIGN KEY([AwarenessId])
REFERENCES [dbo].[Awarenesses] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_Awarenesses_AwarenessId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_ClassTypes_ClassTypeId] FOREIGN KEY([ClassTypeId])
REFERENCES [dbo].[ClassTypes] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_ClassTypes_ClassTypeId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_InsurancePolicies_InsurancePolicyId] FOREIGN KEY([InsurancePolicyId])
REFERENCES [dbo].[InsurancePolicies] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_InsurancePolicies_InsurancePolicyId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_KioskQueues_KioskQueueId] FOREIGN KEY([KioskQueueId])
REFERENCES [dbo].[KioskQueues] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_KioskQueues_KioskQueueId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_Projects_ProjectId] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_Projects_ProjectId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_Services_ServiceId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_Users_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_Users_PatientId]
GO
ALTER TABLE [dbo].[GeneralConsultanServices]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultanServices_Users_PratitionerId] FOREIGN KEY([PratitionerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultanServices] CHECK CONSTRAINT [FK_GeneralConsultanServices_Users_PratitionerId]
GO
ALTER TABLE [dbo].[GeneralConsultantClinicalAssesments]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultantClinicalAssesments_Awarenesses_AwarenessId] FOREIGN KEY([AwarenessId])
REFERENCES [dbo].[Awarenesses] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[GeneralConsultantClinicalAssesments] CHECK CONSTRAINT [FK_GeneralConsultantClinicalAssesments_Awarenesses_AwarenessId]
GO
ALTER TABLE [dbo].[GeneralConsultantClinicalAssesments]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId] FOREIGN KEY([GeneralConsultanServiceId])
REFERENCES [dbo].[GeneralConsultanServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GeneralConsultantClinicalAssesments] CHECK CONSTRAINT [FK_GeneralConsultantClinicalAssesments_GeneralConsultanServices_GeneralConsultanServiceId]
GO
ALTER TABLE [dbo].[GeneralConsultationLogs]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultationLogs_GeneralConsultanMedicalSupports_ProcedureRoomId] FOREIGN KEY([ProcedureRoomId])
REFERENCES [dbo].[GeneralConsultanMedicalSupports] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultationLogs] CHECK CONSTRAINT [FK_GeneralConsultationLogs_GeneralConsultanMedicalSupports_ProcedureRoomId]
GO
ALTER TABLE [dbo].[GeneralConsultationLogs]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId] FOREIGN KEY([GeneralConsultanServiceId])
REFERENCES [dbo].[GeneralConsultanServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GeneralConsultationLogs] CHECK CONSTRAINT [FK_GeneralConsultationLogs_GeneralConsultanServices_GeneralConsultanServiceId]
GO
ALTER TABLE [dbo].[GeneralConsultationLogs]  WITH CHECK ADD  CONSTRAINT [FK_GeneralConsultationLogs_Users_UserById] FOREIGN KEY([UserById])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[GeneralConsultationLogs] CHECK CONSTRAINT [FK_GeneralConsultationLogs_Users_UserById]
GO
ALTER TABLE [dbo].[GroupMenus]  WITH CHECK ADD  CONSTRAINT [FK_GroupMenus_Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMenus] CHECK CONSTRAINT [FK_GroupMenus_Groups_GroupId]
GO
ALTER TABLE [dbo].[GroupMenus]  WITH CHECK ADD  CONSTRAINT [FK_GroupMenus_Menus_MenuId] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMenus] CHECK CONSTRAINT [FK_GroupMenus_Menus_MenuId]
GO
ALTER TABLE [dbo].[HealthCenters]  WITH CHECK ADD  CONSTRAINT [FK_HealthCenters_Cities_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[HealthCenters] CHECK CONSTRAINT [FK_HealthCenters_Cities_CityId]
GO
ALTER TABLE [dbo].[HealthCenters]  WITH CHECK ADD  CONSTRAINT [FK_HealthCenters_Countries_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[HealthCenters] CHECK CONSTRAINT [FK_HealthCenters_Countries_CountryId]
GO
ALTER TABLE [dbo].[HealthCenters]  WITH CHECK ADD  CONSTRAINT [FK_HealthCenters_Provinces_ProvinceId] FOREIGN KEY([ProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[HealthCenters] CHECK CONSTRAINT [FK_HealthCenters_Provinces_ProvinceId]
GO
ALTER TABLE [dbo].[InsurancePolicies]  WITH CHECK ADD  CONSTRAINT [FK_InsurancePolicies_Insurances_InsuranceId] FOREIGN KEY([InsuranceId])
REFERENCES [dbo].[Insurances] ([Id])
GO
ALTER TABLE [dbo].[InsurancePolicies] CHECK CONSTRAINT [FK_InsurancePolicies_Insurances_InsuranceId]
GO
ALTER TABLE [dbo].[InsurancePolicies]  WITH CHECK ADD  CONSTRAINT [FK_InsurancePolicies_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[InsurancePolicies] CHECK CONSTRAINT [FK_InsurancePolicies_Users_UserId]
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAdjusmentDetails_InventoryAdjusments_InventoryAdjusmentId] FOREIGN KEY([InventoryAdjusmentId])
REFERENCES [dbo].[InventoryAdjusments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails] CHECK CONSTRAINT [FK_InventoryAdjusmentDetails_InventoryAdjusments_InventoryAdjusmentId]
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAdjusmentDetails_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails] CHECK CONSTRAINT [FK_InventoryAdjusmentDetails_Products_ProductId]
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAdjusmentDetails_StockProducts_StockProductId] FOREIGN KEY([StockProductId])
REFERENCES [dbo].[StockProducts] ([Id])
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails] CHECK CONSTRAINT [FK_InventoryAdjusmentDetails_StockProducts_StockProductId]
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAdjusmentDetails_TransactionStocks_TransactionStockId] FOREIGN KEY([TransactionStockId])
REFERENCES [dbo].[TransactionStocks] ([Id])
GO
ALTER TABLE [dbo].[InventoryAdjusmentDetails] CHECK CONSTRAINT [FK_InventoryAdjusmentDetails_TransactionStocks_TransactionStockId]
GO
ALTER TABLE [dbo].[InventoryAdjusments]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAdjusments_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[InventoryAdjusments] CHECK CONSTRAINT [FK_InventoryAdjusments_Companies_CompanyId]
GO
ALTER TABLE [dbo].[InventoryAdjusments]  WITH CHECK ADD  CONSTRAINT [FK_InventoryAdjusments_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[InventoryAdjusments] CHECK CONSTRAINT [FK_InventoryAdjusments_Locations_LocationId]
GO
ALTER TABLE [dbo].[JobPositions]  WITH CHECK ADD  CONSTRAINT [FK_JobPositions_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[JobPositions] CHECK CONSTRAINT [FK_JobPositions_Departments_DepartmentId]
GO
ALTER TABLE [dbo].[KioskQueues]  WITH CHECK ADD  CONSTRAINT [FK_KioskQueues_ClassTypes_ClassTypeId] FOREIGN KEY([ClassTypeId])
REFERENCES [dbo].[ClassTypes] ([Id])
GO
ALTER TABLE [dbo].[KioskQueues] CHECK CONSTRAINT [FK_KioskQueues_ClassTypes_ClassTypeId]
GO
ALTER TABLE [dbo].[KioskQueues]  WITH CHECK ADD  CONSTRAINT [FK_KioskQueues_Kiosks_KioskId] FOREIGN KEY([KioskId])
REFERENCES [dbo].[Kiosks] ([Id])
GO
ALTER TABLE [dbo].[KioskQueues] CHECK CONSTRAINT [FK_KioskQueues_Kiosks_KioskId]
GO
ALTER TABLE [dbo].[KioskQueues]  WITH CHECK ADD  CONSTRAINT [FK_KioskQueues_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[KioskQueues] CHECK CONSTRAINT [FK_KioskQueues_Services_ServiceId]
GO
ALTER TABLE [dbo].[KioskQueues]  WITH CHECK ADD  CONSTRAINT [FK_KioskQueues_Services_ServiceKId] FOREIGN KEY([ServiceKId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[KioskQueues] CHECK CONSTRAINT [FK_KioskQueues_Services_ServiceKId]
GO
ALTER TABLE [dbo].[Kiosks]  WITH CHECK ADD  CONSTRAINT [FK_Kiosks_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[Kiosks] CHECK CONSTRAINT [FK_Kiosks_Services_ServiceId]
GO
ALTER TABLE [dbo].[Kiosks]  WITH CHECK ADD  CONSTRAINT [FK_Kiosks_Users_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Kiosks] CHECK CONSTRAINT [FK_Kiosks_Users_PatientId]
GO
ALTER TABLE [dbo].[Kiosks]  WITH CHECK ADD  CONSTRAINT [FK_Kiosks_Users_PhysicianId] FOREIGN KEY([PhysicianId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Kiosks] CHECK CONSTRAINT [FK_Kiosks_Users_PhysicianId]
GO
ALTER TABLE [dbo].[LabResultDetails]  WITH CHECK ADD  CONSTRAINT [FK_LabResultDetails_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId] FOREIGN KEY([GeneralConsultanMedicalSupportId])
REFERENCES [dbo].[GeneralConsultanMedicalSupports] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabResultDetails] CHECK CONSTRAINT [FK_LabResultDetails_GeneralConsultanMedicalSupports_GeneralConsultanMedicalSupportId]
GO
ALTER TABLE [dbo].[LabResultDetails]  WITH CHECK ADD  CONSTRAINT [FK_LabResultDetails_LabUoms_LabUomId] FOREIGN KEY([LabUomId])
REFERENCES [dbo].[LabUoms] ([Id])
GO
ALTER TABLE [dbo].[LabResultDetails] CHECK CONSTRAINT [FK_LabResultDetails_LabUoms_LabUomId]
GO
ALTER TABLE [dbo].[LabTestDetails]  WITH CHECK ADD  CONSTRAINT [FK_LabTestDetails_LabTests_LabTestId] FOREIGN KEY([LabTestId])
REFERENCES [dbo].[LabTests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabTestDetails] CHECK CONSTRAINT [FK_LabTestDetails_LabTests_LabTestId]
GO
ALTER TABLE [dbo].[LabTestDetails]  WITH CHECK ADD  CONSTRAINT [FK_LabTestDetails_LabUoms_LabUomId] FOREIGN KEY([LabUomId])
REFERENCES [dbo].[LabUoms] ([Id])
GO
ALTER TABLE [dbo].[LabTestDetails] CHECK CONSTRAINT [FK_LabTestDetails_LabUoms_LabUomId]
GO
ALTER TABLE [dbo].[LabTests]  WITH CHECK ADD  CONSTRAINT [FK_LabTests_SampleTypes_SampleTypeId] FOREIGN KEY([SampleTypeId])
REFERENCES [dbo].[SampleTypes] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[LabTests] CHECK CONSTRAINT [FK_LabTests_SampleTypes_SampleTypeId]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Companies_CompanyId]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Locations_ParentLocationId] FOREIGN KEY([ParentLocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Locations_ParentLocationId]
GO
ALTER TABLE [dbo].[MedicamentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroupDetails_DrugDosages_FrequencyId] FOREIGN KEY([FrequencyId])
REFERENCES [dbo].[DrugDosages] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroupDetails] CHECK CONSTRAINT [FK_MedicamentGroupDetails_DrugDosages_FrequencyId]
GO
ALTER TABLE [dbo].[MedicamentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroupDetails_MedicamentGroups_MedicamentGroupId] FOREIGN KEY([MedicamentGroupId])
REFERENCES [dbo].[MedicamentGroups] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroupDetails] CHECK CONSTRAINT [FK_MedicamentGroupDetails_MedicamentGroups_MedicamentGroupId]
GO
ALTER TABLE [dbo].[MedicamentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroupDetails_Products_MedicamentId] FOREIGN KEY([MedicamentId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroupDetails] CHECK CONSTRAINT [FK_MedicamentGroupDetails_Products_MedicamentId]
GO
ALTER TABLE [dbo].[MedicamentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroupDetails_Signas_SignaId] FOREIGN KEY([SignaId])
REFERENCES [dbo].[Signas] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroupDetails] CHECK CONSTRAINT [FK_MedicamentGroupDetails_Signas_SignaId]
GO
ALTER TABLE [dbo].[MedicamentGroupDetails]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroupDetails_Uoms_UnitOfDosageId] FOREIGN KEY([UnitOfDosageId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroupDetails] CHECK CONSTRAINT [FK_MedicamentGroupDetails_Uoms_UnitOfDosageId]
GO
ALTER TABLE [dbo].[MedicamentGroups]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroups_FormDrugs_FormDrugId] FOREIGN KEY([FormDrugId])
REFERENCES [dbo].[FormDrugs] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroups] CHECK CONSTRAINT [FK_MedicamentGroups_FormDrugs_FormDrugId]
GO
ALTER TABLE [dbo].[MedicamentGroups]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroups_Uoms_UoMId] FOREIGN KEY([UoMId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroups] CHECK CONSTRAINT [FK_MedicamentGroups_Uoms_UoMId]
GO
ALTER TABLE [dbo].[MedicamentGroups]  WITH CHECK ADD  CONSTRAINT [FK_MedicamentGroups_Users_PhycisianId] FOREIGN KEY([PhycisianId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[MedicamentGroups] CHECK CONSTRAINT [FK_MedicamentGroups_Users_PhycisianId]
GO
ALTER TABLE [dbo].[Medicaments]  WITH CHECK ADD  CONSTRAINT [FK_Medicaments_DrugDosages_FrequencyId] FOREIGN KEY([FrequencyId])
REFERENCES [dbo].[DrugDosages] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Medicaments] CHECK CONSTRAINT [FK_Medicaments_DrugDosages_FrequencyId]
GO
ALTER TABLE [dbo].[Medicaments]  WITH CHECK ADD  CONSTRAINT [FK_Medicaments_DrugRoutes_RouteId] FOREIGN KEY([RouteId])
REFERENCES [dbo].[DrugRoutes] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Medicaments] CHECK CONSTRAINT [FK_Medicaments_DrugRoutes_RouteId]
GO
ALTER TABLE [dbo].[Medicaments]  WITH CHECK ADD  CONSTRAINT [FK_Medicaments_FormDrugs_FormId] FOREIGN KEY([FormId])
REFERENCES [dbo].[FormDrugs] ([Id])
GO
ALTER TABLE [dbo].[Medicaments] CHECK CONSTRAINT [FK_Medicaments_FormDrugs_FormId]
GO
ALTER TABLE [dbo].[Medicaments]  WITH CHECK ADD  CONSTRAINT [FK_Medicaments_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Medicaments] CHECK CONSTRAINT [FK_Medicaments_Products_ProductId]
GO
ALTER TABLE [dbo].[Medicaments]  WITH CHECK ADD  CONSTRAINT [FK_Medicaments_Signas_SignaId] FOREIGN KEY([SignaId])
REFERENCES [dbo].[Signas] ([Id])
GO
ALTER TABLE [dbo].[Medicaments] CHECK CONSTRAINT [FK_Medicaments_Signas_SignaId]
GO
ALTER TABLE [dbo].[Medicaments]  WITH CHECK ADD  CONSTRAINT [FK_Medicaments_Uoms_UomId] FOREIGN KEY([UomId])
REFERENCES [dbo].[Uoms] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Medicaments] CHECK CONSTRAINT [FK_Medicaments_Uoms_UomId]
GO
ALTER TABLE [dbo].[PatientAllergies]  WITH CHECK ADD  CONSTRAINT [FK_PatientAllergies_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientAllergies] CHECK CONSTRAINT [FK_PatientAllergies_Users_UserId]
GO
ALTER TABLE [dbo].[PatientFamilyRelations]  WITH CHECK ADD  CONSTRAINT [FK_PatientFamilyRelations_Families_FamilyId] FOREIGN KEY([FamilyId])
REFERENCES [dbo].[Families] ([Id])
GO
ALTER TABLE [dbo].[PatientFamilyRelations] CHECK CONSTRAINT [FK_PatientFamilyRelations_Families_FamilyId]
GO
ALTER TABLE [dbo].[PatientFamilyRelations]  WITH CHECK ADD  CONSTRAINT [FK_PatientFamilyRelations_Users_FamilyMemberId] FOREIGN KEY([FamilyMemberId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PatientFamilyRelations] CHECK CONSTRAINT [FK_PatientFamilyRelations_Users_FamilyMemberId]
GO
ALTER TABLE [dbo].[PatientFamilyRelations]  WITH CHECK ADD  CONSTRAINT [FK_PatientFamilyRelations_Users_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PatientFamilyRelations] CHECK CONSTRAINT [FK_PatientFamilyRelations_Users_PatientId]
GO
ALTER TABLE [dbo].[Pharmacies]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacies_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[Pharmacies] CHECK CONSTRAINT [FK_Pharmacies_Locations_LocationId]
GO
ALTER TABLE [dbo].[Pharmacies]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacies_MedicamentGroups_MedicamentGroupId] FOREIGN KEY([MedicamentGroupId])
REFERENCES [dbo].[MedicamentGroups] ([Id])
GO
ALTER TABLE [dbo].[Pharmacies] CHECK CONSTRAINT [FK_Pharmacies_MedicamentGroups_MedicamentGroupId]
GO
ALTER TABLE [dbo].[Pharmacies]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacies_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[Pharmacies] CHECK CONSTRAINT [FK_Pharmacies_Services_ServiceId]
GO
ALTER TABLE [dbo].[Pharmacies]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacies_Users_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Pharmacies] CHECK CONSTRAINT [FK_Pharmacies_Users_PatientId]
GO
ALTER TABLE [dbo].[Pharmacies]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacies_Users_PractitionerId] FOREIGN KEY([PractitionerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Pharmacies] CHECK CONSTRAINT [FK_Pharmacies_Users_PractitionerId]
GO
ALTER TABLE [dbo].[PharmacyLogs]  WITH CHECK ADD  CONSTRAINT [FK_PharmacyLogs_Pharmacies_PharmacyId] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacies] ([Id])
GO
ALTER TABLE [dbo].[PharmacyLogs] CHECK CONSTRAINT [FK_PharmacyLogs_Pharmacies_PharmacyId]
GO
ALTER TABLE [dbo].[PharmacyLogs]  WITH CHECK ADD  CONSTRAINT [FK_PharmacyLogs_Users_UserById] FOREIGN KEY([UserById])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[PharmacyLogs] CHECK CONSTRAINT [FK_PharmacyLogs_Users_UserById]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_DrugDosages_DrugDosageId] FOREIGN KEY([DrugDosageId])
REFERENCES [dbo].[DrugDosages] ([Id])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_DrugDosages_DrugDosageId]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_DrugRoutes_DrugRouteId] FOREIGN KEY([DrugRouteId])
REFERENCES [dbo].[DrugRoutes] ([Id])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_DrugRoutes_DrugRouteId]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_FormDrugs_DrugFormId] FOREIGN KEY([DrugFormId])
REFERENCES [dbo].[FormDrugs] ([Id])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_FormDrugs_DrugFormId]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_MedicamentGroups_MedicamentGroupId] FOREIGN KEY([MedicamentGroupId])
REFERENCES [dbo].[MedicamentGroups] ([Id])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_MedicamentGroups_MedicamentGroupId]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_Pharmacies_PharmacyId] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_Pharmacies_PharmacyId]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_Products_ProductId]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_Signas_SignaId] FOREIGN KEY([SignaId])
REFERENCES [dbo].[Signas] ([Id])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_Signas_SignaId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_BpjsClassifications_BpjsClassificationId] FOREIGN KEY([BpjsClassificationId])
REFERENCES [dbo].[BpjsClassifications] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_BpjsClassifications_BpjsClassificationId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Companies_CompanyId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategories] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Uoms_PurchaseUomId] FOREIGN KEY([PurchaseUomId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Uoms_PurchaseUomId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Uoms_UomId] FOREIGN KEY([UomId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Uoms_UomId]
GO
ALTER TABLE [dbo].[Provinces]  WITH CHECK ADD  CONSTRAINT [FK_Provinces_Countries_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Provinces] CHECK CONSTRAINT [FK_Provinces_Countries_CountryId]
GO
ALTER TABLE [dbo].[ReceivingLogs]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingLogs_Locations_SourceId] FOREIGN KEY([SourceId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[ReceivingLogs] CHECK CONSTRAINT [FK_ReceivingLogs_Locations_SourceId]
GO
ALTER TABLE [dbo].[ReceivingLogs]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingLogs_ReceivingStocks_ReceivingId] FOREIGN KEY([ReceivingId])
REFERENCES [dbo].[ReceivingStocks] ([Id])
GO
ALTER TABLE [dbo].[ReceivingLogs] CHECK CONSTRAINT [FK_ReceivingLogs_ReceivingStocks_ReceivingId]
GO
ALTER TABLE [dbo].[ReceivingLogs]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingLogs_Users_UserById] FOREIGN KEY([UserById])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ReceivingLogs] CHECK CONSTRAINT [FK_ReceivingLogs_Users_UserById]
GO
ALTER TABLE [dbo].[ReceivingStockDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingStockDetails_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ReceivingStockDetails] CHECK CONSTRAINT [FK_ReceivingStockDetails_Products_ProductId]
GO
ALTER TABLE [dbo].[ReceivingStockDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingStockDetails_ReceivingStocks_ReceivingStockId] FOREIGN KEY([ReceivingStockId])
REFERENCES [dbo].[ReceivingStocks] ([Id])
GO
ALTER TABLE [dbo].[ReceivingStockDetails] CHECK CONSTRAINT [FK_ReceivingStockDetails_ReceivingStocks_ReceivingStockId]
GO
ALTER TABLE [dbo].[ReceivingStockDetails]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingStockDetails_StockProducts_StockId] FOREIGN KEY([StockId])
REFERENCES [dbo].[StockProducts] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ReceivingStockDetails] CHECK CONSTRAINT [FK_ReceivingStockDetails_StockProducts_StockId]
GO
ALTER TABLE [dbo].[ReceivingStocks]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingStocks_Locations_DestinationId] FOREIGN KEY([DestinationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[ReceivingStocks] CHECK CONSTRAINT [FK_ReceivingStocks_Locations_DestinationId]
GO
ALTER TABLE [dbo].[ReorderingRules]  WITH CHECK ADD  CONSTRAINT [FK_ReorderingRules_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ReorderingRules] CHECK CONSTRAINT [FK_ReorderingRules_Companies_CompanyId]
GO
ALTER TABLE [dbo].[ReorderingRules]  WITH CHECK ADD  CONSTRAINT [FK_ReorderingRules_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ReorderingRules] CHECK CONSTRAINT [FK_ReorderingRules_Locations_LocationId]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Services_ServicedId] FOREIGN KEY([ServicedId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Services_ServicedId]
GO
ALTER TABLE [dbo].[SickLeaves]  WITH CHECK ADD  CONSTRAINT [FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId] FOREIGN KEY([GeneralConsultansId])
REFERENCES [dbo].[GeneralConsultanServices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SickLeaves] CHECK CONSTRAINT [FK_SickLeaves_GeneralConsultanServices_GeneralConsultansId]
GO
ALTER TABLE [dbo].[StockOutLines]  WITH CHECK ADD  CONSTRAINT [FK_StockOutLines_ConcoctionLines_LinesId] FOREIGN KEY([LinesId])
REFERENCES [dbo].[ConcoctionLines] ([Id])
GO
ALTER TABLE [dbo].[StockOutLines] CHECK CONSTRAINT [FK_StockOutLines_ConcoctionLines_LinesId]
GO
ALTER TABLE [dbo].[StockOutLines]  WITH CHECK ADD  CONSTRAINT [FK_StockOutLines_TransactionStocks_TransactionStockId] FOREIGN KEY([TransactionStockId])
REFERENCES [dbo].[TransactionStocks] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[StockOutLines] CHECK CONSTRAINT [FK_StockOutLines_TransactionStocks_TransactionStockId]
GO
ALTER TABLE [dbo].[StockOutPrescriptions]  WITH CHECK ADD  CONSTRAINT [FK_StockOutPrescriptions_Prescriptions_PrescriptionId] FOREIGN KEY([PrescriptionId])
REFERENCES [dbo].[Prescriptions] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[StockOutPrescriptions] CHECK CONSTRAINT [FK_StockOutPrescriptions_Prescriptions_PrescriptionId]
GO
ALTER TABLE [dbo].[StockOutPrescriptions]  WITH CHECK ADD  CONSTRAINT [FK_StockOutPrescriptions_StockProducts_StockProductId] FOREIGN KEY([StockProductId])
REFERENCES [dbo].[StockProducts] ([Id])
GO
ALTER TABLE [dbo].[StockOutPrescriptions] CHECK CONSTRAINT [FK_StockOutPrescriptions_StockProducts_StockProductId]
GO
ALTER TABLE [dbo].[StockOutPrescriptions]  WITH CHECK ADD  CONSTRAINT [FK_StockOutPrescriptions_TransactionStocks_TransactionStockId] FOREIGN KEY([TransactionStockId])
REFERENCES [dbo].[TransactionStocks] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[StockOutPrescriptions] CHECK CONSTRAINT [FK_StockOutPrescriptions_TransactionStocks_TransactionStockId]
GO
ALTER TABLE [dbo].[StockProducts]  WITH CHECK ADD  CONSTRAINT [FK_StockProducts_Locations_DestinanceId] FOREIGN KEY([DestinanceId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[StockProducts] CHECK CONSTRAINT [FK_StockProducts_Locations_DestinanceId]
GO
ALTER TABLE [dbo].[StockProducts]  WITH CHECK ADD  CONSTRAINT [FK_StockProducts_Locations_SourceId] FOREIGN KEY([SourceId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[StockProducts] CHECK CONSTRAINT [FK_StockProducts_Locations_SourceId]
GO
ALTER TABLE [dbo].[StockProducts]  WITH CHECK ADD  CONSTRAINT [FK_StockProducts_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[StockProducts] CHECK CONSTRAINT [FK_StockProducts_Products_ProductId]
GO
ALTER TABLE [dbo].[StockProducts]  WITH CHECK ADD  CONSTRAINT [FK_StockProducts_Uoms_UomId] FOREIGN KEY([UomId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[StockProducts] CHECK CONSTRAINT [FK_StockProducts_Uoms_UomId]
GO
ALTER TABLE [dbo].[TransactionStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransactionStocks_InventoryAdjusments_InventoryAdjusmentId] FOREIGN KEY([InventoryAdjusmentId])
REFERENCES [dbo].[InventoryAdjusments] ([Id])
GO
ALTER TABLE [dbo].[TransactionStocks] CHECK CONSTRAINT [FK_TransactionStocks_InventoryAdjusments_InventoryAdjusmentId]
GO
ALTER TABLE [dbo].[TransactionStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransactionStocks_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[TransactionStocks] CHECK CONSTRAINT [FK_TransactionStocks_Locations_LocationId]
GO
ALTER TABLE [dbo].[TransactionStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransactionStocks_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[TransactionStocks] CHECK CONSTRAINT [FK_TransactionStocks_Products_ProductId]
GO
ALTER TABLE [dbo].[TransactionStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransactionStocks_Uoms_UomId] FOREIGN KEY([UomId])
REFERENCES [dbo].[Uoms] ([Id])
GO
ALTER TABLE [dbo].[TransactionStocks] CHECK CONSTRAINT [FK_TransactionStocks_Uoms_UomId]
GO
ALTER TABLE [dbo].[TransferStockLogs]  WITH CHECK ADD  CONSTRAINT [FK_TransferStockLogs_Locations_DestinationId] FOREIGN KEY([DestinationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[TransferStockLogs] CHECK CONSTRAINT [FK_TransferStockLogs_Locations_DestinationId]
GO
ALTER TABLE [dbo].[TransferStockLogs]  WITH CHECK ADD  CONSTRAINT [FK_TransferStockLogs_Locations_SourceId] FOREIGN KEY([SourceId])
REFERENCES [dbo].[Locations] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[TransferStockLogs] CHECK CONSTRAINT [FK_TransferStockLogs_Locations_SourceId]
GO
ALTER TABLE [dbo].[TransferStockLogs]  WITH CHECK ADD  CONSTRAINT [FK_TransferStockLogs_TransferStocks_TransferStockId] FOREIGN KEY([TransferStockId])
REFERENCES [dbo].[TransferStocks] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[TransferStockLogs] CHECK CONSTRAINT [FK_TransferStockLogs_TransferStocks_TransferStockId]
GO
ALTER TABLE [dbo].[TransferStockProduct]  WITH CHECK ADD  CONSTRAINT [FK_TransferStockProduct_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[TransferStockProduct] CHECK CONSTRAINT [FK_TransferStockProduct_Products_ProductId]
GO
ALTER TABLE [dbo].[TransferStockProduct]  WITH CHECK ADD  CONSTRAINT [FK_TransferStockProduct_TransferStocks_TransferStockId] FOREIGN KEY([TransferStockId])
REFERENCES [dbo].[TransferStocks] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[TransferStockProduct] CHECK CONSTRAINT [FK_TransferStockProduct_TransferStocks_TransferStockId]
GO
ALTER TABLE [dbo].[TransferStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransferStocks_Locations_DestinationId] FOREIGN KEY([DestinationId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[TransferStocks] CHECK CONSTRAINT [FK_TransferStocks_Locations_DestinationId]
GO
ALTER TABLE [dbo].[TransferStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransferStocks_Locations_SourceId] FOREIGN KEY([SourceId])
REFERENCES [dbo].[Locations] ([Id])
GO
ALTER TABLE [dbo].[TransferStocks] CHECK CONSTRAINT [FK_TransferStocks_Locations_SourceId]
GO
ALTER TABLE [dbo].[TransferStocks]  WITH CHECK ADD  CONSTRAINT [FK_TransferStocks_StockProducts_StockProductId] FOREIGN KEY([StockProductId])
REFERENCES [dbo].[StockProducts] ([Id])
GO
ALTER TABLE [dbo].[TransferStocks] CHECK CONSTRAINT [FK_TransferStocks_StockProducts_StockProductId]
GO
ALTER TABLE [dbo].[Uoms]  WITH CHECK ADD  CONSTRAINT [FK_Uoms_UomCategories_UomCategoryId] FOREIGN KEY([UomCategoryId])
REFERENCES [dbo].[UomCategories] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Uoms] CHECK CONSTRAINT [FK_Uoms_UomCategories_UomCategoryId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Cities_DomicileCityId] FOREIGN KEY([DomicileCityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Cities_DomicileCityId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Cities_IdCardCityId] FOREIGN KEY([IdCardCityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Cities_IdCardCityId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Countries_DomicileCountryId] FOREIGN KEY([DomicileCountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Countries_DomicileCountryId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Countries_IdCardCountryId] FOREIGN KEY([IdCardCountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Countries_IdCardCountryId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Degrees_DegreeId] FOREIGN KEY([DegreeId])
REFERENCES [dbo].[Degrees] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Degrees_DegreeId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Departments_DepartmentId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Districts_DomicileDistrictId] FOREIGN KEY([DomicileDistrictId])
REFERENCES [dbo].[Districts] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Districts_DomicileDistrictId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Districts_IdCardDistrictId] FOREIGN KEY([IdCardDistrictId])
REFERENCES [dbo].[Districts] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Districts_IdCardDistrictId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_EmailTemplates_EmailTemplateId] FOREIGN KEY([EmailTemplateId])
REFERENCES [dbo].[EmailTemplates] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_EmailTemplates_EmailTemplateId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Genders_GenderId] FOREIGN KEY([GenderId])
REFERENCES [dbo].[Genders] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Genders_GenderId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Groups_GroupId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_JobPositions_JobPositionId] FOREIGN KEY([JobPositionId])
REFERENCES [dbo].[JobPositions] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_JobPositions_JobPositionId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Occupationals_OccupationalId] FOREIGN KEY([OccupationalId])
REFERENCES [dbo].[Occupationals] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Occupationals_OccupationalId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Provinces_DomicileProvinceId] FOREIGN KEY([DomicileProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Provinces_DomicileProvinceId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Provinces_IdCardProvinceId] FOREIGN KEY([IdCardProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Provinces_IdCardProvinceId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Religions_ReligionId] FOREIGN KEY([ReligionId])
REFERENCES [dbo].[Religions] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Religions_ReligionId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Specialities_SpecialityId] FOREIGN KEY([SpecialityId])
REFERENCES [dbo].[Specialities] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Specialities_SpecialityId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users_SupervisorId] FOREIGN KEY([SupervisorId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_SupervisorId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Villages_DomicileVillageId] FOREIGN KEY([DomicileVillageId])
REFERENCES [dbo].[Villages] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Villages_DomicileVillageId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Villages_IdCardVillageId] FOREIGN KEY([IdCardVillageId])
REFERENCES [dbo].[Villages] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Villages_IdCardVillageId]
GO
ALTER TABLE [dbo].[Villages]  WITH CHECK ADD  CONSTRAINT [FK_Villages_Cities_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Villages] CHECK CONSTRAINT [FK_Villages_Cities_CityId]
GO
ALTER TABLE [dbo].[Villages]  WITH CHECK ADD  CONSTRAINT [FK_Villages_Districts_DistrictId] FOREIGN KEY([DistrictId])
REFERENCES [dbo].[Districts] ([Id])
GO
ALTER TABLE [dbo].[Villages] CHECK CONSTRAINT [FK_Villages_Districts_DistrictId]
GO
ALTER TABLE [dbo].[Villages]  WITH CHECK ADD  CONSTRAINT [FK_Villages_Provinces_ProvinceId] FOREIGN KEY([ProvinceId])
REFERENCES [dbo].[Provinces] ([Id])
GO
ALTER TABLE [dbo].[Villages] CHECK CONSTRAINT [FK_Villages_Provinces_ProvinceId]
GO
