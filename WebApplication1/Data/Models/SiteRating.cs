using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class SiteRating : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
    }
}
