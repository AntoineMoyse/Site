using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Site.Pages.Company
{
    [AllowAnonymous]
    public class LemanModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
