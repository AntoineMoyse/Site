using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Site.Models.Produit;

namespace Site.Models.Categorie
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name="Nom de la catégorie")]
        [Required(ErrorMessage ="Entrer le nom pour la nouvelle catégorie")]
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Product> Produits { get; set; }
    }
}