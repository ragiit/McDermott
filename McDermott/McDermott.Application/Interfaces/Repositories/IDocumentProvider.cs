using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Interfaces.Repositories
{
    public interface IDocumentProvider
    {
        Task<byte[]> GetDocumentAsync(string name, Dictionary<string, string> mergeFields, CancellationToken cancellationToken = default(CancellationToken));
    }
}