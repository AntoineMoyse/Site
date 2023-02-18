using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Models.Shipping;

namespace Site.Pages.Shipping
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Site.Data.SiteContext _context;

        public IndexModel(Site.Data.SiteContext context)
        {
            _context = context;
        }

        public IList<Livraison> Livraison { get;set; }

        public async Task OnGetAsync()
        {
            Livraison = await _context.Livraison.ToListAsync();
        }
    }
}
