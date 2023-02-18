using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Site.Models.ApplicationUser;
using Site.Models.Shipping;

namespace Site.Models.Order
{
    public class Commande
    {
        public int Id { get; set; }

        public IEnumerable<CommandeDetail> CommandeLines { get; set; }
        
        [Display(Name="Votre Adresse")]
        [Required(ErrorMessage ="Entrer votre adresse complète pour éxpédition")]
        public string Adresse { get; set; }

        [Display(Name="Votre Code Postal")]
        [Required(ErrorMessage ="Entrer le code postal pour éxpédition")]
        public string CodePostal { get; set; }

        [Display(Name="Votre Ville")]
        [Required(ErrorMessage ="Entrer la ville pour éxpédition")]
        public string City { get; set; }

        [Display(Name="Votre Pays")]
        [Required(ErrorMessage ="Entrer le pays pour éxpédition")]
        public string Country { get; set; }

        public Livraison Livraison { get; set; }

        [Display(Name="Moyen de livraison")]
        [Required(ErrorMessage ="Choisissez un moyen de livraison")]
        public int LivraisonId { get; set; }
        
        public string etat { get; set; }

        public decimal CommandeTotal { get; set; }

        public DateTime CommandePlaced { get; set; }
        
        public string clientId { get; set; }

        public Client client { get; set; }
    }
}