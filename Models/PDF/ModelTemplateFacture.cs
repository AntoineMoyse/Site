using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Site.Models.Order;
using Site.Models.Produit;
using Site.Models.Shipping;

namespace Site.Models.PDF
{
    public class ModelTemplateFacture
    {
        [BindProperty]
        public Commande Commande { get; set; }
        
        [BindProperty]
        public IList<CommandeDetail> Details { get; set; }

        [BindProperty]
        public IList<EveryProduct> Produits { get; set; }

        [BindProperty]
        public EveryLivraison EveryLivraison { get; set; }
    }
}