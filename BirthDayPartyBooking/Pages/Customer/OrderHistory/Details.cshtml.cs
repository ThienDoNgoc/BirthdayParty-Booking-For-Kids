using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Customer.OrderHistory
{
    [Authorize(Roles = "Customer")]
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository orderRepo;
        private readonly IOrderDetailRepository orderDetailRepo;


        public DetailsModel(IOrderRepository orderRepo, IOrderDetailRepository orderDetailRepo)
        {
            this.orderRepo = orderRepo; 
            this.orderDetailRepo = orderDetailRepo;
        }

        [BindProperty]
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await orderRepo.GetOrderByOrderID(id.Value);
            OrderDetails =await orderDetailRepo.GetOrderDetailByOrderID(Order.Id);

            return Page();
        }

        public IActionResult OnPostPay()
        {
            return RedirectToPage("/Customer/Payment/PayBooking", new {id=Order.Id});
        }

        public async Task<IActionResult> OnPostCancel ()
        {
            Order = await orderRepo.GetOrderByOrderID(Order.Id);
            Order.Status = 5;
            await orderRepo.Update(Order);
            return RedirectToPage("./Index");
        }

    }
}
