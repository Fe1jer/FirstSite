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

        public new async Task<IReadOnlyList<News>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<IReadOnlyList<CaruselItem>> GetFavNewsAsync()
        {
            var news = await GetAllAsync(new NewsSpecification().WhereIsCaruselItem().SortById().Take(8));
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

        public new async Task<News> GetByIdAsync(int newsId)
        {
            return await base.GetByIdAsync(newsId);
        }

        public async Task DeleteAsync(int id)
        {
            News news = await GetByIdAsync(id);
            await DeleteAsync(news);
        }

        public new async Task AddAsync(News news)
        {
            await base.AddAsync(news);
        }

        public async Task CreateAsync(CreateNewsViewModel model)
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
            await AddAsync(news);
        }

        public async Task UpdateAsync(ChangeNewsViewModel model)
        {
            News news = await GetByIdAsync(model.Id);

            news.Name = model.Name;
            if (model.FileImg != null)
            {
                DeleteImg(news.Img);
                news.Img = await CreateImg(model.FileImg);
            }
            news.Desc = model.Desc;
            if (model.IsCaruselNews == true)
            {
                if (model.FileFavImg != null)
                {
                    DeleteImg(news.FavImg);
                    news.FavImg = await CreateImg(model.FileFavImg);
                }
            }
            else
            {
                news.FavImg = null;
            }
            if (model.IsProductHref == true)
            {
                news.ProductHref = model.ProductHref;
                news.Text = null;
            }
            else
            {
                news.ProductHref = null;
                news.Text = model.Text;
            }

            await UpdateAsync(news);
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

        private void DeleteImg(string path)
        {
            if (File.Exists($"wwwroot{path}") && path != null)
            {
                File.Delete($"wwwroot{path}");
            }
        }
    }
}
