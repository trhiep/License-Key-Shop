﻿using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("userAcc"))
            {
                HttpContext.Session.Remove("userAcc");
                HttpContext.Session.Remove("verificationCode");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
