using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.ApplicationUser;
using Site.Models.Order;

namespace Site.Areas.Identity.Pages.Account.Manage
{
    public class CommandeListModel : PageModel
    {
        private readonly SiteContext _context;
        private readonly UserManager<Client> _client;
        public IList<Commande> AllCommandes { get; set; }
        public Client Client { get; set; }
        public CommandeListModel(SiteContext context, UserManager<Client> client)
        {
            _context = context;
            _client = client;
        }
        public async Task OnGetAsync()
        {
            Client = await _client.GetUserAsync(User);
            AllCommandes = await _context.Commande.ToListAsync();
        }
    }
}