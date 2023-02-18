using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Site.Models.Categorie;

namespace Site.Models.Produit
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name="Nom du produit")]
        [Required(ErrorMessage = "Entrer un nom pour le produit")]
        public string Name { get; set; }

        [Display(Name="Courte description")]
        [Required(ErrorMessage = "Renseigner une courte description pour le produit")]
        public string DescriptionCourte { get; set; }

        [Display(Name="Description présente sur la page du produit")]
        [Required(ErrorMessage = "Renseigner une description pour le produit")]
        public string Description { get; set; }

        [Display(Name="Prix")]
        [Column(TypeName="decimal(18,2)")]
        [Required(ErrorMessage ="Donner un prix au produit")]
        public decimal Price { get; set; }

        [Display(Name="Image")]
        public byte[] Image { get; set; }

        [Display(Name="Catégorie")]
        public Category Categorie {  get; set; }
        
        public int CategorieId { get; set; }
    }
}