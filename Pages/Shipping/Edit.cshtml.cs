using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.Shipping;

namespace Site.Pages.Shipping
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly SiteContext _context;

        public EditModel(SiteContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Livraison).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivraisonExists(Livraison.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            foreach (var livraison in _context.EveryLivraisons)
            {
                if (livraison.Id == Livraison.Id)
                {
                    livraison.Name = Livraison.Name;
                    livraison.Description = Livraison.Description;
                    livraison.Prix = Livraison.Prix;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LivraisonExists(int id)
        {
            return _context.Livraison.Any(e => e.Id == id);
        }
    }
}
