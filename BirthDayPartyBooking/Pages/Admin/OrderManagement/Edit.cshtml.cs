using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Enum;
using Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Admin.OrderManagement
{
    [Authorize(Roles = "Host")]
    public class EditModel : PageModel
    {
        private readonly IOrderRepository orderRepo;
        private readonly IOrderDetailRepository orderDetailRepo;
        public EditModel(IOrderRepository orderRepo, IOrderDetailRepository orderDetailRepo)
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
            OrderDetails = await orderDetailRepo.GetOrderDetailByOrderID(Order.Id);
          
            if (Order == null)
            {
                return NotFound();
            }
            var statusDictionary = OrderStatus.StatusNames
                .Select((name, index) => new { Key = index, Value = name })
                .ToDictionary(item => item.Key, item => item.Value);
            ViewData["Status"] = new SelectList(statusDictionary, "Key", "Value");
            return Page();
        }
            
        public async Task<IActionResult> OnPostAccept()
        {

            Order = await orderRepo.GetOrderByOrderID(Order.Id);
            Order.Status = 1;

            try
            {
                await orderRepo.Update(Order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (orderRepo.GetOrderByOrderID(Order.Id)==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage(new { id = Order.Id });
        }
        public async Task<IActionResult> OnPostReject()
        {

            Order = await orderRepo.GetOrderByOrderID(Order.Id);
            Order.Status = 2;

            try
            {
                await orderRepo.Update(Order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (orderRepo.GetOrderByOrderID(Order.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage(new { id = Order.Id });
        }
        public async Task<IActionResult> OnPostClose()
        {
            Order = await orderRepo.GetOrderByOrderID(Order.Id);
            Order.Status = 3;

            try
            {
                await orderRepo.Update(Order);  
            }
            catch (DbUpdateConcurrencyException)
            {
                if (orderRepo.GetOrderByOrderID(Order.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage(new { id = Order.Id });
        }

    }
}
