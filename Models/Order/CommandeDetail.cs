using Site.Models.Produit;

namespace Site.Models.Order
{
    public class CommandeDetail
    {
        public int Id { get; set; }
        public int CommandeId { get; set; }
        public int ProductId { get ; set; }
        public int Quantite { get; set; }
        public decimal Price { get; set; }
        public virtual Product Produit { get; set; }
        public virtual Commande Commande { get; set; }
    }
}