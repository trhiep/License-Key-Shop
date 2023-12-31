﻿using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.username = "";
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormCollection f)
        {
            String username = f["username"];
            String password = f["password"];
            var urs = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(username);
            if (urs == null)
            {
                ViewBag.loginUsernameErr = "Username does not exists!";
            }
            else
            {
                if (urs.Password == EncryptionMethods.SHA256Encrypt(password))
                {
                    HttpContext.Session.SetString("userAcc", urs.Username);
                    if (urs.RoleRoleId == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ViewBag.loginPasswordErr = "Incorrect password!";
                }

            }
            ViewBag.username = username;
            return View();
        }
    }
}
