using McDermott.Application.Features.Commands;
using McDermott.Domain.Common;
using static McDermott.Application.Features.Commands.GetDataCommand;

namespace McDermott.Application.Features.Queries
{
    public class GetDataQueryHandler(IUnitOfWork unitOfWork) :

    #region Transactions

        IRequestHandler<GetQueryGeneralConsultanService, IQueryable<GeneralConsultanService>>,
        IRequestHandler<GetQueryGeneralConsultanServiceCPPT, IQueryable<GeneralConsultanCPPT>>,

    #endregion Transactions

    #region Configuration

        IRequestHandler<GetQueryUser, IQueryable<User>>,
        IRequestHandler<GetQueryGroup, IQueryable<Group>>,
        IRequestHandler<GetQueryGroupMenu, IQueryable<GroupMenu>>,
        IRequestHandler<GetQueryMenu, IQueryable<Menu>>,
        IRequestHandler<GetQueryCompany, IQueryable<Company>>,
        IRequestHandler<GetQueryCountry, IQueryable<Country>>,
        IRequestHandler<GetQueryProvince, IQueryable<Province>>,
        IRequestHandler<GetQueryCity, IQueryable<City>>,
        IRequestHandler<GetQueryOccupational, IQueryable<Occupational>>,
        IRequestHandler<GetQueryVillage, IQueryable<Village>>,
        IRequestHandler<GetQueryDistrict, IQueryable<District>>,

    #endregion Configuration

    #region Medicals

        IRequestHandler<GetQueryDiagnosis, IQueryable<Diagnosis>>,
        IRequestHandler<GetQuerySpeciality, IQueryable<Speciality>>,
        IRequestHandler<GetQueryProject, IQueryable<Project>>,
        IRequestHandler<GetQueryService, IQueryable<Service>>,
        IRequestHandler<GetQueryUom, IQueryable<Uom>>,
        IRequestHandler<GetQueryLabUom, IQueryable<LabUom>>,
        IRequestHandler<GetQueryNursingDiagnoses, IQueryable<NursingDiagnoses>>,
        IRequestHandler<GetQueryLabTest, IQueryable<LabTest>>,
        IRequestHandler<GetQueryLabTestDetail, IQueryable<LabTestDetail>>,
        IRequestHandler<GetQuerySampleType, IQueryable<SampleType>>,

    #endregion Medicals

    #region Patients

        IRequestHandler<GetQueryPatientFamilyRelation, IQueryable<PatientFamilyRelation>>,
        IRequestHandler<GetQueryInsurancePolicy, IQueryable<InsurancePolicy>>,

    #endregion Patients

    #region Employees

        IRequestHandler<GetQueryJobPosition, IQueryable<JobPosition>>,
        IRequestHandler<GetQueryDepartment, IQueryable<Department>>,

    #endregion Employees

    #region Inventories

        IRequestHandler<GetQueryUomCategory, IQueryable<UomCategory>>

    #endregion Inventories

    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        #region Transactions

        // GeneralConsultanService
        public Task<IQueryable<GeneralConsultanService>> Handle(GetQueryGeneralConsultanService request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select);
        }

        public Task<IQueryable<GeneralConsultanCPPT>> Handle(GetQueryGeneralConsultanServiceCPPT request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<GeneralConsultanCPPT>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanCPPT
            {
                Id = x.Id,
                Subjective = x.Subjective,
                Objective = x.Objective,
                NursingDiagnosesId = x.NursingDiagnosesId,
                NursingDiagnoses = new NursingDiagnoses
                {
                    Problem = x.NursingDiagnoses != null ? x.NursingDiagnoses.Problem : "",
                },
                DiagnosisId = x.DiagnosisId,
                Diagnosis = new Diagnosis
                {
                    Name = x.Diagnosis != null ? x.Diagnosis.Name : "",
                },
                UserId = x.UserId,
                User = new User
                {
                    Name = x.User != null ? x.User.Name : "",
                },
                Planning = x.Planning,
                MedicationTherapy = x.MedicationTherapy,
                NonMedicationTherapy = x.NonMedicationTherapy,
                Anamnesa = x.Anamnesa,
                DateTime = x.DateTime,
            } : request.Select);
        }

        #endregion Transactions

        #region Configurations

        // User
        public Task<IQueryable<User>> Handle(GetQueryUser request, CancellationToken cancellationToken)
        {
            return HandleQuery<User>(request, cancellationToken, request.Select is null ? x => new User
            {
                Id = x.Id,
                Name = x.Name,
            } : request.Select);
        }

        // Group
        public Task<IQueryable<Group>> Handle(GetQueryGroup request, CancellationToken cancellationToken)
        {
            return HandleQuery<Group>(request, cancellationToken, request.Select is null ? x => new Group
            {
                Id = x.Id,
                Name = x.Name
            } : request.Select);
        }

        // GroupMenu
        public Task<IQueryable<GroupMenu>> Handle(GetQueryGroupMenu request, CancellationToken cancellationToken)
        {
            return HandleQuery<GroupMenu>(request, cancellationToken, request.Select is null ? x => new GroupMenu
            {
                Id = x.Id,
                MenuId = x.MenuId,
                Menu = new Menu
                {
                    Name = x.Menu.Name,
                    Parent = new Menu
                    {
                        Name = x.Menu.Parent.Name
                    }
                },

                IsCreate = x.IsCreate,
                IsDelete = x.IsDelete,
                IsDefaultData = x.IsDefaultData,
                IsImport = x.IsImport,
                IsRead = x.IsRead,
                IsUpdate = x.IsUpdate,
            } : request.Select);
        }

        // Menu
        public Task<IQueryable<Menu>> Handle(GetQueryMenu request, CancellationToken cancellationToken)
        {
            return HandleQuery<Menu>(request, cancellationToken, request.Select is null ? x => new Menu
            {
                Id = x.Id,
                Name = x.Name,
                Sequence = x.Sequence,
                Url = x.Url,
                ParentId = x.ParentId,
                Icon = x.Icon,
                IsDefaultData = x.IsDefaultData,
                Parent = new Menu
                {
                    Name = x.Parent.Name
                }
            } : request.Select);
        }

        // Company
        public Task<IQueryable<Company>> Handle(GetQueryCompany request, CancellationToken cancellationToken)
        {
            return HandleQuery<Company>(request, cancellationToken, request.Select is null ? x => new Company
            {
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Email = x.Email,
                Website = x.Website,
                VAT = x.VAT,
                Street1 = x.Street1,
                Street2 = x.Street2,
                Zip = x.Zip,
                CurrencyId = x.CurrencyId,
                Logo = x.Logo,
                CityId = x.CityId,
                ProvinceId = x.ProvinceId,
                CountryId = x.CountryId,
                Country = new Country
                {
                    Name = x.Country.Name
                },
                Province = new Province
                {
                    Name = x.Province.Name
                },
                City = new City
                {
                    Name = x.City.Name
                },
            } : request.Select);
        }

        // Country
        public Task<IQueryable<Country>> Handle(GetQueryCountry request, CancellationToken cancellationToken)
        {
            return HandleQuery<Country>(request, cancellationToken, request.Select is null ? x => new Country
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            } : request.Select);
        }

        // Province
        public Task<IQueryable<Province>> Handle(GetQueryProvince request, CancellationToken cancellationToken)
        {
            return HandleQuery<Province>(request, cancellationToken, request.Select is null ? x => new Province
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                CountryId = x.CountryId,
                Country = new Country
                {
                    Name = x.Country.Name,
                    Code = x.Country.Code
                }
            } : request.Select);
        }

        // City
        public Task<IQueryable<City>> Handle(GetQueryCity request, CancellationToken cancellationToken)
        {
            return HandleQuery<City>(request, cancellationToken, request.Select is null ? x => new City
            {
                Id = x.Id,
                Name = x.Name,
                ProvinceId = x.ProvinceId,
                Province = new Domain.Entities.Province
                {
                    Name = x.Province.Name
                },
            } : request.Select);
        }

        // Occupational
        public Task<IQueryable<Occupational>> Handle(GetQueryOccupational request, CancellationToken cancellationToken)
        {
            return HandleQuery<Occupational>(request, cancellationToken, request.Select is null ? x => new Occupational
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            } : request.Select);
        }

        public Task<IQueryable<District>> Handle(GetQueryDistrict request, CancellationToken cancellationToken)
        {
            return HandleQuery<District>(request, cancellationToken, request.Select is null ? x => new District
            {
                Id = x.Id,
                Name = x.Name,
                ProvinceId = x.ProvinceId,
                CityId = x.CityId,
                Province = new Domain.Entities.Province
                {
                    Name = x.Province.Name
                },
                City = new Domain.Entities.City
                {
                    Name = x.City.Name
                },
            } : request.Select);
        }

        public Task<IQueryable<Village>> Handle(GetQueryVillage request, CancellationToken cancellationToken)
        {
            return HandleQuery<Village>(request, cancellationToken, request.Select is null ? x => new Village
            {
                Id = x.Id,
                Name = x.Name,
                PostalCode = x.PostalCode,
                ProvinceId = x.ProvinceId,
                CityId = x.CityId,
                DistrictId = x.DistrictId,
                Province = new Domain.Entities.Province
                {
                    Name = x.Province.Name
                },
                City = new Domain.Entities.City
                {
                    Name = x.City.Name
                },
                District = new Domain.Entities.District
                {
                    Name = x.District.Name
                },
            } : request.Select);
        }

        #endregion Configurations

        #region Medicals

        public Task<IQueryable<Diagnosis>> Handle(GetQueryDiagnosis request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<Diagnosis>(request, cancellationToken, request.Select is null ? x => new Diagnosis
            {
                Id = x.Id,
                Name = x.Name,
                NameInd = x.NameInd,
                Code = x.Code,
                CronisCategoryId = x.CronisCategoryId,
                CronisCategory = new CronisCategory
                {
                    Name = x.CronisCategory == null ? "-" : x.CronisCategory.Name,
                }
            } : request.Select);
        }

        public Task<IQueryable<Project>> Handle(GetQueryProject request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<Project>(request, cancellationToken, request.Select is null ? x => new Project
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            } : request.Select);
        }

        public Task<IQueryable<Uom>> Handle(GetQueryUom request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<Uom>(request, cancellationToken, request.Select is null ? x => new Uom
            {
                Id = x.Id,
                Name = x.Name,
                UomCategoryId = x.UomCategoryId,
                UomCategory = new UomCategory
                {
                    Name = x.UomCategory == null ? "" : x.UomCategory.Name,
                },
                Active = x.Active,
                BiggerRatio = x.BiggerRatio,
                RoundingPrecision = x.RoundingPrecision,
                Type = x.Type,
            } : request.Select);
        }

        public Task<IQueryable<LabUom>> Handle(GetQueryLabUom request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<LabUom>(request, cancellationToken, request.Select is null ? x => new LabUom
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            } : request.Select);
        }

        public Task<IQueryable<SampleType>> Handle(GetQuerySampleType request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<SampleType>(request, cancellationToken, request.Select is null ? x => new SampleType
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            } : request.Select);
        }

        public Task<IQueryable<LabTest>> Handle(GetQueryLabTest request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<LabTest>(request, cancellationToken, request.Select is null ? x => new LabTest
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                ResultType = x.ResultType,
                SampleTypeId = x.SampleTypeId,
                SampleType = new SampleType
                {
                    Name = x.SampleType.Name
                }
            } : request.Select);
        }

        public Task<IQueryable<LabTestDetail>> Handle(GetQueryLabTestDetail request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<LabTestDetail>(request, cancellationToken, request.Select is null ? x => new LabTestDetail
            {
                Id = x.Id,
                Name = x.Name,
                NormalRangeFemale = x.NormalRangeFemale,
                NormalRangeMale = x.NormalRangeMale,
                LabTestId = x.LabTestId,
                LabTest = new LabTest
                {
                    Name = x.LabTest.Name
                },
                LabUomId = x.LabUomId,
                LabUom = new LabUom
                {
                    Name = x.LabUom.Name
                },
                ResultValueType = x.ResultValueType,
                Remark = x.Remark,
                ResultType = x.ResultType
            } : request.Select);
        }

        public Task<IQueryable<Speciality>> Handle(GetQuerySpeciality request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<Speciality>(request, cancellationToken, request.Select is null ? x => new Speciality
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
            } : request.Select);
        }

        public Task<IQueryable<NursingDiagnoses>> Handle(GetQueryNursingDiagnoses request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<NursingDiagnoses>(request, cancellationToken, request.Select is null ? x => new NursingDiagnoses
            {
                Id = x.Id,
                Problem = x.Problem,
                Code = x.Code
            } : request.Select);
        }

        public Task<IQueryable<Service>> Handle(GetQueryService request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<Service>(request, cancellationToken, request.Select is null ? x => new Service
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Quota = x.Quota,
                IsKiosk = x.IsKiosk,
                IsMcu = x.IsMcu,
                IsMaternity = x.IsMaternity,
                IsPatient = x.IsPatient,
                IsTelemedicine = x.IsTelemedicine,
                IsVaccination = x.IsVaccination,
                ServicedId = x.ServicedId,
                Serviced = new Service
                {
                    Name = x.Serviced.Name
                }
            } : request.Select);
        }

        #endregion Medicals

        #region Patients

        public Task<IQueryable<PatientFamilyRelation>> Handle(GetQueryPatientFamilyRelation request, CancellationToken cancellationToken)
        {
            return HandleQuery<PatientFamilyRelation>(request, cancellationToken, request.Select is null ? x => new PatientFamilyRelation
            {
                Id = x.Id,
                PatientId = x.PatientId,
                FamilyId = x.FamilyId,
                FamilyMemberId = x.FamilyMemberId,
            } : request.Select);
        }

        public Task<IQueryable<InsurancePolicy>> Handle(GetQueryInsurancePolicy request, CancellationToken cancellationToken)
        {
            return HandleQuery<InsurancePolicy>(request, cancellationToken, request.Select is null ? x => new InsurancePolicy
            {
                Id = x.Id,
                UserId = x.UserId, // Patient
                User = new User
                {
                    Name = x.User.Name,
                },
                InsuranceId = x.InsuranceId,
                Insurance = new Insurance
                {
                    Name = x.Insurance.Name,
                },
                PolicyNumber = x.PolicyNumber,
                Active = x.Active,

                // BPJS Integration fields
                NoKartu = x.NoKartu,
                Nama = x.Nama,
                HubunganKeluarga = x.HubunganKeluarga,
                TglLahir = x.TglLahir,
                TglMulaiAktif = x.TglMulaiAktif,
                TglAkhirBerlaku = x.TglAkhirBerlaku,
                GolDarah = x.GolDarah,
                NoHP = x.NoHP,
                NoKTP = x.NoKTP,
                PstProl = x.PstProl,
                PstPrb = x.PstPrb,
                Aktif = x.Aktif,
                KetAktif = x.KetAktif,
                Tunggakan = x.Tunggakan,
                KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                KdProviderPstNmProvider = x.KdProviderPstNmProvider,
                KdProviderGigiKdProvider = x.KdProviderGigiKdProvider,
                KdProviderGigiNmProvider = x.KdProviderGigiNmProvider,
                JnsKelasNama = x.JnsKelasNama,
                JnsKelasKode = x.JnsKelasKode,
                JnsPesertaNama = x.JnsPesertaNama,
                JnsPesertaKode = x.JnsPesertaKode,
                AsuransiKdAsuransi = x.AsuransiKdAsuransi,
                AsuransiNmAsuransi = x.AsuransiNmAsuransi,
                AsuransiNoAsuransi = x.AsuransiNoAsuransi,
                AsuransiCob = x.AsuransiCob
            } : request.Select);
        }

        #endregion Patients

        #region Inventories

        public Task<IQueryable<UomCategory>> Handle(GetQueryUomCategory request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<UomCategory>(request, cancellationToken, request.Select is null ? x => new UomCategory
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type
            } : request.Select);
        }

        #endregion Inventories

        #region Employees

        public Task<IQueryable<JobPosition>> Handle(GetQueryJobPosition request, CancellationToken cancellationToken)
        {
            return HandleQuery<JobPosition>(request, cancellationToken, request.Select is null ? x => new JobPosition
            {
                Id = x.Id,
                Name = x.Name,
                DepartmentId = x.DepartmentId,
                Department = new Department
                {
                    Name = x.Department.Name
                }
            } : request.Select);
        }

        public Task<IQueryable<Department>> Handle(GetQueryDepartment request, CancellationToken cancellationToken)
        {
            return HandleQuery<Department>(request, cancellationToken, request.Select is null ? x => new Department
            {
                Id = x.Id,
                Name = x.Name,
                ParentDepartmentId = x.ParentDepartmentId,
                CompanyId = x.CompanyId,
                ManagerId = x.ManagerId,
                ParentDepartment = new Domain.Entities.Department
                {
                    Name = x.ParentDepartment.Name
                },
                Company = new Domain.Entities.Company
                {
                    Name = x.Company.Name
                },
                Manager = new Domain.Entities.User
                {
                    Name = x.Manager.Name
                },
                DepartmentCategory = x.DepartmentCategory
            } : request.Select);
        }

        #endregion Employees

        #region Add constraint to ensure TEntity is a type of BaseAuditableEntity

        private Task<IQueryable<TEntity>> HandleQuery<TEntity>(BaseQuery<TEntity> request, CancellationToken cancellationToken, Expression<Func<TEntity, TEntity>>? select = null)
            where TEntity : BaseAuditableEntity // Add the constraint here
        {
            try
            {
                var query = _unitOfWork.Repository<TEntity>().Entities.AsNoTracking();

                // Apply Predicate (filtering)
                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply Ordering
                if (request.OrderByList.Any())
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TEntity>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply Includes (eager loading)
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                // Apply Search Term
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = ApplySearchTerm(query, request.SearchTerm);
                }

                // Apply Select if provided, else return the entity as it is
                if (select is not null)
                    query = query.Select(select);

                return Task.FromResult(query.Adapt<IQueryable<TEntity>>());
            }
            catch (Exception)
            {
                // Return empty IQueryable<TEntity> if there's an exception
                return Task.FromResult(Enumerable.Empty<TEntity>().AsQueryable());
            }
        }

        private IQueryable<TEntity> ApplySearchTerm<TEntity>(IQueryable<TEntity> query, string searchTerm) where TEntity : class
        {
            // This method applies the search term based on the entity type
            if (typeof(TEntity) == typeof(Village))
            {
                var villageQuery = query as IQueryable<Village>;
                return (IQueryable<TEntity>)villageQuery.Where(v =>
                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.District.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.City.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Province.Name, $"%{searchTerm}%"));
            }
            else if (typeof(TEntity) == typeof(District))
            {
                var districtQuery = query as IQueryable<District>;
                return (IQueryable<TEntity>)districtQuery.Where(d => EF.Functions.Like(d.Name, $"%{searchTerm}%"));
            }
            else if (typeof(TEntity) == typeof(District))
            {
                var districtQuery = query as IQueryable<Occupational>;
                return (IQueryable<TEntity>)districtQuery.Where(v =>
                            EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                            EF.Functions.Like(v.Description, $"%{searchTerm}%"));
            }
            else if (typeof(TEntity) == typeof(District))
            {
                var districtQuery = query as IQueryable<Diagnosis>;
                return (IQueryable<TEntity>)districtQuery.Where(v =>
                           EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                           EF.Functions.Like(v.NameInd, $"%{searchTerm}%") ||
                           EF.Functions.Like(v.CronisCategory.Name, $"%{searchTerm}%") ||
                           EF.Functions.Like(v.Code, $"%{searchTerm}%"));
            }

            return query; // No filtering if the type doesn't match
        }

        #endregion Add constraint to ensure TEntity is a type of BaseAuditableEntity
    }
}

// Buat di Copy
// User
//public Task<IQueryable<User>> Handle(GetQueryUser request, CancellationToken cancellationToken)
//{
//    return HandleQuery<User>(request, cancellationToken, request.Select is null ? x => new User
//    {
//        Id = x.Id,
//    } : request.Select);
//}