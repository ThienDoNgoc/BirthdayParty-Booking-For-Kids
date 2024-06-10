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
using BusinessObject.Enum;
using Repository.IRepo;

namespace BirthDayPartyBooking.Pages.Admin.Accounts
{
    [Authorize(Roles = "Host")]
    public class EditModel : PageModel
    {
        private readonly IAccountRepository accountRepo;

        public EditModel(IAccountRepository accountRepository)
        {
            accountRepo = accountRepository;
        }

        [BindProperty]
        public Account Account { get; set; }

        public IActionResult OnGetAsync()
        {

            string id = HttpContext.Session.GetString("UserId");

            Account = accountRepo.GetAccountByAccountId(id);

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
                await accountRepo.Update(Account);
            }
            catch
            {
                return BadRequest(ModelState);
            }

            return RedirectToPage("/Admin/Accounts/Edit");
        }

    }
}
