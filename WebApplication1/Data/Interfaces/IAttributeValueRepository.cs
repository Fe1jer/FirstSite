using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IAttributeValueRepository
    {
        Task<AttributeValue> GetByIdAsync(int productId);
        Task<IReadOnlyList<AttributeValue>> GetAllAsync();
        Task<IReadOnlyList<AttributeValue>> GetAllAsync(ISpecification<AttributeValue> specification);
        Task DeleteListAsync(List<AttributeValue> productAttributes);
    }
}
