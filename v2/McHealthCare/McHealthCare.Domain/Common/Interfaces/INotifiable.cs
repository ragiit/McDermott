using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Domain.Common.Interfaces
{
    public interface INotifiable
    {
        string Type { get; }
        object Data { get; }
    }

}
