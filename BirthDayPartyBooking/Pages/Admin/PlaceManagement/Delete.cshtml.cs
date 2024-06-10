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

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    [Authorize(Roles = "Host")]
    public class DeleteModel : PageModel
    {
        private readonly IPlaceRepository placeRepo;

        public DeleteModel(IPlaceRepository placeRepo)
        {
            this.placeRepo = placeRepo;
        }

        [BindProperty]
        public Place Place { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place = await placeRepo.GetPlaceByPlaceID(id.Value);

            if (Place == null)
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

            Place = await placeRepo.GetPlaceByPlaceID(id.Value);

            if (Place != null)
            {
                await placeRepo.Remove(Place);
            }

            return RedirectToPage("./Index");
        }
    }
}
