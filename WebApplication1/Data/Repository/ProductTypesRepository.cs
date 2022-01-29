using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Repository.Base;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Data.Repository
{
    public class ProductTypesRepository : Repository<ProductType>, IProductTypesRepository
    {
        public ProductTypesRepository(AppDBContext appDBContext) : base(appDBContext)
        {
        }
        public async Task<ProductType> FindByType(ProductType productType)
        {
            var productsTypes = await GetAllAsync();
            return productsTypes.Where(p => p.Category == productType.Category).Where(p => p.Company == productType.Company)
                .Where(p => p.Name == productType.Name).FirstOrDefault();
        }
    }
}
