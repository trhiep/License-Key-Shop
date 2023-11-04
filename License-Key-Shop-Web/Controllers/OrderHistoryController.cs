using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace License_Key_Shop_Web.Controllers
{
    public class OrderHistoryController : Controller
    {
        public bool CanAccessThisPage()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    if (userInf.RoleRoleId == 1 && userInf.IsActive == true)
                    {
                        var roleList = PRN211_FA23_SE1733Context.INSTANCE.RoleHe173252s.ToArray();
                        ViewBag.userInf = userInf;
                        ViewBag.roleList = roleList;
                        return true;
                    }
                }
            }
            if (HttpContext.Session.Keys.Contains("userAcc"))
            {
                HttpContext.Session.Remove("userAcc");
            }
            return false;
        }
        public IActionResult Index()
        {
            bool canAccess = CanAccessThisPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    var orderHis = PRN211_FA23_SE1733Context.INSTANCE.OrderHistoryHe173252s
                        .Where(order => order.UserUsername == userInf.Username)
                        .Select(entity => new
                        {
                            OrderId = entity.OrderId,
                            UserUsername = entity.UserUsername,
                            OrderDetailHe173252 = entity.OrderDetailHe173252s
                        })
                        .OrderByDescending(ord => ord.OrderId);
                    ViewBag.orderHis = orderHis;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
