using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;
using License_Key_Shop_Web.Utils;

namespace License_Key_Shop_Web.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            User userBasicInf = new User() { FirstName = "", LastName = "", Username = "", Email = "" };
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

                var usr = LicenseShopDBContext.INSTANCE.Users.Find(username);
                if (usr != null)
                {
                    ViewBag.registerUsernameErr = "This username already exists!";
                }
                else
                {
                    User userInf = new User()
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
                    LicenseShopDBContext.INSTANCE.Users.Add(userInf);
                    Cart userCart = new Cart() {
                        UserUsername = username,
                        Total = 0,
                    };
                    LicenseShopDBContext.INSTANCE.Carts.Add(userCart);
                    UserBalance userBal = new UserBalance()
                    {
                        UserUsername = username,
                        Amount = 0,
                    };
                    LicenseShopDBContext.INSTANCE.UserBalances.Add(userBal);
                    LicenseShopDBContext.INSTANCE.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }

            }
            else
            {
                ViewBag.registerPassErr = "Confirm password does not match!";
            }
            User userBasicInf = new User()
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
