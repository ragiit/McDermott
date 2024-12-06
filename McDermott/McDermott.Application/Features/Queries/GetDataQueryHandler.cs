using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Config.VillageCommand;
using static McDermott.Application.Features.Commands.GetDataCommand;

namespace McDermott.Application.Features.Queries
{
    public class GetDataQueryHandler(IUnitOfWork _unitOfWork) :
        IRequestHandler<GetVillageQuerylable, IQueryable<Village>>
    {
        public Task<IQueryable<District>> Handle(GetDistrictQuerylable request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<District>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<District>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<District>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v => EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new District
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
                    });

                return Task.FromResult(query.Adapt<IQueryable<District>>());
            }
            catch (Exception)
            {
                return Task.FromResult(Enumerable.Empty<District>().AsQueryable());
            }
        }

        public Task<IQueryable<Village>> Handle(GetVillageQuerylable request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Village>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<Village>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Village>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.District.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.City.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Village
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
                    });

                return Task.FromResult(query.Adapt<IQueryable<Village>>());
            }
            catch (Exception)
            {
                // Return empty IQueryable<Village> if there is an exception
                return Task.FromResult(Enumerable.Empty<Village>().AsQueryable());
            }
        }
    }
}