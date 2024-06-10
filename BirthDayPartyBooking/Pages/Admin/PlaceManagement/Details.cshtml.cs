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

namespace BirthDayPartyBooking.Pages.Admin.PlaceManagement
{
    [Authorize(Roles = "Host")]
    public class DetailsModel : PageModel
    {
        private readonly IPlaceRepository placeRepo;

        public DetailsModel(IPlaceRepository placeRepo)
        {
            this.placeRepo = placeRepo;
        }

        public Place Place { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string Id = HttpContext.Session.GetString("UserId");

            Place = await placeRepo.GetAllPlaceByHostIDAndPlaceID(Id, id.Value);

            if (Place == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
