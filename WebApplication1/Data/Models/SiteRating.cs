using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class SiteRating : Entity
    {
        public User User { get; set; }
        public int Rating { get; set; }
    }
}
