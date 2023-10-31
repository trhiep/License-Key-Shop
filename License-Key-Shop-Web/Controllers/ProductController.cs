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
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                ViewBag.userInf = userInf;
            }
        }
        public IActionResult Index()
        {
            loadUserInf();
            var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.ToList();
            ViewBag.prdList = prdList;
            return View();
        }

        public IActionResult Details(int Id)
        {
            loadUserInf();
            var prdDetails = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
            ViewBag.prdDetails = prdDetails;
            return View();
        }
    }
}
