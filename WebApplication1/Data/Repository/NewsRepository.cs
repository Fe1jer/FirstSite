using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;
using WebApplication1.ViewModels;

namespace WebApplication1.Data.Repository
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly IWebHostEnvironment _appEnvironment;

        public NewsRepository(AppDBContext appDBContext, IWebHostEnvironment appEnvironment) : base(appDBContext)
        {
            _appEnvironment = appEnvironment;
        }

        public async Task<IReadOnlyList<News>> GetNewsAsync()
        {
            return await GetAllAsync();
        }

        public async Task<IReadOnlyList<CaruselItem>> GetFavNewsAsync()
        {
            var news = await GetAllAsync(new NewsSpecification().WhereIsCaruselItem());
            List<CaruselItem> caruselItems = new List<CaruselItem>();

            foreach (News item in news)
            {
                CaruselItem caruselItem = new CaruselItem()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Desc = item.Desc,
                    Href = item.ProductHref,
                    Img = item.FavImg
                };
                caruselItems.Add(caruselItem);
            }

            return caruselItems;
        }

        public async Task<IReadOnlyList<News>> GetNewsAsync(ISpecification<News> specification)
        {
            return await GetAllAsync(specification);
        }

        public async Task<News> GetNewsByIdAsync(int newsId)
        {
            return await GetByIdAsync(newsId);
        }

        public async Task DeleteNewsAsync(int id)
        {
            News news = await GetNewsByIdAsync(id);
            await DeleteAsync(news);
        }

        public async Task UpdateNewsAsync(News news)
        {
            await UpdateAsync(news);
        }

        public async Task AddNewsAsync(News news)
        {
            await AddAsync(news);
        }

        public async Task CreateNews(CreateNewsViewModel model)
        {
            News news = new News()
            {
                Name = model.Name,
                CreateData = model.CreateData,
                Img = await CreateImg(model.Img),
                Desc = model.Desc
            };

            if (model.IsCaruselNews == true)
            {
                news.FavImg = await CreateImg(model.FavImg);
            }
            if (model.IsProductHref == true)
            {
                news.ProductHref = model.ProductHref;
            }
            else
            {
                news.Text = model.Text;
            }

            await AddNewsAsync(news);
        }

        private async Task<string> CreateImg(IFormFile img)
        {
            // путь к папке Files
            string path = "/img/news/" + img.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }

            return path;
        }
    }
}
