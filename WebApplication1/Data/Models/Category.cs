using System.Collections.Generic;

namespace WebApplication1.Data.Models
{
    public class Category
    {
        public int Id { set; get; }
        public string CategoryName { set; get; }
        public string Desc { set; get; }
        public List<Сompany> Сompany { set; get; }
    }
}