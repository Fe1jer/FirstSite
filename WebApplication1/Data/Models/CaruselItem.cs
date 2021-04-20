using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class CaruselItem : Entity
    {
        public string ShortDesc { set; get; }
        public string LongDesc { set; get; }
        public string Img { set; get; }
    }
}
