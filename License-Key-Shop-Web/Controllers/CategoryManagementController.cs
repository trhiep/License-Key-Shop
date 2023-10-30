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
                var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
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
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var cate = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.Find(Id);
                if (cate != null)
                {
                    var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s
                        .Where(prdoduct => prdoduct.CategoryCategoryId == Id)
                        .Select(entity => new
                        {
                            entity.ProductId,
                        });
                    foreach (var prd in prdList)
                    {
                        if (prd != null)
                        {
                            // Get product key list to delete first
                            var productKetList = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s
                                .Where(key => key.ProductProductId == prd.ProductId)
                                .Select(entity => new
                                {
                                    entity.KeyId,
                                });
                            // Delete key of this product if exist
                            if (productKetList != null)
                            {
                                foreach (var key in productKetList)
                                {
                                    var keyInf = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Find(key.KeyId);
                                    if (keyInf != null)
                                    {
                                        PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Remove(keyInf);
                                    }
                                }
                            }

                            // Get product int cart list to delete
                            var productInCartList = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
                                .Where(cartI => cartI.ProductProductId == prd.ProductId)
                                .Select(entity => new
                                {
                                    entity.ItemId,
                                });
                            // Delete key of this product if exist
                            if (productInCartList != null)
                            {
                                foreach (var cartI in productInCartList)
                                {
                                    var productIncartInf = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Find(cartI.ItemId);
                                    if (productIncartInf != null)
                                    {
                                        PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Remove(productIncartInf);
                                    }
                                }
                            }

                            // Delete product
                            var productToRemove = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(prd.ProductId);
                            if (productToRemove != null)
                            {
                                PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Remove(productToRemove);
                            }
                        }
                    }
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
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        // =================================================
    }
}
