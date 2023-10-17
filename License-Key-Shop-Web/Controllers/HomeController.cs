using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
