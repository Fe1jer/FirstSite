using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Page
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _allProducts;
        private readonly IProductsManager _productFilter;
        private readonly IUserRepository _allUser;
        private readonly IShopCart _shopCart;

        public IndexModel(IShopCart shopCart, IProductRepository iAllProducts, IUserRepository allUser, IProductsManager productFilter)
        {
            _productFilter = productFilter;
            _shopCart = shopCart;
            _allUser = allUser;
            _allProducts = iAllProducts;
        }
    
        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            //throw new Exception("stop");
            return new JsonResult("Hello Response Back");
        }

        public IActionResult OnGetPartial()
        {
            return new PartialViewResult
            {
                ViewName = "_HelloWorldPartial",
                ViewData = this.ViewData
            };
        }

        public ActionResult OnPostAction()
        {
            return new EmptyResult();
        }




        public async Task<JsonResult> OnGetCountProducts(List<string> filters)
        {
            var products = await _allProducts.GetProductsAsync(new ProductSpecification().SortByFavourite());
            products = _productFilter.SortProducts(products.ToList(), filters);
            int count = products.Count();
            return new JsonResult(count);
        }
    }
}
