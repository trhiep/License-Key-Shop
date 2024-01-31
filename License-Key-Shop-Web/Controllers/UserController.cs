using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace License_Key_Shop_Web.Controllers
{
    public class UserController : Controller
    {
        public bool CanAccessThisPage()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {

                    return true;
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
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                ViewBag.userInf = userInf;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public IActionResult ChangePass(IFormCollection f)
        {
            bool canAccess = CanAccessThisPage();
            if (canAccess)
            {
                string oldPass = f["oldPass"];
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (EncryptionMethods.SHA256Encrypt(oldPass).Equals(userInf.Password))
                {
                    string newPass = f["newPass"];
                    string confirmNewPass = f["confirmPass"];
                    if (newPass.Equals(confirmNewPass))
                    {
                        userInf.Password = EncryptionMethods.SHA256Encrypt(newPass);
                        LicenseShopDBContext.INSTANCE.Users.Update(userInf);
                        LicenseShopDBContext.INSTANCE.SaveChanges();
                        TempData["updatePassSuccsess"] = "Change password successfully!";
                    } else
                    {
                        TempData["updatePassErr"] = "Confirm password is not match!";
                    }
                } else
                {
                    TempData["updatePassErr"] = "Old password is incorrect!";
                }

                ViewBag.userInf = userInf;
                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }[HttpPost]
        public IActionResult ChangeInf(IFormCollection f)
        {
            bool canAccess = CanAccessThisPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {
                    string username = f["username"];
                    string firstname = f["firstname"];
                    string lastname = f["lastname"];
                    string email = f["email"];

                    userInf.FirstName = firstname;
                    userInf.LastName = lastname;
                    userInf.Email = email;

                    LicenseShopDBContext.INSTANCE.Update(userInf);
                    LicenseShopDBContext.INSTANCE.SaveChanges();
                    TempData["updateUserInfSuccess"] = "Update user information successfully!";
                }
                ViewBag.userInf = userInf;
                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
