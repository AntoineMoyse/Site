using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Models.FAQ;

namespace Site.Pages_Question
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Site.Data.SiteContext _context;

        public IndexModel(Site.Data.SiteContext context)
        {
            _context = context;
        }

        public IList<Question> Question { get;set; }

        public async Task OnGetAsync()
        {
            Question = await _context.Question.ToListAsync();
        }
    }
}
