using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class CategoryManagementController : Controller
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
                var cateList = LicenseShopDBContext.INSTANCE.Categories.ToArray();
                ViewBag.cateList = cateList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // ================ Create CATEROGY ================
        [HttpPost]
        public IActionResult Create(IFormCollection f)
        {
            string categoryName = f["categoryName"];
            var cateList = LicenseShopDBContext.INSTANCE.Categories.ToArray();
            Boolean isExisted = false;
            foreach (var item in cateList)
            {
                if (item.CategoryName.ToLower().Equals(categoryName.ToLower()))
                {
                    isExisted = true;
                }
            }

            if (isExisted == true)
            {
                TempData["AddCategoryErr"] = "This category already exist!";
            }
            else
            {
                Category category = new Category() { CategoryName = categoryName };
                LicenseShopDBContext.INSTANCE.Categories.Add(category);
                LicenseShopDBContext.INSTANCE.SaveChanges();
                TempData["AddCategorySuccess"] = "Add new category successfully!";
            }
            return RedirectToAction("Index", "CategoryManagement");
        }
        // =================================================

        // ================ Update CATEROGY ================
        [HttpPost]
        public IActionResult Update(IFormCollection f)
        {
            int categoryID = int.Parse(f["categoryID"]);
            String categoryName = f["categoryName"];
            var cate = LicenseShopDBContext.INSTANCE.Categories.Find(categoryID);
            if (cate != null)
            {
                cate.CategoryName = categoryName;
                LicenseShopDBContext.INSTANCE.Categories.Update(cate);
                LicenseShopDBContext.INSTANCE.SaveChanges();
                TempData["updateCategorySuccess"] = "Update category successfully!";
            }
            else
            {
                TempData["updateCategoryErr"] = "This category does not exist!";
            }
            return RedirectToAction("Index", "CategoryManagement");
        }
    }
}
