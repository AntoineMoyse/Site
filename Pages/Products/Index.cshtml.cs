using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.Categorie;
using Site.Models.Produit;

namespace Site.Pages_Products
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SiteContext _context;
        public IList<Category> Categorie { get; set; }
        public IList<Product> Product { get; set; }

        public IndexModel(SiteContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Categorie = await _context.Categories.ToListAsync();
            Product = await _context.Product.ToListAsync();
        }
    }
}
