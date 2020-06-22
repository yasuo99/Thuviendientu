using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PayPal.Api;
using ThuVienDienTu.Data;
using ThuVienDienTu.Helper;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TopupController : Controller
    {
        private ApplicationDbContext _db;
        Payment payment;
        public TopupController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [ActionName("TopupWithPaypal")]
        public async Task<IActionResult> Topup(int topupAmount)
        {
            APIContext aPIContext = Configuration.GetAPIContext();
            var user = _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            var payerId = Request.Query["PayerID"].ToString();
            if(string.IsNullOrEmpty(payerId))
            {
                string baseUrl = GetRawUrl(Request) + "?";
                var guid = Convert.ToString(new Random().Next(100000));
                var payment = CreatePayment(aPIContext, baseUrl+ "guid="+guid, topupAmount, user);
                string paypalRedirectUrl = "";
                var links = payment.links.GetEnumerator();
                while(links.MoveNext())
                {
                    Links link = links.Current;
                    if (link.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalRedirectUrl = link.href;
                    }
                }
                HttpContext.Session.SetString(guid, payment.id);
                HttpContext.Session.SetString("topupAmount", topupAmount.ToString());
                return Redirect(paypalRedirectUrl);
            }
            else
            {
                var guid = Request.Query["guid"];
                var topup = HttpContext.Session.GetString("topupAmount");
                var executedPayment = ExecutePayment(aPIContext, payerId, HttpContext.Session.GetString(guid));
                if(executedPayment.state.ToLower() != "approved")
                {
                    return View("Failure");
                }
                TopupHistory topupHistory = new TopupHistory()
                {
                    ApplicationUserId = user.Id,
                    TopupAmount = Int32.Parse(topup),
                    TopupDate = DateTime.Now
                };
                user.Balance += Int32.Parse(topup) * 22000;
                _db.ApplicationUsers.Update(user);
                _db.TopupHistories.Add(topupHistory);
                await _db.SaveChangesAsync();
            }
            return View("Successful");
        }
        public static string GetRawUrl(HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
        }
        public Payment CreatePayment(APIContext aPIContext, string redirectUrl, int topupAmount, ApplicationUser user)
        {
            Payer payer = new Payer() { payment_method = "paypal" };
            ItemList itemList = new ItemList()
            {
                items = new List<Item>()
            };
            itemList.items.Add(new Item()
            {
                name = "Nạp tiền vào tài khoản",
                currency = "USD",
                quantity = "1",
                price = topupAmount.ToString()
            });
            Details details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = topupAmount.ToString()
            };
            Amount amount = new Amount()
            {
                currency = "USD",
                details = details,
                total = topupAmount.ToString()
            };
            var transactions = new List<Transaction>();
            transactions.Add(new Transaction() { 
                invoice_number = DateTime.Now.ToString() + user.Id,
                amount = amount,
                item_list = itemList,
                description = "Nạp tiền vào tài khoản"
            });
            var redirectUrls = new RedirectUrls()
            {
                return_url = redirectUrl,
                cancel_url = redirectUrl
            };
            this.payment = new Payment()
            {
                intent = "SALE",
                transactions = transactions,
                payer = payer,
                redirect_urls = redirectUrls
            };
            return this.payment.Create(aPIContext);
        }
        public Payment ExecutePayment(APIContext aPIContext, string payerid, string paymentid)
        {
            PaymentExecution paymentExecution = new PaymentExecution()
            {
                payer_id = payerid,
              
            };
            this.payment = new Payment()
            {
                id = paymentid
            };
            return this.payment.Execute(aPIContext, paymentExecution);

        }
    }
}