using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications.Base;

namespace WebApplication1.Data.Specifications
{
    public class NewsSpecification : Specification<News>
    {
        public NewsSpecification() : base() { }

        public NewsSpecification WhereIsCaruselItem()
        {
            AddWhere(order => order.FavImg != null);
            return this;
        }

        public NewsSpecification SortById()
        {
            AddDescendingOrdering(order => order.Id);
            return this;
        }
    }
}
