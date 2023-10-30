using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace License_Key_Shop_Web.Controllers
{
    public class ProductKeyController : Controller
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
                var keyList = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.ToArray();
                ViewBag.keyList = keyList;
                var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.ToArray();
                ViewBag.prdList = prdList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // ================ Create Product Key ================
        public IActionResult Create()
        {
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.ToArray();
                ViewBag.prdList = prdList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public IActionResult Create(IFormCollection f)
        {
            int productId = Int32.Parse(f["productId"]);
            string keyValue = f["keyValue"];
            string? expirationDateStr = f["expirationDate"];
            if (expirationDateStr.Equals(""))
            {
                expirationDateStr = null;
            }
            ProductKeyHe173252 prdKey = new ProductKeyHe173252()
            {
                ProductProductId = productId,
                ProductKey = keyValue,
                ExpirationDate = expirationDateStr,
                IsExpired = false
            };
            PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Add(prdKey);
            PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
            TempData["addKeyMsg"] = "Add new key successfully!";
            return Redirect("/ProductKey/Create");
        }
        // =================================================

        // ================ Delete Product =================
        public IActionResult Delete(int Id)
        {
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var prdKey = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Find(Id);
                if (prdKey != null)
                {
                    PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Remove(prdKey);
                    PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
                }
                else
                {
                    TempData["deleteProductKeyErr"] = "This product key does not exist!";
                }
                return RedirectToAction("Index", "ProductKey");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        // =================================================

        // ================ Update Product Key ================
        public IActionResult Update(int Id)
        {
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var prdKeyDetail = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Find(Id);
                ViewBag.prdKeyDetail = prdKeyDetail;
                if (prdKeyDetail != null)
                {
                    var prdDetail = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(prdKeyDetail.ProductProductId);
                    ViewBag.prdDetail = prdDetail;
                }
                else
                {
                    return RedirectToAction("Index", "ProductKey");
                }
                var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.ToArray();
                ViewBag.prdList = prdList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Update(IFormCollection f)
        {
            int keyId = Int32.Parse(f["keyId"]);
            var editingPrdKey = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Find(keyId);
            if (editingPrdKey != null)
            {
                int productId = Int32.Parse(f["productId"]);
                string keyValue = f["keyValue"];
                if (keyValue.Equals(""))
                {
                    keyValue = editingPrdKey.ProductKey;
                }
                string? expirationDate = f["expirationDate"];
                if (expirationDate.Equals(""))
                {
                    expirationDate = null;
                }

                editingPrdKey.ProductProductId = productId;
                editingPrdKey.ExpirationDate = expirationDate;
                editingPrdKey.ProductKey = keyValue;

                PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Update(editingPrdKey);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
            }
            return RedirectToAction("Index", "ProductKey");
        }
        // ================================================
    }
}
