using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Site.Pages.Company
{
    [AllowAnonymous]
    public class PartenairesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
