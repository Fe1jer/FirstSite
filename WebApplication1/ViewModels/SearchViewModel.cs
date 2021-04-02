using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class SearchViewModel
    {
        public ProductsListViewModel ProductsListViewModel { get; set; }
        public string SearchName { get; set; }
    }
}
