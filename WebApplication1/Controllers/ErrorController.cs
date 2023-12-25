using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternetShop.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/404")]
        public IActionResult PageNotFound()
        {
        //    var a = Request.GetTypedHeaders().Referer.ToString();
        //    var b = Request.Headers["Referer"].ToString();
        //    string originalPath = "unknown";
        //    if (HttpContext.Items.ContainsKey("originalPath"))
        //    {
        //        originalPath = HttpContext.Items["originalPath"] as string;
        //    }
            return View();
        }
        [Route("/Error/535")]
        public IActionResult SenderMailError()
        {
            return View();
        }
    }
}
