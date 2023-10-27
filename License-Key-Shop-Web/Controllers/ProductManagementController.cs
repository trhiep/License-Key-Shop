using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            var prdList = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.ToArray();
            ViewBag.prdList = prdList;
            var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
            ViewBag.cateList = cateList;
            string currencyUnit = _configuration["CurrencyUnit"];
            ViewBag.currencyUnit = currencyUnit;
            return View();
        }

        public IActionResult Details(int Id)
        {
            var prdDetail = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
            ViewBag.prdDetail = prdDetail;
            var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
            ViewBag.cateList = cateList;
            string currencyUnit = _configuration["CurrencyUnit"];
            ViewBag.currencyUnit = currencyUnit;
            return View();
        }

        public IActionResult Update(int Id)
        {
            var prdDetail = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(Id);
            ViewBag.prdDetail = prdDetail;
            var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
            ViewBag.cateList = cateList;
            string currencyUnit = _configuration["CurrencyUnit"];
            ViewBag.currencyUnit = currencyUnit;
            return View();
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

        public IActionResult Create()
        {
            var cateList = PRN211_FA23_SE1733Context.INSTANCE.CategoryHe173252s.ToArray();
            ViewBag.cateList = cateList;
            return View();
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
    }
}
