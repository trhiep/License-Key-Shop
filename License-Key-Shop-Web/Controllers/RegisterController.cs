using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;
using License_Key_Shop_Web.Utils;

namespace License_Key_Shop_Web.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            UserHe173252 userBasicInf = new UserHe173252() { FirstName = "", LastName = "", Username = "", Email = "" };
            ViewBag.basicInf = userBasicInf;
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormCollection f)
        {
            String firstName = f["firstName"];
            String lastName = f["lastName"];
            String username = f["username"];
            String email = f["email"];
            String password = f["password"];
            String confirmPassword = f["confirmPassword"];

            if (confirmPassword.Equals(password))
            {

                var usr = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(username);
                if (usr != null)
                {
                    ViewBag.registerUsernameErr = "This username already exists!";
                }
                else
                {
                    UserHe173252 userInf = new UserHe173252()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Username = username,
                        Email = email,
                        Password = EncryptionMethods.SHA256Encrypt(password),
                        RoleRoleId = 1,
                        IsActive = true,
                        IsVerified = false,
                    };
                    PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Add(userInf);
                    CartHe173252 userCart = new CartHe173252() {
                        UserUsername = username,
                        Total = 0,
                    };
                    PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Add(userCart);
                    UserBalanceHe173252 userBal = new UserBalanceHe173252()
                    {
                        UserUsername = username,
                        Amount = 0,
                    };
                    PRN211_FA23_SE1733Context.INSTANCE.UserBalanceHe173252s.Add(userBal);
                    PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }

            }
            else
            {
                ViewBag.registerPassErr = "Confirm password does not match!";
            }
            UserHe173252 userBasicInf = new UserHe173252()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email,
            };
            ViewBag.basicInf = userBasicInf;
            return View();
        }

    }
}
