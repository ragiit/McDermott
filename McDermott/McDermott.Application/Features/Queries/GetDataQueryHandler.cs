using McDermott.Application.Features.Commands;
using McDermott.Domain.Common;
using static McDermott.Application.Features.Commands.GetDataCommand;

namespace McDermott.Application.Features.Queries
{
    public class GetDataQueryHandler(IUnitOfWork unitOfWork) :

    #region Transactions

        IRequestHandler<GetQueryGeneralConsultanService, IQueryable<GeneralConsultanService>>,

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
        IRequestHandler<GetQueryDistrict, IQueryable<District>>

    #endregion Configuration

    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        #region Transactions

        // GeneralConsultanService
        public Task<IQueryable<GeneralConsultanService>> Handle(GetQueryGeneralConsultanService request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select);
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

        // Add constraint to ensure TEntity is a type of BaseAuditableEntity
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

            return query; // No filtering if the type doesn't match
        }
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