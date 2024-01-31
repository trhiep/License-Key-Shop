using Humanizer;
using License_Key_Shop_Web.ADO;
using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.MyInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
using System.Linq;

namespace License_Key_Shop_Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IEmailSender _emailSender;
        public CartController(IEmailSender emailSender)
        {
            this._emailSender = emailSender;
        }
        public bool CanAccessThisAdminPage()
        {
            string? useAcc = HttpContext.Session.GetString("userAcc");
            if (useAcc != null)
            {
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {
                    if (userInf.RoleRoleId == 1 && userInf.IsActive == true)
                    {
                        var roleList = LicenseShopDBContext.INSTANCE.Users.ToArray();
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
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {
                    var cartItemList = LicenseShopDBContext.INSTANCE.CartItems
                        .Where(cartI => cartI.UserUsername == userInf.Username)
                        .Select(entity => new
                        {
                            ItemId = entity.ItemId,
                            Username = entity.UserUsername,
                            ProductInf = entity.ProductProduct,
                            Quantity = entity.Quantity,
                        });
                    var cartTotal = LicenseShopDBContext.INSTANCE.Carts.Find(useAcc);
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
                var cartItem = LicenseShopDBContext.INSTANCE.CartItems
                    .Where(cartI => cartI.ProductProductId == productId && cartI.UserUsername.Equals(useAcc)).FirstOrDefault();
                if (cartItem != null)
                {
                    cartItem.Quantity += 1;
                    LicenseShopDBContext.INSTANCE.CartItems.Update(cartItem);
                }
                else
                {
                    var productToAdd = LicenseShopDBContext.INSTANCE.Products.Find(productId);
                    if (productToAdd != null)
                    {
                        CartItem newItemInCart = new CartItem()
                        {
                            UserUsername = useAcc,
                            ProductProductId = productToAdd.ProductId,
                            Quantity = 1,
                        };
                        LicenseShopDBContext.INSTANCE.CartItems.Add(newItemInCart);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                var userCart = LicenseShopDBContext.INSTANCE.Carts.Find(useAcc);
                if (userCart != null)
                {
                    var addingProduct = LicenseShopDBContext.INSTANCE.Products.Find(productId);
                    userCart.Total += addingProduct.Price;
                }
                LicenseShopDBContext.INSTANCE.SaveChanges();
                return Redirect(oldPage);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Delete(int Id)
        {
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {
                    var productInCartOfCustomer = LicenseShopDBContext.INSTANCE.CartItems
                                .Where(cartI => cartI.ProductProductId == Id && cartI.UserUsername.Equals(useAcc)).FirstOrDefault();
                    if (productInCartOfCustomer != null)
                    {
                        LicenseShopDBContext.INSTANCE.CartItems.Remove(productInCartOfCustomer);
                        var cartTotalOfCus = LicenseShopDBContext.INSTANCE.Carts.Find(useAcc);
                        cartTotalOfCus.Total -= productInCartOfCustomer.ProductProduct.Price * productInCartOfCustomer.Quantity;
                        LicenseShopDBContext.INSTANCE.Carts.Update(cartTotalOfCus);
                        LicenseShopDBContext.INSTANCE.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> PlaceOrder()
        {
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
