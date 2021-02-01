using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;

namespace WebApplication1.Data.Repository
{
    public class ProductFilterRepository : IProductFilter
    {
        private readonly AppDBContext appDBContent;

        public ProductFilterRepository(AppDBContext appDBContent)
        {
            this.appDBContent = appDBContent;
        }

        public IEnumerable<string> Categories { get => (appDBContent.Product.Select(i => i.Category)).Distinct().ToArray();  }
        public IEnumerable<string> Countries { get => (appDBContent.Product.Select(i => i.Country)).Distinct().ToArray(); }
        public IEnumerable<string> Companies { get => (appDBContent.Product.Select(i => i.Company)).Distinct().ToArray(); }
    }
}
