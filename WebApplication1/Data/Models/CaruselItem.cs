using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class CaruselItem : Entity
    {
        public string Name { set; get; }
        public string Desc { set; get; }
        public string Img { set; get; }
        public string Href { set; get; }
    }
}
