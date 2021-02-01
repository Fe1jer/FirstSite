namespace WebApplication1.Data.Models
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string ShortDesc { set; get; }
        public string LongDesc { set; get; }
        public string img { set; get; }
        public string Category { set; get; }
        public string Company { set; get; }
        public string Country { set; get; }
        public ushort Price { set; get; }
        public bool IsFavourite { set; get; }
        public bool Available { set; get; }
    }
}