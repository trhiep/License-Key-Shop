using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Web;
using System.Xml.Linq;

namespace License_Key_Shop_Web.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductManagementController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
                var prdList = LicenseShopDBContext.INSTANCE.Products.ToArray();
                ViewBag.prdList = prdList;
                var cateList = LicenseShopDBContext.INSTANCE.Categories.ToArray();
                ViewBag.cateList = cateList;
                string currencyUnit = _configuration["CurrencyUnit"];
                ViewBag.currencyUnit = currencyUnit;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // ================ View Detail Product ================
        public IActionResult Details(int Id)
        {
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var prdDetail = LicenseShopDBContext.INSTANCE.Products.Find(Id);
                ViewBag.prdDetail = prdDetail;
                var cateList = LicenseShopDBContext.INSTANCE.Categories.ToArray();
                ViewBag.cateList = cateList;
                string currencyUnit = _configuration["CurrencyUnit"];
                ViewBag.currencyUnit = currencyUnit;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        // ================================================

        // ================ Create Product ================
        public IActionResult Create()
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
        [HttpPost]
        public async Task<ActionResult> Create(IFormCollection f)
        {
            IFormFile imageFile = f.Files["file"];

            if (imageFile != null && imageFile.Length > 0)
            {
                string imgUrl = CloudinaryUploader.ProcessUpload(imageFile);
                // Add information to database
                int categoryId = Int32.Parse(f["categoryId"]);
                string productName = f["productName"];
                double price = Double.Parse(f["price"]);
                string description = f["description"];

                
                Product newPrd = new Product()
                {
                    CategoryCategoryId = categoryId,
                    ProductName = productName,
                    Price = price,
                    Image = imgUrl,
                    Description = description,
                };
                LicenseShopDBContext.INSTANCE.Products.Add(newPrd);
                LicenseShopDBContext.INSTANCE.SaveChanges();
            }
            return Redirect("/ProductManagement/Create");
        }
        // =================================================

        // ================ Update Product ================
        public IActionResult Update(int Id)
        {
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var prdDetail = LicenseShopDBContext.INSTANCE.Products.Find(Id);
                ViewBag.prdDetail = prdDetail;
                var cateList = LicenseShopDBContext.INSTANCE.Categories.ToArray();
                ViewBag.cateList = cateList;
                string currencyUnit = _configuration["CurrencyUnit"];
                ViewBag.currencyUnit = currencyUnit;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(IFormCollection f)
        {
            int productId = Int32.Parse(f["productId"]);
            var editingPrd = LicenseShopDBContext.INSTANCE.Products.Find(productId);
            if (editingPrd != null)
            {
                var imageFile = HttpContext.Request.Form.Files["imgFile"];
                if (imageFile != null && imageFile.Length > 0)
                {
                    string imgUrl = CloudinaryUploader.ProcessUpload(imageFile);
                    // Delete old image
                    // Incomplete
                    editingPrd.Image = imgUrl;
                }
                int categoryId = Int32.Parse(f["categoryId"]);
                string productName = f["productName"];
                double price = Double.Parse(f["price"]);
                string description = f["description"];

                editingPrd.Price = price;
                editingPrd.Description = description;
                editingPrd.ProductName = productName;
                editingPrd.CategoryCategoryId = categoryId;

                LicenseShopDBContext.INSTANCE.Products.Update(editingPrd);
                LicenseShopDBContext.INSTANCE.SaveChanges();
                return Redirect("/ProductManagement");
            }
            else
            {
                return Redirect("ProductManagement/Update/" + productId);
            }

        }
        // ================================================

        // ================ Delete Product =================
        public IActionResult Delete(int Id)
        {
            bool canAccess = CanAccessThisManagementPage();
            if (canAccess)
            {
                var prd = LicenseShopDBContext.INSTANCE.Products.Find(Id);
                if (prd != null)
                {
                    // Get product key list to delete first
                    var productKetList = LicenseShopDBContext.INSTANCE.ProductKeys
                        .Where(key => key.ProductProductId == Id)
                        .Select(entity => new
                        {
                            entity.KeyId,
                        });
                    // Delete key of this product if exist
                    if (productKetList != null)
                    {
                        foreach (var key in productKetList)
                        {
                            var keyInf = LicenseShopDBContext.INSTANCE.ProductKeys.Find(key.KeyId);
                            if (keyInf != null)
                            {
                                LicenseShopDBContext.INSTANCE.ProductKeys.Remove(keyInf);
                            }
                        }
                    }

                    // Get product int cart list to delete
                    var productInCartList = LicenseShopDBContext.INSTANCE.CartItems
                        .Where(cartI => cartI.ProductProductId == Id)
                        .Select(entity => new
                        {
                            entity.ItemId,
                        });
                    // Delete key of this product if exist
                    if (productInCartList != null)
                    {
                        foreach (var cartI in productInCartList)
                        {
                            var productIncartInf = LicenseShopDBContext.INSTANCE.CartItems.Find(cartI.ItemId);
                            if (productIncartInf != null)
                            {
                                LicenseShopDBContext.INSTANCE.CartItems.Remove(productIncartInf);
                            }
                        }
                    }

                    // Delete product
                    LicenseShopDBContext.INSTANCE.Products.Remove(prd);
                    LicenseShopDBContext.INSTANCE.SaveChanges();
                }
                else
                {
                    TempData["deleteProductErr"] = "This product does not exist!";
                }
                return RedirectToAction("Index", "ProductManagement");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        // =================================================
    }
}
