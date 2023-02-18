using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace Site.Pages.Company
{
    [AllowAnonymous]
    public class ContactModel : PageModel
    {
        private readonly IEmailSender _emailSender;

        [BindProperty, Required(ErrorMessage="Entrez votre nom"), Display(Name = "Votre nom")]
        public string Name { get; set; }
        [BindProperty, EmailAddress, Required(ErrorMessage="Entrez votre adresse mail"), Display(Name = "Votre adresse mail")]
        public string Mail { get; set; }
        [BindProperty, Required(ErrorMessage="Entrez le nom de votre société"), Display(Name = "Votre société")]
        public string Company { get; set; }
        [BindProperty, Required(ErrorMessage="Entrez votre numéro de téléphone"), Display(Name = "Votre numéro de téléphone")]
        public string PhoneNbr { get; set; }
        [BindProperty, Required(ErrorMessage="Entrez votre message"), Display(Name = "Votre message")]
        public string Body { get; set; }

        public ContactModel(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public PageResult ContactSuccess()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var Corps = $@"Monsieur/Madame {Name} chez {Company} joignable au {PhoneNbr} ou par mail {Mail} a envoyé le message suivant : {Body}";              
                await _emailSender.SendEmailAsync("adresse@mail.com", $@"Demande de Contact {Name} de l'entreprise {Company}", Corps);
                return RedirectToPage("ContactSuccess");
            }
            return Page();
        }
    }
}
