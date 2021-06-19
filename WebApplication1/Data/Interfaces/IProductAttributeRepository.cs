using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IProductAttributeRepository
    {
        Task<ProductAttribute> GetByIdAsync(int productId);
        Task<IReadOnlyList<ProductAttribute>> GetAllAsync();
        Task<IReadOnlyList<ProductAttribute>> GetAllAsync(ISpecification<ProductAttribute> specification);
        Task DeleteListAsync(List<ProductAttribute> productAttributes);
    }
}
