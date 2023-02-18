using Site.Data;
using Site.Models.Order;
using Site.Models.Panier;
using System.Collections.Generic;
using System.Linq;

namespace Site.Services
{
    public class CommandeService : ICommande
    {
        private readonly SiteContext _context;
        private readonly Cart _cart;

        public CommandeService(SiteContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public void CreateCommande(Commande commande)
        {
            _context.Add(commande);
            _context.SaveChanges();

            var commandeDetails = new List<CommandeDetail>(_cart.PanierItem.Count());

            foreach (var item in _cart.PanierItem)
            {
                commandeDetails.Add(
                    new CommandeDetail
                    {
                        CommandeId = commande.Id,
                        ProductId = item.Produit.Id,
                        Quantite = item.Quantite,
                        Price = item.Produit.Price,
                        Produit = item.Produit
                    });
                _context.Update(item.Produit);
            }

            _context.CommandeDetails.AddRange(commandeDetails);
            _context.SaveChanges();
        }
    }
}