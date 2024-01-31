using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace License_Key_Shop_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                ViewBag.userInf = userInf;
            }

            var random = new Random();

            var allProduct = LicenseShopDBContext.INSTANCE.Products.ToArray();
            var randomProduct = allProduct.OrderBy(r => random.Next()).Take(4).ToList();
            ViewBag.randomProduct = randomProduct;
            string currencyUnit = _configuration["CurrencyUnit"];
            ViewBag.currencyUnit = currencyUnit;
            return View();
        }
    }
}
