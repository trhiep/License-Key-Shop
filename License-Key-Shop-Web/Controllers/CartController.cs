using License_Key_Shop_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace License_Key_Shop_Web.Controllers
{
    public class CartController : Controller
    {
        public bool CanAccessThisAdminPage()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    if (userInf.RoleRoleId == 1 && userInf.IsActive == true)
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
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    var cartItemList = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
                        .Where(cartI => cartI.UserUsername == userInf.Username)
                        .Select(entity => new
                        {
                            ItemId = entity.ItemId,
                            Username = entity.UserUsername,
                            ProductInf = entity.ProductProduct,
                            Quantity = entity.Quantity,
                        });
                    var cartTotal = PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Find(useAcc);
                    ViewBag.cartItemList = cartItemList;
                    ViewBag.cartTotal = cartTotal;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult AddToCart(IFormCollection f)
        {
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                int productId = Int32.Parse(f["productId"]);
                string oldPage = f["currentPage"];
                var cartItem = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
                    .Where(cartI => cartI.ProductProductId == productId && cartI.UserUsername.Equals(useAcc)).FirstOrDefault();
                if (cartItem != null)
                {
                    cartItem.Quantity += 1;
                    PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Update(cartItem);
                }
                else
                {
                    var productToAdd = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(productId);
                    if (productToAdd != null)
                    {
                        CartItemHe173252 newItemInCart = new CartItemHe173252()
                        {
                            UserUsername = useAcc,
                            ProductProductId = productToAdd.ProductId,
                            Quantity = 1,
                        };
                        PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Add(newItemInCart);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                var userCart = PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Find(useAcc);
                if (userCart != null)
                {
                    var addingProduct = PRN211_FA23_SE1733Context.INSTANCE.ProductHe173252s.Find(productId);
                    userCart.Total += addingProduct.Price;
                }
                PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
                return Redirect(oldPage);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
