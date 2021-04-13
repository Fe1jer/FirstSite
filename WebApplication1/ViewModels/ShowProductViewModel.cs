using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ShowProductViewModel
    {
        public Product Product { get; set; }
        public bool IsInCart { get; set; }
    }
}
