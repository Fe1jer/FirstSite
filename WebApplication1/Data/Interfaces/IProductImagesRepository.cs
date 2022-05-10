using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Interfaces
{
    public interface IProductImagesRepository
    {
        Task<ProductImage> GetByIdAsync(int imageId);
        Task<IReadOnlyList<ProductImage>> GetAllAsync();
        Task<IReadOnlyList<ProductImage>> GetAllAsync(ISpecification<ProductImage> specification);
        Task DeleteListAsync(List<ProductImage> productImages);
    }
}
