using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class CaruselItem : Entity
    {
        public string ShortDesc { set; get; }
        public string Name { set; get; }
        public string Img { set; get; }
    }
}
