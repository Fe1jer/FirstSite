using System;
using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class News : Entity
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public string Desc { get; set; }
        public string Text { get; set; }
        public string FavImg { get; set; }
        public string Href { get; set; }
        public DateTime CreateData { get; set; }
    }
}
