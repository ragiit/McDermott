using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Interfaces.Repositories
{
    public interface IComboBoxService
    {
        Task<List<TResult>> QueryGetComboBox<TEntity, TResult>(
        string searchText = "",
        Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class;
    }
}
