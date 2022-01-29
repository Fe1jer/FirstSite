using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Services;
using InternetShop.Data.Specifications;
using InternetShop.Data.Specifications.Base;
using InternetShop.ViewModels;
using InternetShop.Data.Repository.Base;

namespace InternetShop.Data.Repository
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly string Path;
        readonly FileService FileService;

        public NewsRepository(AppDBContext appDBContext) : base(appDBContext)
        {
            Path = "\\img\\news\\";
            FileService = new FileService();
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
            if (news.Img != null)
            {
                FileService.DeleteFile(news.Img);
            }
            if (news.FavImg != null)
            {
                FileService.DeleteFile(news.FavImg);
            }
        }

        public new async Task AddAsync(News news)
        {
            await base.AddAsync(news);
        }

        public async Task CreateAsync(CreateNewsViewModel model)
        {
            string path = Path + model.Name.Replace(" ", "_") + "\\";
            News news = new News()
            {
                Name = model.Name,
                CreateData = model.CreateData,
                Img = FileService.UploadFile(model.Img, path + model.Img.FileName),
                Desc = model.Desc
            };
            FileService.ResizeAndCrop(news.Img, 320, 170);
            if (model.IsCaruselNews == true)
            {
                news.FavImg = FileService.UploadFile(model.FavImg, path + model.FavImg.FileName);
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
            string path = Path + model.Name.Replace(" ","_") + "\\";

            news.Name = model.Name;
            if (model.FileImg != null)
            {
                FileService.DeleteFile(news.Img);
                news.Img = FileService.UploadFile(model.FileImg, path + model.FileImg.FileName);
                FileService.ResizeAndCrop(news.Img, 320, 170);
            }
            news.Desc = model.Desc;
            if (model.IsCaruselNews == true)
            {
                if (model.FileFavImg != null)
                {
                    FileService.DeleteFile(news.FavImg);
                    news.FavImg = FileService.UploadFile(model.FileFavImg, path + model.FileFavImg.FileName);
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
    }
}
