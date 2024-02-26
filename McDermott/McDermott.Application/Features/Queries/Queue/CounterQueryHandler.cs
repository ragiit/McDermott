using McDermott.Application.Dtos.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Transaction.CounterCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class CounterQueryHandler
    {
        internal class GetAllCounterQueryHandler : IRequestHandler<GetCounterQuery, List<CounterDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllCounterQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<CounterDto>> Handle(GetCounterQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Counter>().Entities
                        .Include(x => x.Physician)
                        .Include(x => x.Service)
                        .AsNoTracking()
                        .Select(Counter => Counter.Adapt<CounterDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetCounterByIdQueryHandler : IRequestHandler<GetCounterByIdQuery, CounterDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCounterByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CounterDto> Handle(GetCounterByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Counter>().GetByIdAsync(request.Id);

                return result.Adapt<CounterDto>();
            }
        }

        internal class CreateCounterHandler : IRequestHandler<CreateCounterRequest, CounterDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateCounterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CounterDto> Handle(CreateCounterRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Counter>().AddAsync(request.CounterDto.Adapt<Counter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<CounterDto>();
            }
        }

        internal class UpdateCounterHandler : IRequestHandler<UpdateCounterRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateCounterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateCounterRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Counter>().UpdateAsync(request.CounterDto.Adapt<Counter>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteCounterHandler : IRequestHandler<DeleteCounterRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCounterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCounterRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Counter>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListCounterHandler : IRequestHandler<DeleteListCounterRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListCounterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListCounterRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Counter>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
