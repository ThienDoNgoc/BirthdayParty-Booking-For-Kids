using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Repository.IRepo;

namespace BirthDayPartyBooking.Pages.Customer
{
    public class BookingModel : PageModel
    {
        private readonly IAccountRepository accountRepo;
        private readonly IServiceRepository serviceRepo;
        private readonly IPlaceRepository placeRepo;
        private readonly IOrderRepository orderRepo;
        private readonly IOrderDetailRepository orderDetailRepo;
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public Account Customer { get; set; }
        [BindProperty]
        public Account Host { get; set; }
        [BindProperty]
        public IList<OrderDetail> OrderDetails { get; set; }
        [BindProperty]
        public OrderDetail OrderDetail { get; set; }
        [BindProperty]
        public int Number { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public BookingModel(IAccountRepository accountRepo,
                            IServiceRepository serviceRepo, 
                            IPlaceRepository placeRepo, 
                            IOrderRepository orderRepo, 
                            IOrderDetailRepository orderDetailRepo)
        {
            this.placeRepo = placeRepo;
            this.accountRepo = accountRepo;
            this.serviceRepo = serviceRepo;
            this.orderRepo=orderRepo;
            this.orderDetailRepo=orderDetailRepo;
        }

        public IActionResult OnGet()
        {
            Order = new Order();
            if (Id != null)
                HttpContext.Session.SetString("HostId", Id);
            else 
                Id = HttpContext.Session.GetString("HostId");

            string customerId = HttpContext.Session.GetString("UserId");

            Customer = accountRepo.GetAccountByAccountId(customerId);
            Host =  accountRepo.GetAccountByAccountId(Id);
            if (Id != null && serviceRepo != null)
            {
                var services = serviceRepo.GetValidServices(Host.Id);
                if (services != null)
                {
                    ViewData["ServiceId"] = new SelectList(services.Select(p => new
                    {
                        p.Id,
                        NameAndPrice = p.ServiceType?.Name + " - " + p.Name + " - $" + p.Price?.ToString()
                    }), "Id", "NameAndPrice");
                }
            }

            ViewData["PlaceId"] = new SelectList(placeRepo.GetAllPlaceByHostID(Host.Id).Select(p => new
            {   
                p.Id,
                NameAndAddress = p.Name + ", " + p.Address + " - $" + p.Price
            }), "Id", "NameAndAddress");

            var orderDetailsJson = HttpContext.Session.GetString("OrderDetails"+Id);
            OrderDetails = (orderDetailsJson != null) ? JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson)
                : new List<OrderDetail>();

            var totalPrice = (Order.PlaceId==null)?0:(placeRepo.GetAllPlaceByHostID(Host.Id).FirstOrDefault(p => p.Id == Order.Id)).Price;

            foreach(OrderDetail orderDetail in OrderDetails)
            {
                orderDetail.Service = serviceRepo.GetServiceByServiceID(orderDetail.ServiceId.Value);
                var typeId = orderDetail.Service.ServiceTypeId;
                orderDetail.Service.ServiceType = serviceRepo.GetServiceTypeByServiceTypeID(typeId.Value);
                totalPrice += orderDetail.Price.Value;
            }

            OrderDetails = OrderDetails.OrderBy(o => o.Service.ServiceTypeId).ToList();

            Order.TotalPrice = totalPrice;
            return Page();
        }

        public IActionResult OnPostFormOrder()
        {
            if (Id != null)
                HttpContext.Session.SetString("HostId", Id);
            else
                Id = HttpContext.Session.GetString("HostId");
            if(Order.Date > DateTime.Now.AddYears(1))
            {
                TempData["WarningMessage"] = "Please not book so far.";
                return RedirectToPage();
            }
            var check = orderRepo.CheckOrderExist(Order);
           
            if (check)
            {
                TempData["WarningMessage"] = "This place has been booked on this date.";
                return RedirectToPage();
            }
            string customerId = HttpContext.Session.GetString("UserId");
            var orderDetailsJson = HttpContext.Session.GetString("OrderDetails"+Id);
            OrderDetails = (orderDetailsJson != null) ? JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson)
                : new List<OrderDetail>();

            if(OrderDetails.Count == 0)
            {
                TempData["WarningMessage"] = "Pls choose at least 1 service.";
                return RedirectToPage();
            }

            var placePrice = placeRepo.GetAllPlaceByHostID(Guid.Parse(Id)).FirstOrDefault(p => p.Id == Order.PlaceId);
            var totalPrice = (Order.PlaceId == null) ? 0 : placePrice.Price;

            foreach (OrderDetail orderDetail in OrderDetails)
            {
                orderDetail.Service = serviceRepo.GetServiceByServiceID(orderDetail.ServiceId.Value);
                var typeId = orderDetail.Service.ServiceTypeId;
                orderDetail.Service.ServiceType = serviceRepo.GetServiceTypeByServiceTypeID(orderDetail.ServiceId.Value);
                totalPrice += orderDetail.Price.Value;
            }
        
            Customer = accountRepo.GetAccountByAccountId(customerId);
            Order.GuestId = Customer.Id;
            Order.OrderDate = DateTime.Now;
            Order.HostId = Guid.Parse(Id);
            Order.Status = 0;
            Order.TotalPrice = totalPrice;
            Order.DeleteFlag = 0;
            orderRepo.AddNew(Order);           
            
            foreach (OrderDetail orderDetail in OrderDetails)
            {
                orderDetail.OrderId = Order.Id;
                orderDetail.Service = null;
                orderDetailRepo.AddNew(orderDetail);
            }
            HttpContext.Session.Remove("OrderDetails"+Id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostFormDetail()
        {
            if (Id != null)
                HttpContext.Session.SetString("HostId", Id);
            else
                Id = HttpContext.Session.GetString("HostId");
            OrderDetail.Service = serviceRepo.GetServiceByServiceID(OrderDetail.ServiceId.Value);
            var orderDetailsJson = HttpContext.Session.GetString("OrderDetails"+Id);
            OrderDetail.Number = Number;
            OrderDetail.Price = OrderDetail.Service.Price * Number;
            OrderDetails = (orderDetailsJson != null) ? JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson)
                : new List<OrderDetail>();

            var orderDetailToUpdate = OrderDetails.FirstOrDefault(od => od.Service.Id == OrderDetail.Service.Id);

            if (orderDetailToUpdate != null)
            {
                var serviceType = serviceRepo.GetServiceTypeByServiceTypeID(OrderDetail.Service.ServiceTypeId.Value);
                if (serviceType.Name == "Dish")
                {
                    orderDetailToUpdate.Number += OrderDetail.Number;
                    orderDetailToUpdate.Price = OrderDetail.Service.Price * orderDetailToUpdate.Number;
                } 
            }
            else
                OrderDetails.Add(OrderDetail);

            HttpContext.Session.SetString("OrderDetails"+Id, JsonConvert.SerializeObject(OrderDetails));
            return RedirectToPage();
        }

        public IActionResult OnPostClearDetail()
        {
            HttpContext.Session.Remove("OrderDetails"+Id);
            return RedirectToPage("/Customer/Booking");
        }

        public IActionResult OnPostBack()
        {
            HttpContext.Session.Remove("OrderDetails"+Id);
            return RedirectToPage("/Customer/Index");
        }
    }
}
