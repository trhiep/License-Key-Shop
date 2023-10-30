using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class DepositManagementController : Controller
    {
        public bool CanAccessThisManagementPage()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    if (userInf.RoleRoleId != 1 && userInf.IsActive == true)
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
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var depositHistoryList = PRN211_FA23_SE1733Context.INSTANCE.DepositHistoryHe173252s.ToArray();
                ViewBag.depositHistoryList = depositHistoryList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Create(IFormCollection f)
        {
            string username = f["username"];
            var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(username);
            if (userInf != null && userInf.IsActive == true && userInf.RoleRoleId == 1)
            {
                float amount = float.Parse(f["amount"]);
                string? useAcc = HttpContext.Session.GetString("userAcc");
                DateTime timeNow = DateTime.Now;

                var userBalInf = PRN211_FA23_SE1733Context.INSTANCE.UserBalanceHe173252s.Find(username);
                userBalInf.Amount += amount;
                PRN211_FA23_SE1733Context.INSTANCE.UserBalanceHe173252s.Update(userBalInf);

                BalanceHistoryHe173252 balHistory = new BalanceHistoryHe173252()
                {
                    UserUsername = username,
                    Status = true,
                    Amount = amount,
                    Reason = "Deposit Successfully.",
                    ChangeDate = timeNow,
                    NewBalance = userBalInf.Amount
                };
                PRN211_FA23_SE1733Context.INSTANCE.Add(balHistory);

                DepositHistoryHe173252 depoHistoryInf = new DepositHistoryHe173252()
                {
                    UserUsername = username,
                    Amount = amount,
                    ActionDate = timeNow,
                    ActionBy = useAcc
                };
                PRN211_FA23_SE1733Context.INSTANCE.DepositHistoryHe173252s.Add(depoHistoryInf);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
            }
            return Redirect("/DepositManagement/Index");
        }
    }
}
