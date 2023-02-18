using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.ApplicationUser;

namespace Site.Pages_User
{
    [Authorize(Roles = "Admin")]
    public class UserDetailsModel : PageModel
    {
        private readonly SiteContext _context;
        public Client Client { get; set; }
        public UserDetailsModel (SiteContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Client = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            return Page();
        }
    }
}
