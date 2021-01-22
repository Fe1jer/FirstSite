using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.Models
{
    public class Car
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string ShortDesc { set; get; }
        public string LongDesc { set; get; }
        public string img { set; get; }
        public ushort Price { set; get; }
        public bool IsFavourite { set; get; }
        public bool Available { set; get; }
        public int CategoryID { set; get; }
        public Category Category{ set; get; }
    }
}