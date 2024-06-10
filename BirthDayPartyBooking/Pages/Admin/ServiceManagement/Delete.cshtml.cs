using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    [Authorize(Roles = "Host")]
    public class DeleteModel : PageModel
    {
        private readonly IServiceRepository serviceRepo;

        public DeleteModel(IServiceRepository serviceRepo)
        {
            this.serviceRepo = serviceRepo;
        }

        [BindProperty]
        public Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            string Id = HttpContext.Session.GetString("UserId");

            Service = await serviceRepo.GetServiceByServiceIDAndHostID(id.Value, Id);
            if (Service == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = serviceRepo.GetServiceByServiceID(id.Value);

            if (Service != null)
            {
                Service.DeleteFlag = 1;
                await serviceRepo.Update(Service);
            }

            return RedirectToPage("./Index");
        }
    }
}
