using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.MyInterface;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class CustomerManagementController : Controller
    {
        private readonly IEmailSender _emailSender;
        public CustomerManagementController(IEmailSender emailSender)
        {
            this._emailSender = emailSender;
        }

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
                var customerList = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s
                        .Where(rId => rId.RoleRoleId == 1)
                        .Select(entity => new
                        {
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            Username = entity.Username,
                            Email = entity.Email,
                            IsVerified = entity.IsVerified,
                            IsActive = entity.IsActive,
                        });
                ViewBag.customerList = customerList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        
        public async Task<IActionResult> ResetPassword()
        {
            var receiver = "hieptran.pa@gmail.com";
            var subject = "Test mail C#";
            var message = "<p><strong>Hello&nbsp;</strong></p>\r\n<p><em><strong>My name is Hiep</strong></em></p>\r\n<p>Nice to meet you</p>";

            await _emailSender.SendEmailAsync(receiver, subject, message, true);
            return View();
        }
    }
}
