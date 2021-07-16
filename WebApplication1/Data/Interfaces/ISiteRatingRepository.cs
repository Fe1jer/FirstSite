using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface ISiteRatingRepository
    {
        Task<IReadOnlyList<SiteRating>> GetAllAsync();
        Task<IReadOnlyList<SiteRating>> GetAllAsync(ISpecification<SiteRating> specification);
        Task<SiteRating> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task AddAsync(SiteRating siteRating);
        Task UpdateAsync(SiteRating siteRating);
        Task<double> OverallSiteRating();
    }
}
