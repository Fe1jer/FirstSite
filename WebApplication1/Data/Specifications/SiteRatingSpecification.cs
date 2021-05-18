using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
{
    public class SiteRatingSpecification : Specification<SiteRating>
    {
        public SiteRatingSpecification() : base() { }

        public SiteRatingSpecification IncludeUser()
        {
            AddInclude(rating => rating.User);
            return this;
        }
    }
}
