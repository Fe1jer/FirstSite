using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;
using InternetShop.ViewModels;

namespace InternetShop.Data.Interfaces
{
    public interface INewsRepository
    {
        Task<IReadOnlyList<News>> GetAllAsync();
        Task<IReadOnlyList<News>> GetNewsAsync(ISpecification<News> specification);
        Task<News> GetByIdAsync(int newsId);
        Task DeleteAsync(int id);
        Task UpdateAsync(ChangeNewsViewModel news);
        Task AddAsync(News news);
        Task CreateAsync(CreateNewsViewModel model);
        Task<IReadOnlyList<CaruselItem>> GetFavNewsAsync();
    }
}
