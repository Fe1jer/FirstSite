using InternetShop.Data.AbstractClasses;

namespace InternetShop.Data.Models
{
    public class CaruselItem : Entity
    {
        public string Name { set; get; }
        public string Desc { set; get; }
        public string Img { set; get; }
        public string Href { set; get; }
    }
}
