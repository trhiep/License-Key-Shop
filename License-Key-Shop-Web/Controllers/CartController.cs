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

        public IActionResult Delete(int Id)
        {
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    var productInCartOfCustomer = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
                                .Where(cartI => cartI.ProductProductId == Id && cartI.UserUsername.Equals(useAcc)).FirstOrDefault();
                    if (productInCartOfCustomer != null)
                    {
                        PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Remove(productInCartOfCustomer);
                        var cartTotalOfCus = PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Find(useAcc);
                        cartTotalOfCus.Total -= productInCartOfCustomer.ProductProduct.Price * productInCartOfCustomer.Quantity;
                        PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Update(cartTotalOfCus);
                        PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
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
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = PRN211_FA23_SE1733Context.INSTANCE.UserHe173252s.Find(useAcc);
                if (userInf != null)
                {
                    var cartItemList = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
                        .Where(cartI => cartI.UserUsername.Equals(useAcc))
                        .Select(entity => new
                        {
                            ItemId = entity.ItemId,
                            Username = entity.UserUsername,
                            ProductInf = entity.ProductProduct,
                            Quantity = entity.Quantity,
                        });
                    if (cartItemList != null)
                    {
                        var userBal = PRN211_FA23_SE1733Context.INSTANCE.UserBalanceHe173252s.Find(useAcc);
                        string messageHeader = "";
                        string messageBody = "";
                        foreach (var item in cartItemList)
                        {
                            var prdKey = PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s
                                .Where(prdK => prdK.ProductProductId == item.ProductInf.ProductId && prdK.IsExpired == false)
                                .Take(item.Quantity);
                            if (prdKey != null)
                            {
                                OrderHistoryADO.Insert(userInf.Username);
                                var orderHistoryOfCus = PRN211_FA23_SE1733Context.INSTANCE.OrderHistoryHe173252s
                                .Where(ord => ord.UserUsername.Equals(useAcc))
                                .OrderByDescending(x => x.OrderId).FirstOrDefault();

                                foreach (var k in prdKey)
                                {
                                    OrderDetailHe173252 orderDetail = new OrderDetailHe173252()
                                    {
                                        ProductSoldName = k.ProductProduct.ProductName,
                                        OrderHistoryOrderId = orderHistoryOfCus.OrderId,
                                        ProductKey = k.ProductKey,
                                        ExpirationDate = k.ExpirationDate,
                                    };
                                    PRN211_FA23_SE1733Context.INSTANCE.OrderDetailHe173252s.Add(orderDetail);
                                    userBal.Amount -= k.ProductProduct.Price;
                                    BalanceHistoryHe173252 balHis = new BalanceHistoryHe173252()
                                    {
                                        UserUsername = useAcc,
                                        Status = false,
                                        Amount = k.ProductProduct.Price,
                                        Reason = "Buy " + k.ProductProduct.ProductName + ".",
                                        ChangeDate = DateTime.Now,
                                        NewBalance = userBal.Amount
                                    };
                                    messageBody += "<p>Product: " + k.ProductProduct.ProductName + "</p><br>" +
                                        "<p>Product Key: " + k.ProductKey + "</p><br>";
                                    PRN211_FA23_SE1733Context.INSTANCE.ProductKeyHe173252s.Remove(k);


                                    var productInCartOfCustomer = PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s
                                    .Where(cartI => cartI.ProductProductId == item.ProductInf.ProductId && cartI.UserUsername.Equals(useAcc)).FirstOrDefault();
                                    if (productInCartOfCustomer != null)
                                    {
                                        PRN211_FA23_SE1733Context.INSTANCE.CartItemHe173252s.Remove(productInCartOfCustomer);
                                    }
                                }
                            }
                        }
                        var cartTotalOfCus = PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Find(useAcc);
                        cartTotalOfCus.Total = 0;
                        PRN211_FA23_SE1733Context.INSTANCE.CartHe173252s.Update(cartTotalOfCus);
                        PRN211_FA23_SE1733Context.INSTANCE.UserBalanceHe173252s.Update(userBal);
                        PRN211_FA23_SE1733Context.INSTANCE.SaveChanges();
                        await _emailSender.SendEmailAsync(userInf.Email, "Thanks for order", messageHeader + messageBody, true);
                        return RedirectToAction("Index", "Cart");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Cart");
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
