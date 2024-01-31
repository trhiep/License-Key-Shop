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
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {
                    if (userInf.RoleRoleId != 1 && userInf.IsActive == true)
                    {
                        var roleList = LicenseShopDBContext.INSTANCE.Roles.ToArray();
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
                var depositHistoryList = LicenseShopDBContext.INSTANCE.DepositHistories.ToArray();
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
            var userInf = LicenseShopDBContext.INSTANCE.Users.Find(username);
            if (userInf != null && userInf.IsActive == true && userInf.RoleRoleId == 1)
            {
                float amount = float.Parse(f["amount"]);
                string? useAcc = HttpContext.Session.GetString("userAcc");
                DateTime timeNow = DateTime.Now;

                var userBalInf = LicenseShopDBContext.INSTANCE.UserBalances.Find(username);
                userBalInf.Amount += amount;
                LicenseShopDBContext.INSTANCE.UserBalances.Update(userBalInf);

                BalanceHistory balHistory = new BalanceHistory()
                {
                    UserUsername = username,
                    Status = true,
                    Amount = amount,
                    Reason = "Deposit Successfully.",
                    ChangeDate = timeNow,
                    NewBalance = userBalInf.Amount
                };
                LicenseShopDBContext.INSTANCE.Add(balHistory);

                DepositHistory depoHistoryInf = new DepositHistory()
                {
                    UserUsername = username,
                    Amount = amount,
                    ActionDate = timeNow,
                    ActionBy = useAcc
                };
                LicenseShopDBContext.INSTANCE.DepositHistories.Add(depoHistoryInf);
                LicenseShopDBContext.INSTANCE.SaveChanges();
            }
            return Redirect("/DepositManagement/Index");
        }
    }
}
