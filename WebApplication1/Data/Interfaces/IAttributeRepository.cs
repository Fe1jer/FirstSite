using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IAttributeRepository
    {
        Task<Attribute> GetByIdAsync(int productId);
        Task<IReadOnlyList<Attribute>> GetAllAsync();
        Task<IReadOnlyList<Attribute>> GetAllAsync(ISpecification<Attribute> specification);
    }
}
