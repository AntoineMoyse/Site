using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.FAQ;

namespace Site.Pages.Company.FAQ
{
    [AllowAnonymous]
    public class FAQModel : PageModel
    {
        public IList<Question> Questions { get; private set; }
        private readonly SiteContext _context;

        public FAQModel (SiteContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            Questions = await _context.Question.ToListAsync();
        }
    }
}
