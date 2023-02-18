using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.ApplicationUser;

namespace Site.Pages_User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SiteContext _context;
        public IList<Client> clientlist { get; set; }

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

            var client = from c in _context.Users select c;

            if (!String.IsNullOrEmpty(SearchText))
            {
                switch(SelectOption)
                {
                    case "Id Client":
                        clientlist = client.Where(s => s.Id!.Contains(SearchText)).ToListAsync().Result;
                        break;

                    case "Nom":
                        clientlist = client.Where(s => s.UserName!.Contains(SearchText)).ToListAsync().Result;
                        break;

                    case "Email":
                        clientlist = client.Where(s => s.Email!.Contains(SearchText)).ToListAsync().Result;
                        break;
                    
                    case "Numéro téléphone":
                        clientlist = client.Where(s => s.Email!.Contains(SearchText)).ToListAsync().Result;
                        break;
                }
            }
        }

        public IActionResult OnPostSearch()
        {
            return RedirectToPage("Index", new { option = SelectOption, text = SearchText});
        }
    }
}
