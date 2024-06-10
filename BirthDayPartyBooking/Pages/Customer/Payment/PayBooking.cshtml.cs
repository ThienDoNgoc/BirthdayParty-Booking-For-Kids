using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BirthDayPartyBooking.Pages.Customer.Payment
{
    [Authorize(Roles = "Customer")]
    public class PayBookingModel : PageModel
    {
        private readonly BirthdayPartyBookingContext _context;

        public PayBookingModel(BirthdayPartyBookingContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(Guid? id)
        {
            string vnp_Returnurl = "https://localhost:44304/Customer/Payment/ReturnPay";
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string vnp_TmnCode = "SF27X1PR";
            string vnp_HashSecret = "YNUDIRUGCVFYUHTPAKZTREPIYHRHPFSI";

            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);

            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.TotalPrice * 100 * 23000).ToString());

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(HttpContext));

            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Id);
            vnpay.AddRequestData("vnp_OrderType", "other");

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString() + DateTime.Now.Ticks);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            Response.Redirect(paymentUrl);
        }
    }
}
