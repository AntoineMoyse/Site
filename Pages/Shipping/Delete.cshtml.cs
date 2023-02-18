using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Models.Shipping;

namespace Site.Pages.Shipping
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly Site.Data.SiteContext _context;

        public DeleteModel(Site.Data.SiteContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Livraison = await _context.Livraison.FindAsync(id);

            if (Livraison != null)
            {
                _context.Livraison.Remove(Livraison);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
