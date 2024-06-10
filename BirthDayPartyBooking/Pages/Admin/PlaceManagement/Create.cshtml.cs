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

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    [Authorize(Roles = "Host")]
    public class CreateModel : PageModel
    {
        private readonly IPlaceRepository placeRepo;

        public CreateModel(IPlaceRepository placeRepo)
        {
            this.placeRepo = placeRepo;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Place Place { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Place.HostId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            Place.DeleteFlag = 0;
            await placeRepo.AddNew(Place);

            return RedirectToPage("./Index");
        }
    }
}
