using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepo;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BusinessObject.ReportData;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Administrator
{
    [Authorize(Roles = "Admin")]
    public class StatisticModel : PageModel
    {
        public readonly IOrderRepository orderRepo;

        public StatisticModel(IOrderRepository repo)
        {
            orderRepo = repo;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        [BindProperty]
        public List<HostOrderSummary> ordersSumary { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public async Task OnGetAsync()
        {
            ordersSumary = await orderRepo.GetHostOrderSummaries();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ordersSumary = await orderRepo.GetHostOrderSummariesByDate(StartDate, EndDate);
            }

            return Page();
        }
    }
}
