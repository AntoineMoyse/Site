using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Site.Models.Order;

namespace Site.Models.ApplicationUser
{
    public class Client : IdentityUser
    {
        public IEnumerable<Commande> Commandes { get; set; }
    }
}