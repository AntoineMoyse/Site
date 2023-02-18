using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.Categorie;
using Site.Models.Produit;

namespace Site.Areas.Identity.Pages.Account.Manage
{
    public class CommandeProduitViewModel : PageModel
    {
        private readonly SiteContext _context;
        [BindProperty]
        public EveryProduct Product { get; set; }
        public IList<Category> Categorie { get;set; }
        
        public CommandeProduitViewModel(SiteContext context)
        {
            _context=context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Categorie = await _context.Categories.ToListAsync();
            Product = await _context.EveryProduct.FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
