using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class MaintenanceRecordCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetAllMaintenanceRecordQuery(Expression<Func<MaintenanceRecord, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaintenanceRecordDto>>
        {
            public Expression<Func<MaintenanceRecord, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleMaintenanceRecordQuery : IRequest<MaintenanceRecordDto>
        {
            public List<Expression<Func<MaintenanceRecord, object>>> Includes { get; set; }
            public Expression<Func<MaintenanceRecord, bool>> Predicate { get; set; }
            public Expression<Func<MaintenanceRecord, MaintenanceRecord>> Select { get; set; }

            public List<(Expression<Func<MaintenanceRecord, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetMaintenanceRecordQuery : IRequest<(List<MaintenanceRecordDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<MaintenanceRecord, object>>> Includes { get; set; }
            public Expression<Func<MaintenanceRecord, bool>> Predicate { get; set; }
            public Expression<Func<MaintenanceRecord, MaintenanceRecord>> Select { get; set; }

            public List<(Expression<Func<MaintenanceRecord, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateMaintenanceRecordQuery(Expression<Func<MaintenanceRecord, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<MaintenanceRecord, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMaintenanceRecordRequest(MaintenanceRecordDto MaintenanceRecordDto) : IRequest<MaintenanceRecordDto>
        {
            public MaintenanceRecordDto MaintenanceRecordDto { get; set; } = MaintenanceRecordDto;
        }

        public class CreateListMaintenanceRecordRequest(List<MaintenanceRecordDto> GeneralConsultanCPPTDtos) : IRequest<List<MaintenanceRecordDto>>
        {
            public List<MaintenanceRecordDto> MaintenanceRecordDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMaintenanceRecordRequest(MaintenanceRecordDto MaintenanceRecordDto) : IRequest<MaintenanceRecordDto>
        {
            public MaintenanceRecordDto MaintenanceRecordDto { get; set; } = MaintenanceRecordDto;
        }

        public class UpdateListMaintenanceRecordRequest(List<MaintenanceRecordDto> MaintenanceRecordDtos) : IRequest<List<MaintenanceRecordDto>>
        {
            public List<MaintenanceRecordDto> MaintenanceRecordDtos { get; set; } = MaintenanceRecordDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMaintenanceRecordRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
