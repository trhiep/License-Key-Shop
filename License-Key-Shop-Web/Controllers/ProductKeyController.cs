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
                var keyList = LicenseShopDBContext.INSTANCE.ProductKeys.ToArray();
                ViewBag.keyList = keyList;
                var prdList = LicenseShopDBContext.INSTANCE.Products.ToArray();
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
                var prdList = LicenseShopDBContext.INSTANCE.Products.ToArray();
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
            ProductKey prdKey = new ProductKey()
            {
                ProductProductId = productId,
                ProductKeyValue = keyValue,
                ExpirationDate = expirationDateStr,
                IsExpired = false
            };
            LicenseShopDBContext.INSTANCE.ProductKeys.Add(prdKey);
            LicenseShopDBContext.INSTANCE.SaveChanges();
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
                var prdKey = LicenseShopDBContext.INSTANCE.ProductKeys.Find(Id);
                if (prdKey != null)
                {
                    LicenseShopDBContext.INSTANCE.ProductKeys.Remove(prdKey);
                    LicenseShopDBContext.INSTANCE.SaveChanges();
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
                var prdKeyDetail = LicenseShopDBContext.INSTANCE.ProductKeys.Find(Id);
                ViewBag.prdKeyDetail = prdKeyDetail;
                if (prdKeyDetail != null)
                {
                    var prdDetail = LicenseShopDBContext.INSTANCE.Products.Find(prdKeyDetail.ProductProductId);
                    ViewBag.prdDetail = prdDetail;
                }
                else
                {
                    return RedirectToAction("Index", "ProductKey");
                }
                var prdList = LicenseShopDBContext.INSTANCE.Products.ToArray();
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
            var editingPrdKey = LicenseShopDBContext.INSTANCE.ProductKeys.Find(keyId);
            if (editingPrdKey != null)
            {
                int productId = Int32.Parse(f["productId"]);
                string keyValue = f["keyValue"];
                if (keyValue.Equals(""))
                {
                    keyValue = editingPrdKey.ProductKeyValue;
                }
                string? expirationDate = f["expirationDate"];
                if (expirationDate.Equals(""))
                {
                    expirationDate = null;
                }

                editingPrdKey.ProductProductId = productId;
                editingPrdKey.ExpirationDate = expirationDate;
                editingPrdKey.ProductKeyValue = keyValue;

                LicenseShopDBContext.INSTANCE.ProductKeys.Update(editingPrdKey);
                LicenseShopDBContext.INSTANCE.SaveChanges();
            }
            return RedirectToAction("Index", "ProductKey");
        }
        // ================================================
    }
}
