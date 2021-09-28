using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
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
