using McDermott.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class TransferStockCommand
    {
        #region GET

        public class GetTransferStockQuery(Expression<Func<TransferStock, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockDto>>
        {
            public Expression<Func<TransferStock, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region GET Product

        public class GetTransferStockProductQuery(Expression<Func<TransferStockProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockProductDto>>
        {
            public Expression<Func<TransferStockProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Product

        #region GET Detail

        public class GetTransferStockDetailQuery(Expression<Func<TransferStockDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockDetailDto>>
        {
            public Expression<Func<TransferStockDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Detail

        #region CREATE

        public class CreateTransferStockRequest(TransferStockDto TransferStockDto) : IRequest<TransferStockDto>
        {
            public TransferStockDto TransferStockDto { get; set; } = TransferStockDto;
        }

        public class CreateListTransferStockRequest(List<TransferStockDto> TransferStockDtos) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockDtos { get; set; } = TransferStockDtos;
        }

        #endregion CREATE

        #region CREATE Product

        public class CreateTransferStockProductRequest(TransferStockProductDto TransferStockProductDto) : IRequest<TransferStockProductDto>
        {
            public TransferStockProductDto TransferStockProductDto { get; set; } = TransferStockProductDto;
        }

        public class CreateListTransferStockProductRequest(List<TransferStockProductDto> TransferStockProductDtos) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductDtos { get; set; } = TransferStockProductDtos;
        }

        #endregion CREATE Product

        #region CREATE Detail

        public class CreateTransferStockDetailRequest(TransferStockDetailDto TransferStockDetailDto) : IRequest<TransferStockDetailDto>
        {
            public TransferStockDetailDto TransferStockDetailDto { get; set; } = TransferStockDetailDto;
        }

        public class CreateListTransferStockDetailRequest(List<TransferStockDetailDto> TransferStockDetailDtos) : IRequest<List<TransferStockDetailDto>>
        {
            public List<TransferStockDetailDto> TransferStockDetailDtos { get; set; } = TransferStockDetailDtos;
        }

        #endregion CREATE Detail

        #region Update

        public class UpdateTransferStockRequest(TransferStockDto TransferStockDto) : IRequest<TransferStockDto>
        {
            public TransferStockDto TransferStockDto { get; set; } = TransferStockDto;
        }

        public class UpdateListTransferStockRequest(List<TransferStockDto> TransferStockDtos) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockDtos { get; set; } = TransferStockDtos;
        }

        #endregion Update        

        #region Update Product

        public class UpdateTransferStockProductRequest(TransferStockProductDto TransferStockProductDto) : IRequest<TransferStockProductDto>
        {
            public TransferStockProductDto TransferStockProductDto { get; set; } = TransferStockProductDto;
        }

        public class UpdateListTransferStockProductRequest(List<TransferStockProductDto> TransferStockProductDtos) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductDtos { get; set; } = TransferStockProductDtos;
        }

        #endregion Update Product

        #region Update Detail

        public class UpdateTransferStockDetailRequest(TransferStockDetailDto TransferStockDetailDto) : IRequest<TransferStockDetailDto>
        {
            public TransferStockDetailDto TransferStockDetailDto { get; set; } = TransferStockDetailDto;
        }

        public class UpdateListTransferStockDetailRequest(List<TransferStockDetailDto> TransferStockDetailDtos) : IRequest<List<TransferStockDetailDto>>
        {
            public List<TransferStockDetailDto> TransferStockDetailDtos { get; set; } = TransferStockDetailDtos;
        }

        #endregion Update Detail

        #region DELETE

        public class DeleteTransferStockRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE        

        #region DELETE Product

        public class DeleteTransferStockProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Product

        #region DELETE Detail

        public class DeleteTransferStockDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Detail
    }
}