using System.ComponentModel.DataAnnotations;

namespace Site.Models.FAQ
{
    public class Question
    {
        public int Id { get; set; }

        [Display(Name="Question")]
        [Required(ErrorMessage ="Entrer la nouvelle question")]
        public string question { get; set; }

        [Display(Name="Réponse")]
        [Required(ErrorMessage ="Entrer la réponse de la nouvelle question")]
        public string reponse { get; set; }
    }
}