using InternetShop.Data.Models;
using System.Threading.Tasks;

namespace InternetShop.Data.Interfaces
{
    public interface IProductTypesRepository
    {
        Task<ProductType> FindByType(ProductType productType);
    }
}
