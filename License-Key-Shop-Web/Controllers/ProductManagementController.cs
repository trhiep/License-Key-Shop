using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Web;

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
                var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.ToArray();
                ViewBag.prdList = prdList;
                var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
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
                var prdDetail = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
                ViewBag.prdDetail = prdDetail;
                var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
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
                var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
                ViewBag.cateList = cateList;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection f)
        {
            var imageFile = HttpContext.Request.Form.Files["file"];

            if (imageFile != null && imageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString();
                string fileExtension = Path.GetExtension(imageFile.FileName);
                string completeFileName = uniqueFileName + fileExtension;
                var filePath = Path.Combine("wwwroot/WebStorage/Images/ProductImages", completeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                int categoryId = Int32.Parse(f["categoryId"]);
                string productName = f["productName"];
                double price = Double.Parse(f["price"]);
                string description = f["description"];


                ProductHe173252 newPrd = new ProductHe173252()
                {
                    CategoryCategoryId = categoryId,
                    ProductName = productName,
                    Price = price,
                    Image = completeFileName,
                    Description = description,
                };
                PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Add(newPrd);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
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
                var prdDetail = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
                ViewBag.prdDetail = prdDetail;
                var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
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
            var editingPrd = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(productId);
            if (editingPrd != null)
            {
                var imageFile = HttpContext.Request.Form.Files["imgFile"];
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString();
                    string fileExtension = Path.GetExtension(imageFile.FileName);
                    string completeFileName = uniqueFileName + fileExtension;
                    var filePath = Path.Combine("wwwroot/WebStorage/Images/ProductImages", completeFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Delete old image
                    var oldfilePath = Path.Combine("wwwroot/WebStorage/Images/ProductImages", editingPrd.Image);
                    if (System.IO.File.Exists(oldfilePath))
                    {
                        System.IO.File.Delete(oldfilePath);
                    }
                    editingPrd.Image = completeFileName;
                }
                int categoryId = Int32.Parse(f["categoryId"]);
                string productName = f["productName"];
                double price = Double.Parse(f["price"]);
                string description = f["description"];

                editingPrd.Price = price;
                editingPrd.Description = description;
                editingPrd.ProductName = productName;
                editingPrd.CategoryCategoryId = categoryId;

                PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Update(editingPrd);
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
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
                var prd = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
                if (prd != null)
                {
                    // Get product key list to delete first
                    var productKetList = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s
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
                            var keyInf = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Find(key.KeyId);
                            if (keyInf != null)
                            {
                                PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Remove(keyInf);
                            }
                        }
                    }

                    // Get product int cart list to delete
                    var productInCartList = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
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
                            var productIncartInf = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Find(cartI.ItemId);
                            if (productIncartInf != null)
                            {
                                PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Remove(productIncartInf);
                            }
                        }
                    }

                    // Delete product
                    PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Remove(prd);
                    PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
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
