using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepo;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BirthDayPartyBooking.Pages.Login_Register
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountRepository accountRepo;

        public RegisterModel(IAccountRepository accountRepo)
        {
            this.accountRepo = accountRepo;
        }

        public IActionResult OnGet()
        {
            if (TempData["Account"] != null)
            {
                Account = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>((string)TempData["Account"]);
            }
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(Account.Password != ConfirmPassword)
            {

                TempData["WarningMessage"] = "Your Password not match";
                TempData["Account"] = Newtonsoft.Json.JsonConvert.SerializeObject(Account); // Serialize the Account object to a string
                return RedirectToPage();
            }
            var checkEmail = accountRepo.CheckEmailExist(Account.Email);
            if (checkEmail)
            {
                TempData["WarningMessage"] = "This email has been used!";
                TempData["Account"] = Newtonsoft.Json.JsonConvert.SerializeObject(Account); // Serialize the Account object to a string
                return RedirectToPage();
            }
            Account.DeleteFlag = 0;
            accountRepo.AddNew(Account);

            return RedirectToPage("./Login");
        }
    }
}
