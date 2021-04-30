using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface INewsRepository
    {
        Task<IReadOnlyList<News>> GetAllAsync();
        Task<IReadOnlyList<News>> GetNewsAsync(ISpecification<News> specification);
        Task<News> GetByIdAsync(int newsId);
        Task DeleteAsync(int id);
        Task UpdateAsync(News news);
        Task AddAsync(News news);
        Task CreateNews(CreateNewsViewModel model);
        Task<IReadOnlyList<CaruselItem>> GetFavNewsAsync();
    }
}
