using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductFilter
    {
        IEnumerable<string> Categories { get; }
        IEnumerable<string> Countries { get; }
        IEnumerable<string> Companies { get; }
    }
}
