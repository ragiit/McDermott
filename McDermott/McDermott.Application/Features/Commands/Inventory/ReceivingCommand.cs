using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class ReceivingCommand
    {
        #region GET Receiving Stock Detail

        public class GetReceivingStockProductQuery(Expression<Func<ReceivingStockProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ReceivingStockProductDto>>
        {
            public Expression<Func<ReceivingStockProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Receiving Stock Detail

        #region GET Receiving Stock

        public class GetReceivingStockQuery(Expression<Func<ReceivingStock, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ReceivingStockDto>>
        {
            public Expression<Func<ReceivingStock, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Receiving Stock

        #region GET Receiving Log
        public class GetReceivingLogQuery(Expression<Func<ReceivingLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ReceivingLogDto>>
        {
            public Expression<Func<ReceivingLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        #endregion

        #region CREATE Receiving Stock Detail

        public class CreateReceivingStockProductRequest(ReceivingStockProductDto ReceivingStockProductDto) : IRequest<ReceivingStockProductDto>
        {
            public ReceivingStockProductDto ReceivingStockProductDto { get; set; } = ReceivingStockProductDto;
        }

        public class CreateListReceivingStockProductRequest(List<ReceivingStockProductDto> ReceivingStockProductDtos) : IRequest<List<ReceivingStockProductDto>>
        {
            public List<ReceivingStockProductDto> ReceivingStockProductDtos { get; set; } = ReceivingStockProductDtos;
        }

        #endregion CREATE Receiving Stock Detail

        #region CREATE Receiving Stock

        public class CreateReceivingStockRequest(ReceivingStockDto ReceivingStockDtos) : IRequest<ReceivingStockDto>
        {
            public ReceivingStockDto ReceivingStockDto { get; set; } = ReceivingStockDtos;
        }

        public class CreateListReceivingStockRequest(List<ReceivingStockDto> ReceivingStockDtos) : IRequest<List<ReceivingStockDto>>
        {
            public List<ReceivingStockDto> ReceivingStockDtos { get; set; } = ReceivingStockDtos;
        }

        #endregion CREATE Receiving Stock

        #region CREATE Receiving log
        public class CreateReceivingLogRequest(ReceivingLogDto ReceivingLogDtos) : IRequest<ReceivingLogDto>
        {
            public ReceivingLogDto ReceivingLogDto { get; set; } = ReceivingLogDtos;
        }

        public class CreateListReceivingLogRequest(List<ReceivingLogDto> ReceivingLogDtos) : IRequest<List<ReceivingLogDto>>
        {
            public List<ReceivingLogDto> ReceivingLogDtos { get; set; } = ReceivingLogDtos;
        }
        #endregion

        #region UPDATE Receiving Stock Product

        public class UpdateReceivingStockProductRequest(ReceivingStockProductDto ReceivingStockProductDto) : IRequest<ReceivingStockProductDto>
        {
            public ReceivingStockProductDto ReceivingStockProductDto { get; set; } = ReceivingStockProductDto;
        }

        public class UpdateListReceivingStockProductRequest(List<ReceivingStockProductDto> ReceivingStockProductDtos) : IRequest<List<ReceivingStockProductDto>>
        {
            public List<ReceivingStockProductDto> ReceivingStockProductDtos { get; set; } = ReceivingStockProductDtos;
        }

        #endregion Update Receiving Stock Product

        #region UPDATE Receiving Stock

        public class UpdateReceivingStockRequest(ReceivingStockDto ReceivingStockDto) : IRequest<ReceivingStockDto>
        {
            public ReceivingStockDto ReceivingStockDto { get; set; } = ReceivingStockDto;
        }

        public class UpdateListReceivingStockRequest(List<ReceivingStockDto> ReceivingStockDtos) : IRequest<List<ReceivingStockDto>>
        {
            public List<ReceivingStockDto> ReceivingStockDtos { get; set; } = ReceivingStockDtos;
        }

        #endregion Update Receiving Stock

        #region UPDATE Receiving Log
        public class UpdateReceivingLogRequest(ReceivingLogDto ReceivingLogDto) : IRequest<ReceivingLogDto>
        {
            public ReceivingLogDto ReceivingLogDto { get; set; } = ReceivingLogDto;
        }

        public class UpdateListReceivingLogRequest(List<ReceivingLogDto> ReceivingLogDtos) : IRequest<List<ReceivingLogDto>>
        {
            public List<ReceivingLogDto> ReceivingLogDtos { get; set; } = ReceivingLogDtos;
        }
        #endregion

        #region DELETE Receiving Stock Product

        public class DeleteReceivingStockPoductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Receiving Stock Product

        #region DELETE Receiving Stock

        public class DeleteReceivingStockRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Receiving Stock

        #region DELETE Receiving Log
        public class DeleteReceivingLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}
