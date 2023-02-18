using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Models.Shipping;

namespace Site.Pages.Shipping
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly Site.Data.SiteContext _context;

        public DetailsModel(Site.Data.SiteContext context)
        {
            _context = context;
        }

        public Livraison Livraison { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Livraison = await _context.Livraison.FirstOrDefaultAsync(m => m.Id == id);

            if (Livraison == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
