using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;
using System.Drawing;

namespace WebApplication1.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> FavProducts { get; set; }
        public IEnumerable<HomeProduct> AutoOnTheCover { get; set; }
    }
}
