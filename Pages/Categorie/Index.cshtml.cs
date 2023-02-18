using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Models.Categorie;

namespace Site.Pages_Categorie
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Site.Data.SiteContext _context;

        public IndexModel(Site.Data.SiteContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
