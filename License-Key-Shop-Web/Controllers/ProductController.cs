using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class ProductController : Controller
    {
        public void loadUserInf()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                ViewBag.userInf = userInf;
            }
        }
        public IActionResult Index()
        {
            loadUserInf();
            var prdList = LicenseShopDBContext.INSTANCE.Products.ToList();
            ViewBag.prdList = prdList;
            return View();
        }

        public IActionResult Details(int Id)
        {
            loadUserInf();
            var prdDetails = LicenseShopDBContext.INSTANCE.Products.Find(Id);
            ViewBag.prdDetails = prdDetails;
            return View();
        }
    }
}
