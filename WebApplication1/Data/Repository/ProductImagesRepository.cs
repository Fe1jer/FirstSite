using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Repository.Base;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Repository
{
    public class ProductImagesRepository : Repository<ProductImage>, IProductImagesRepository
    {
        public ProductImagesRepository(AppDBContext appDBContext) : base(appDBContext)
        {
        }

        public new async Task<IReadOnlyList<ProductImage>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<ProductImage> GetByIdAsync(int imageId)
        {
            var images = await GetAllAsync();
            return images.FirstOrDefault(u => u.Id == imageId);
        }

        public new async Task AddAsync(ProductImage productAttribute)
        {
            await base.AddAsync(productAttribute);
        }

        public async Task DeleteListAsync(List<ProductImage> productImages)
        {
            foreach (ProductImage productImage in productImages)
            {
                await DeleteAsync(productImage);
            }
        }
    }
}
