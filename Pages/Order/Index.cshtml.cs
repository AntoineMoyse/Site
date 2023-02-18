using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.Order;

namespace Site.Pages_Commande
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SiteContext _context;
        public IList<Commande> commandelist { get; set; }

        [BindProperty]
        public string SelectOption { get; set; }

        [BindProperty]
        public string SearchText { get; set; }

        public IndexModel(SiteContext context)
        {
            _context = context;
        }

        public void OnGet(string option, string text)
        {
            SelectOption = option;
            SearchText = text;

            var commande = from c in _context.Commande select c;

            if (!String.IsNullOrEmpty(SearchText))
            {
                switch(SelectOption)
                {
                    case "Id Commande":
                        commandelist = commande.Where(s => s.Id.ToString()!.Contains(SearchText)).ToListAsync().Result;
                        break;

                    case "Id Client":
                        commandelist = commande.Where(s => s.clientId!.Contains(SearchText)).ToListAsync().Result;
                        break;

                    case "Adresse":
                        commandelist = commande.Where(s => s.Adresse!.Contains(SearchText)).ToListAsync().Result;
                        break;
                }
            }
        }

        public IActionResult OnPostSearch(string SelectOption, string SearchText)
        {
            return RedirectToPage("Index", new { option = SelectOption, text = SearchText});
        }
    }
}
