using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.MyInterface;
using License_Key_Shop_Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IEmailSender _emailSender;
        public ForgotPasswordController(IEmailSender emailSender)
        {
            this._emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCode(IFormCollection f)
        {
            string email = f["email"];
            var userInf = LicenseShopDBContext
                .INSTANCE.Users
                .Where(x => x.Email.Equals(email))
                .FirstOrDefault();
            if (userInf != null)
            {
                string code = CodeGenerator.GenerateRandomString(6);
                HttpContext.Session.SetString("verificationCode", code);
                await _emailSender.SendEmailAsync(userInf.Email, "Mã xác nhận đặt lại mật khẩu", "Mã xác nhận của bạn là: " + code, true);
                TempData["email"] = email;
                return View();
            }
            else
            {
                TempData["emailNotExistErr"] = "Email không tồn tại!";
                TempData["email"] = email;
                return RedirectToAction("Index", "ForgotPassword");
            }
        }

        public IActionResult SetNewPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetNewPass(IFormCollection f)
        {
            string email = f["email"];
            string enteredCode = f["code"];
            string sentCode = HttpContext.Session.GetString("verificationCode");
            if (sentCode != null)
            {
                if (sentCode.Equals(enteredCode))
                {
                    TempData["email"] = email;
                    return View();
                }
                else
                {
                    TempData["incorrectCodeErr"] = "Mã xác nhận không chính xác!";
                    TempData["email"] = email;
                    return RedirectToAction("GetCode", "ForgotPassword");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult ConfirmNewPassword(IFormCollection f)
        {
            string email = f["email"];
            string newPass = f["newPass"];
            string confirmPass = f["confirmPass"];
            if (newPass.Equals(confirmPass))
            {
                var userInf = LicenseShopDBContext.INSTANCE.Users
                .Where(x => x.Email.Equals(email))
                .FirstOrDefault();
                userInf.Password = EncryptionMethods.SHA256Encrypt(newPass);
                LicenseShopDBContext.INSTANCE.Users.Update(userInf);
                LicenseShopDBContext.INSTANCE.SaveChanges();
                return RedirectToAction("Index", "Login");
            } else
            {
                TempData["confirmPassErr"] = "Nhập lại mật khẩu không chính xác";
                TempData["email"] = email;
                return RedirectToAction("SetNewPass", "ForgotPassword");
            }
        }
    }
}
