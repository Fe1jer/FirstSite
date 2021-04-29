using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Interfaces
{
    public interface INewsRepository
    {
        Task<IReadOnlyList<News>> GetNewsAsync();
        Task<IReadOnlyList<News>> GetNewsAsync(ISpecification<News> specification);
        Task<News> GetNewsByIdAsync(int newsId);
        Task DeleteNewsAsync(int id);
        Task UpdateNewsAsync(News news);
        Task AddNewsAsync(News news);
        Task CreateNews(CreateNewsViewModel model);
        Task<IReadOnlyList<CaruselItem>> GetFavNewsAsync();
    }
}
