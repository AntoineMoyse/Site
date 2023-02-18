using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.Categorie;
using Site.Models.Produit;

namespace Site.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class Product_secondModel : PageModel
    {
        private readonly SiteContext _context;
        public IList<Product> Produits { get; private set; }
        public IList<Category> Categories { get; private set;}

        public Product_secondModel(SiteContext context)
        {
            _context = context;
        }

        public async Task OnGet()             //Fonction appell� quand on arrive sur la page, permet d'afficher les produits
        {
            Produits = await _context.Product.ToListAsync();
            Categories = await _context.Categories.ToListAsync();
        }
    }
}
