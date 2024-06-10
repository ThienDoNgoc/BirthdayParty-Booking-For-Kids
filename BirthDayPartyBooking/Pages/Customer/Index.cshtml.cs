using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using BusinessObject.Enum;

namespace BirthDayPartyBooking.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class IndexModel : PageModel
    {
        private readonly IAccountRepository accountRepo;

        public IndexModel(IAccountRepository accountRepo)
        {
            this.accountRepo = accountRepo;
        }

        public IList<Account> Hosts { get;set; }

        public async Task OnGetAsync()
        {
            Hosts = await accountRepo.GetAllActiveHosts();
        }
    }
}
