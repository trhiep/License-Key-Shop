using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.MyInterface;
using License_Key_Shop_Web.Utils;
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


        public async Task<IActionResult> ResetPassword(string Id)
        {
            string newPass = getRandomPassword();
            var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(Id);
            userInf.Password = EncryptionMethods.SHA256Encrypt(newPass);
            string message = "<p>Hello <strong>"+userInf.FirstName + " " + userInf.LastName+"</strong></p>\r\n<p>This is your new password: <strong>"+ newPass + "</strong></p>";
            await _emailSender.SendEmailAsync(userInf.Email, "Your password is resetted!", message, true);
            PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Update(userInf);
            PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
            return RedirectToAction("Index", "CustomerManagement");
        }

        private string getRandomPassword()
        {

            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&";
            Random random = new Random();
            char[] randomChars = new char[8];

            for (int i = 0; i < 8; i++)
            {
                randomChars[i] = characters[random.Next(characters.Length)];
            }

            return new string(randomChars);
        }
    }
}
