using Site.Models.Order;

namespace Site.Services
{
    public interface ICommande
    {
        void CreateCommande(Commande commande);
    }
}