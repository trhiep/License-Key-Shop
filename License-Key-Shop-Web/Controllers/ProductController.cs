using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int Id)
        {
            var prdDetails = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
            ViewBag.prdDetails = prdDetails;
            return View();
        }
    }
}
