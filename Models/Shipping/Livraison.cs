using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Models.Shipping
{
    public class Livraison
    {
        public int Id { get; set; }
        
        [Display(Name="Nom de la livraison")]
        [Required(ErrorMessage = "Entrer un nom pour la livraison")]
        public string Name { get; set; }

        [Display(Name="Description de la livraison (préciser le temps de livraison et le prix)")]
        [Required(ErrorMessage = "Entrer une description pour la livraison")]
        public string Description { get; set; }

        [Display(Name="Prix de la livraison")]
        [Column(TypeName="decimal(18,2)")]
        [Required(ErrorMessage = "Entrer un prix pour la livraison")]
        public decimal Prix { get; set; }
    }
}