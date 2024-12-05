using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Patient
{
    public class DiseaseHistoryCommand
    {
        #region GET

        public class GetDiseaseHistoryQuery : IRequest<(List<DiseaseHistoryTemp>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<(Expression<Func<DiseaseHistoryTemp, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion GET

    }
}