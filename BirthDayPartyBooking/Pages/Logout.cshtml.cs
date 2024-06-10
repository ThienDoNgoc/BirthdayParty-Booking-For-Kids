using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BirthDayPartyBooking.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("UserId");
            return RedirectToPage("/Login_Register/Login");
        }
    }
}
