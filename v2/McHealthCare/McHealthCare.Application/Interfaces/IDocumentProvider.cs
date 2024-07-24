using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Interfaces
{
    public interface IDocumentProvider
    {
        Task<byte[]> GetDocumentAsync(string name, Dictionary<string, string> mergeFields, CancellationToken cancellationToken = default(CancellationToken));
    }
}