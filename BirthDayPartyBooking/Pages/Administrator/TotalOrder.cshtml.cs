using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Repository.IRepo;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using BusinessObject.Enum;

namespace BirthDayPartyBooking.Pages.Administrator
{
    [Authorize(Roles = "Admin")]
    public class ReportStatisticModel : PageModel
    {
        public readonly IOrderRepository orderRepo;

        public ReportStatisticModel(IOrderRepository repo)
        {
            orderRepo = repo;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        [BindProperty]
        public List<Order> orders { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }
        [BindProperty]
        public int? StatusFilter { get; set; }

        public string[] StatusNames = OrderStatus.StatusNames;

        public async Task OnGetAsync()
        {
            orders = await orderRepo.GetOrderForReport();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                orders = await orderRepo.GetOrderByDate(StartDate, EndDate);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostFilterAsync()
        {
            if (StatusFilter.HasValue)
            {
                orders = await orderRepo.GetOrdersByStatus(StatusFilter.Value);
            }
            else
            {
                orders = await orderRepo.GetOrderForReport();
            }

            return Page();
        }
    }
}
