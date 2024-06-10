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
using Repository.IRepo;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BirthDayPartyBooking.Pages.Admin.ServiceManagement
{
    [Authorize(Roles = "Host")]
    public class EditModel : PageModel
    {
        private readonly IServiceRepository serviceRepo;

        public EditModel(IServiceRepository serviceRepo)
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

            Service = serviceRepo.GetServiceByServiceID(id.Value);

            if (Service == null)
            {
                return NotFound();
            }
            ViewData["ServiceTypeId"] = new SelectList(serviceRepo.GetAllServiceTypes(), "Id", "Name") ;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ServiceTypeId"] = new SelectList(serviceRepo.GetAllServiceTypes(), "Id", "Name");
                return Page();
            }


            try
            {
                await serviceRepo.Update(Service);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (serviceRepo.GetServiceByServiceID(Service.Id)==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
