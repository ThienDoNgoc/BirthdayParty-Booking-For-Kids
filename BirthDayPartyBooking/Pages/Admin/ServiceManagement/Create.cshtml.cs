using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    [Authorize(Roles = "Host")]
    public class CreateModel : PageModel
    {
        private readonly IServiceRepository serviceRepo;

        public CreateModel(IServiceRepository serviceRepo)
        {
            this.serviceRepo = serviceRepo;
        }

        public IActionResult OnGet()
        {
       
        ViewData["ServiceTypeId"] = new SelectList(serviceRepo.GetAllServiceTypes(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ServiceTypeId"] = new SelectList(serviceRepo.GetAllServiceTypes(), "Id", "Name");
                return Page();
            }
            Service.HostId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Service.DeleteFlag = 0;
            await serviceRepo.AddNew(Service);

            return RedirectToPage("./Index");
        }
    }
}
