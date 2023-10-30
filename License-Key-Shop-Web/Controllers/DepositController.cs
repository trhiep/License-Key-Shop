using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace License_Key_Shop_Web.Controllers
{
    public class DepositController : Controller
    {
        public bool CanAccessThisAdminPage()
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
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    var userBalance = PRN211_FA23_SE1733Context.INSTANCE.UserBalanceHe173252s.Find(useAcc);
                    ViewBag.userBalance = userBalance;
                    var depositHistoryList = PRN211_FA23_SE1733Context.INSTANCE.DepositHistoryHe173252s
                        .Where(deposit => deposit.UserUsername.Equals(useAcc))
                        .Select(entity => new
                        {
                            DepositId = entity.DepositId,
                            UserUsername = entity.UserUsername,
                            Amount = entity.Amount,
                            ActionDate = entity.ActionDate,
                            ActionBy = entity.ActionBy,
                        });
                    ViewBag.depositHistoryList = depositHistoryList;
                }
                ViewBag.userInf = userInf;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
