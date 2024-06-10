using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BirthDayPartyBooking.Pages.Admin.OrderManagement
{
    [Authorize(Roles = "Host")]

    public class CustomerDetailModel : PageModel
    {
        private readonly IAccountRepository accountRepo;

        public CustomerDetailModel(IAccountRepository accountRepo)
        {
            this.accountRepo = accountRepo;
        }

        public Account account { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            account = accountRepo.GetAccountByAccountId(id.ToString());

            if (account == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
