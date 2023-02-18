using Site.Models.Produit;

namespace Site.Models.Panier
{
    public class CartItem
    {
        public int Id { get; set; }
        public Product Produit { get; set; }
        public int Quantite { get; set; }
        public string PanierId { get; set; }
    }
}