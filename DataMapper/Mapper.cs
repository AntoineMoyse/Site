using Site.Models.ApplicationUser;
using Site.Models.Order;

namespace Site.DataMapper
{
    public class Mapper
    {
        public Commande CommandeIndexModelToOrder(Commande model, Client client)
        {
            return new Commande
            {
                Adresse = model.Adresse,
                CodePostal = model.CodePostal,
                City = model.City,
                Country = model.Country,
                LivraisonId = model.LivraisonId,
                CommandeTotal = model.CommandeTotal,
                CommandePlaced = model.CommandePlaced,
                clientId = client.Id,
                client = client,
                etat = model.etat
            };
        }
    }
}