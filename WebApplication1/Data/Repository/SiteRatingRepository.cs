using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Repository
{
    public class SiteRatingRepository : Repository<SiteRating>, ISiteRatingRepository
    {
        public SiteRatingRepository(AppDBContext appDBContext) : base(appDBContext) { }

        public async Task<double> OverallSiteRating()
        {
            var ratings = await base.GetAllAsync();
            if(ratings.Count != 0)
            {
                return ratings.Average(r => r.Rating);
            }
            return 0;
        }
        public new async Task<IReadOnlyList<SiteRating>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public new async Task<IReadOnlyList<SiteRating>> GetAllAsync(ISpecification<SiteRating> specification)
        {
            return await base.GetAllAsync(specification);
        }

        public new async Task<SiteRating> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            SiteRating siteRating = await base.GetByIdAsync(id);
            await DeleteAsync(siteRating);
        }

        public new async Task AddAsync(SiteRating siteRating)
        {
            await base.AddAsync(siteRating);
        }

        public new async Task UpdateAsync(SiteRating siteRating)
        {
            await base.UpdateAsync(siteRating);
        }
    }
}
