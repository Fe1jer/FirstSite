using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IProductAttributeRepository
    {
        Task<ProductAttribute> GetByIdAsync(int productId);
        Task<IReadOnlyList<ProductAttribute>> GetAllAsync();
        Task<IReadOnlyList<ProductAttribute>> GetAllAsync(ISpecification<ProductAttribute> specification);
        Task DeleteListAsync(List<ProductAttribute> productAttributes);
    }
}
