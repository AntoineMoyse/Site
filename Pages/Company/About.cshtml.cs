using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Site.Pages.Company
{
    [AllowAnonymous]
    public class AboutModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
