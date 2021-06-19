using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface IAttributeCategoryRepository
    {
        Task<AttributeCategory> GetByIdAsync(int productId);
        Task<IReadOnlyList<AttributeCategory>> GetAllAsync();
        Task<IReadOnlyList<AttributeCategory>> GetAllAsync(ISpecification<AttributeCategory> specification);
    }
}
