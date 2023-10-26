using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class CategoryManagementController : Controller
    {
        public IActionResult Index()
        {
            var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
            ViewBag.cateList = cateList;
            return View();
        }

        // ================ Create CATEROGY ================
        [HttpPost]
        public IActionResult Create(IFormCollection f)
        {
            string categoryName = f["categoryName"];
            var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
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
                CategoryHe173252 category = new CategoryHe173252() { CategoryName = categoryName };
                PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.Add(category);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
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
            var cate = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.Find(categoryID);
            if (cate != null)
            {
                cate.CategoryName = categoryName;
                PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.Update(cate);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
                TempData["updateCategorySuccess"] = "Update category successfully!";
            }
            else
            {
                TempData["updateCategoryErr"] = "This category does not exist!";
            }
            return RedirectToAction("Index", "CategoryManagement");
        }
        // =================================================
        // ================ Delete CATEROGY ================
        public IActionResult Delete(int Id)
        {
            var cate = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.Find(Id);
            if (cate != null)
            {
                PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.Remove(cate);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
                TempData["deleteCategorySuccess"] = "Delete category successfully!";
            }
            else
            {
                TempData["deleteCategoryErr"] = "This category does not exist!";
            }
            return RedirectToAction("Index", "CategoryManagement");
        }
        // =================================================
    }
}
