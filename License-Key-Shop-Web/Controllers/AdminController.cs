﻿using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class AdminController : Controller
    {

        public bool CanAccessThisAdminPage()
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
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                if (useAcc != null)
                {
                    LoadUserInf(useAcc);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public void LoadUserInf(string username)
        {
            
        }
    }
}
