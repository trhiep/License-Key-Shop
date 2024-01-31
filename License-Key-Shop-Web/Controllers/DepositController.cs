using License_Key_Shop_Web.Models;
using License_Key_Shop_Web.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace License_Key_Shop_Web.Controllers
{
    public class DepositController : Controller
    {
        private readonly IConfiguration _configuration;
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
            bool canAccess = CanAccessThisAdminPage();
            if (canAccess)
            {
                string? useAcc = HttpContext.Session.GetString("userAcc");
                var userInf = LicenseShopDBContext.INSTANCE.Users.Find(useAcc);
                if (userInf != null)
                {
                    var userBalance = LicenseShopDBContext.INSTANCE.UserBalances.Find(useAcc);
                    ViewBag.userBalance = userBalance;
                    var depositHistoryList = LicenseShopDBContext.INSTANCE.DepositHistories
                        .Where(deposit => deposit.UserUsername.Equals(useAcc))
                        .Select(entity => new
                        {
                            DepositId = entity.DepositId,
                            UserUsername = entity.UserUsername,
                            Amount = entity.Amount,
                            ActionDate = entity.ActionDate,
                            ActionBy = entity.ActionBy,
                        });
                    ViewBag.depositHistoryList = depositHistoryList;
                }
                ViewBag.userInf = userInf;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
       
        public string CreatePaymentUrl(int TypePaymentVN, long depositAmount)
        {
            var urlPayment = "";
            //Get Config Info
            string vnp_Returnurl = _configuration["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = _configuration["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)depositAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            DateTime today = DateTime.Today;
            vnpay.AddRequestData("vnp_CreateDate", today.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            string hostName = Dns.GetHostName();
            string IPAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();
            vnpay.AddRequestData("vnp_IpAddr", IPAddress);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "NẠP TIỀN");
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            //vnpay.AddRequestData("vnp_TxnRef", order.Code);

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return urlPayment;
        }
    }
}
