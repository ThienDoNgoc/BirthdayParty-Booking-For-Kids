using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Repository.IRepo;

namespace BirthDayPartyBooking.Pages.Customer.Accounts
{
    [Authorize(Roles = "Customer")]
    public class EditModel : PageModel
    {
        private readonly IAccountRepository _accountRepo;

        public EditModel(IAccountRepository repository)
        {
            _accountRepo = repository;
        }

        [BindProperty]
        public Account Account { get; set; }

        public IActionResult OnGetAsync()
        {

            string id = HttpContext.Session.GetString("UserId");

            Account =  _accountRepo.GetAccountByAccountId(id);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _accountRepo.Update(Account);
            }
            catch 
            {
                return BadRequest(ModelState);
            }
            return RedirectToPage("/Customer/Index");
        }
    }
}
