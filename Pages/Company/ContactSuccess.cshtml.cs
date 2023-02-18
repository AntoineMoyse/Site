using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Site.Pages.Company
{
    [AllowAnonymous]
    public class ContactSuccessModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
