using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Site.Data;
using Site.Models.ApplicationUser;
using Site.Models.Order;
using Site.Models.PDF;
using Site.Models.Produit;
using Site.Models.Shipping;
using Site.PuppeteerSite;
using Site.Services;

namespace Site.Pages.Order
{
    [Authorize(Roles = "Admin")]
    public class CommandeDetailsModel : PageModel
    {
        private readonly SiteContext _context;
        private readonly UserManager<Client> _client;

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty]
        public Commande Commande { get; set; }

        [BindProperty]
        public IList<CommandeDetail> Details { get; set; }

        [BindProperty]
        public IList<EveryProduct> Products { get; set; }

        [BindProperty]
        public EveryLivraison EveryLivraison { get; set; }

        private readonly IEmailSender _emailSender;
        private readonly ITemplate _templateService;

        public CommandeDetailsModel(SiteContext context, UserManager<Client> client, IEmailSender emailSender, ITemplate templateService)
        {
            _context = context;
            _client = client;
            _emailSender = emailSender;
            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            await getdata(id);

            if(Commande == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string SelectOption)
        {
            await getdata(Commande.Id);
            Commande.etat = SelectOption;
            _context.SaveChanges();
            emailchangementetatcommande(SelectOption);
            return RedirectToPage("./Index");
        }

        public async Task getdata(int? id)
        {
            Products = await _context.EveryProduct.ToListAsync();
            Commande = await _context.Commande.FirstOrDefaultAsync(m => m.Id == id);
            Client = await _context.Users.FirstOrDefaultAsync(c => c.Id == Commande.clientId);
            Details = await _context.CommandeDetails.Where(d => d.CommandeId == Commande.Id).ToListAsync();
            EveryLivraison = await _context.EveryLivraisons.FirstOrDefaultAsync(e => e.Id == Commande.LivraisonId);
        }

        private bool CommandeExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public void emailchangementetatcommande(string etat)
        {
            string Corps;
            if(etat == "Commande payée")
            {
                Corps = $@"Bonjour, il y a du nouveau pour votre commande n°{Commande.Id}
                <div></div>
                Votre commande est passé à l'état <<{etat}>>, cette modification a été mis à jour sur votre compte personnel.
                <div></div>
                Le payement de votre commande à bien été pris en compte par notre équipe et nous faisons tout notre possible pour préparer votre commande.
                <div></div>
                Nous vous recontacterons dès que votre commande sera expédié.
                <div></div>
                A très bientôt !
                <div></div>
                Ce mail a été envoyé automatiquement, merci de ne pas y répondre. Pour toutes questions de votre part, merci de contacter adresse@mail.com";
                _emailSender.SendEmailAsync(Client.Email, $@"Mise à jour de votre commande Mon Site n°{Commande.Id}", Corps);
            }
            else
            {
                if(etat == "Commande expédiée")
                {
                    Corps = $@"Bonjour, il y a du nouveau pour votre commande n°{Commande.Id}
                    <div></div>
                    Votre commande est passé à l'état <<{etat}>>, cette modification a été mis à jour sur votre compte personnel.
                    <div></div>
                    Votre commande a été expédié et devrait arrivé dans les plus bref délais. Merci de votre confiance et de votre commande chez nous !
                    <div></div>
                    A très bientôt !
                    <div></div>
                    Ce mail a été envoyé automatiquement, merci de ne pas y répondre. Pour toutes questions de votre part, merci de contacter adresse@mail.com";
                    _emailSender.SendEmailAsync(Client.Email, $@"Mise à jour sur votre commande Mon Site n°{Commande.Id}", Corps);
                }
            }
        }

        public async Task<IActionResult> OnPostPrintPDF(int? id)
        {
            await getdata(id);
            var model = new ModelTemplateFacture
            {
                Commande = Commande,
                Details = Details,
                Produits = Products,
                EveryLivraison = EveryLivraison
            };
            var html = await _templateService.RenderAsync("Templates/TemplateFacture", model);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = PuppeteerExtensions.ExecutablePath
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });
            return File(pdfContent, "application/pdf", $"Facture-{Commande.Id}.pdf");
        }
    }
}
