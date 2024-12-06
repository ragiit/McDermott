using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands
{
    public class GetDataCommand
    {
        #region Configurations

        public class GetDistrictQuerylable : IRequest<IQueryable<District>>
        {
            public List<Expression<Func<District, object>>>? Includes { get; set; }
            public Expression<Func<District, bool>>? Predicate { get; set; }
            public Expression<Func<District, District>>? Select { get; set; }
            public List<(Expression<Func<District, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false;
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string? SearchTerm { get; set; }
        }

        public class GetVillageQuerylable : IRequest<IQueryable<Village>>
        {
            public List<Expression<Func<Village, object>>>? Includes { get; set; }
            public Expression<Func<Village, bool>>? Predicate { get; set; }
            public Expression<Func<Village, Village>>? Select { get; set; }
            public List<(Expression<Func<Village, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false;
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string? SearchTerm { get; set; }
        }

        #endregion Configurations
    }
}