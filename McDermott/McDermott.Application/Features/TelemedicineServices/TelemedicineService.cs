using McDermott.Application.Features.Commands;
using McDermott.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Config.CityCommand;
using static McDermott.Application.Features.TelemedicineServices.TelemedicineServiceCommand;

namespace McDermott.Application.Features.TelemedicineServices
{
    public class TelemedicineServiceCommand
    {
        public class GetUserTelemedicine : IRequest<IQueryable<User>>
        { }
    }

    public class TelemedicineServiceHandler(IUnitOfWork unitOfWork) :
        IRequestHandler<GetUserTelemedicine, IQueryable<User>>
    {
        public Task<IQueryable<User>> Handle(GetUserTelemedicine request, CancellationToken cancellationToken) => Task.FromResult(unitOfWork.Repository<User>().Entities.AsNoTracking().AsQueryable());
    }

    public class TelemedicineService
    {
    }
}