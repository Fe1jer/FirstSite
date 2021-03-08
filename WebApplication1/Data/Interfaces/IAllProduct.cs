using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Interfaces
{
    public interface IAllProduct
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Product> GetFavProducts { get; }
        Product GetObjectProduct(int carId);
        void CreateProduct(Product product);
        void ChangeProduct(Product product);
        bool ProductAvailability(Product product);
        void DeleteProduct(int id);
    }
}
