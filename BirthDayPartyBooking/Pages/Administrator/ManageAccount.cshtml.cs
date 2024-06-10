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

namespace BirthDayPartyBooking.Pages.Administrator
{
    [Authorize(Roles = "Admin")]
    public class ManageAccountModel : PageModel
    {
        private readonly IAccountRepository _accountRepo;

        public ManageAccountModel(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public IList<Account> Account { get; set; }
        [BindProperty]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            Account = await _accountRepo.GetAllAccounts();
        }
        public async Task<IActionResult> OnPostAsync(Guid itemId)
        {
            var item = _accountRepo.GetAccountByAccountId(itemId.ToString());
            if (item == null)
            {
                return NotFound();
            }

            item.DeleteFlag = item.DeleteFlag == 1 ? 0 : 1;
            try
            {
                await _accountRepo.Update(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            Account = await _accountRepo.GetAllAccounts();
            return Page();
        }
        public async Task<IActionResult> OnPostSearchAsync()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                Account = await _accountRepo.GetAccounts(SearchString);
            }
            else
            {
                Account = await _accountRepo.GetAllAccounts();
            }

            return Page();
        }
    }
}
