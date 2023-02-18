using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Models.Shipping;

namespace Site.Pages.Shipping
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Site.Data.SiteContext _context;

        public CreateModel(Site.Data.SiteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Livraison Livraison { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Livraison.Add(Livraison);
            await _context.SaveChangesAsync();

            int id = Livraison.Id;
            _context.EveryLivraisons.Add(
                new EveryLivraison
                {
                    Id = id,
                    Name = Livraison.Name,
                    Description = Livraison.Description,
                    Prix = Livraison.Prix,
                }
            );
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
