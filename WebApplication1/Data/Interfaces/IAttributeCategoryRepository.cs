using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IAttributeCategoryRepository
    {
        Task<AttributeCategory> GetByIdAsync(int productId);
        Task<IReadOnlyList<AttributeCategory>> GetAllAsync();
        Task<IReadOnlyList<AttributeCategory>> GetAllAsync(ISpecification<AttributeCategory> specification);
    }
}
