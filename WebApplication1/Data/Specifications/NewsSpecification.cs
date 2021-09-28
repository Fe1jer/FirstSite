using InternetShop.Data.Models;
using InternetShop.Data.Specifications.Base;

namespace InternetShop.Data.Specifications
{
    public class NewsSpecification : Specification<News>
    {
        public NewsSpecification() : base() { }

        public new NewsSpecification Take(int count)
        {
            base.Take = count;
            return this;
        }

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
