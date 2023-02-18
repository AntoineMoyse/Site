using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Site.Data;
using Site.DataMapper;
using Site.Models.ApplicationUser;
using Site.Models.Order;
using Site.Models.Panier;
using Site.Models.PDF;
using Site.Models.Produit;
using Site.PuppeteerSite;
using Site.Services;
using Site.Models.Shipping;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Site.Pages.Order
{
    [Authorize(Roles = "Admin")]
    public class CommandeModel : PageModel
    {
        [BindProperty]
        public Commande Commande { get; set; }
        public IList<CommandeDetail> Details { get; set; }
        public IList<EveryProduct> Products { get; set; }
        public IEnumerable<CartItem> Cart { get; set; }
        public EveryLivraison EveryLivraison { get; set; }
        public IList<Livraison> Livraisons { get; set; }
        public List<SelectListItem> Options { get; set; }
        private readonly ICommande _commandeService;
        private readonly Cart _cart;
        private readonly Mapper _mapper;
        private static UserManager<Client> _client;
        private readonly ITemplate _templateService;
        private readonly SiteContext _context;


        public CommandeModel(ICommande commandeService, Cart cart, UserManager<Client> client, ITemplate templateService, SiteContext context)
        {
            _commandeService = commandeService;
            _cart = cart;
            _client = client;
            _mapper = new Mapper();
            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            Cart = this._cart.GetShoppingCartItems();
            Livraisons = await _context.Livraison.ToListAsync();
            Options = _context.Livraison.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Cart = this._cart.GetShoppingCartItems();
            Livraisons = await _context.Livraison.ToListAsync();
            Options = _context.Livraison.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();
            var items = _cart.GetShoppingCartItems();
            _cart.PanierItem = items;

            if (ModelState.IsValid)
            {
                var userId = _client.GetUserId(User);
                var user = await _client.FindByIdAsync(userId);
                Commande.CommandeTotal = items.Sum(item => item.Quantite * item.Produit.Price)+GetLivraisonPrix(Commande.LivraisonId);
                Commande.etat = "Commande enregistrée";
                Commande.CommandePlaced = DateTime.Now;
                var order = _mapper.CommandeIndexModelToOrder(Commande, user);
                _commandeService.CreateCommande(order);
                _cart.ClearCart();
                await mailcommandesuccess(order, user);
                return RedirectToPage("CommandeSuccess");
            }
            
            return Page();
        }

        public async Task getdata(Commande commande)
        {
            Details = await _context.CommandeDetails.Where(d => d.CommandeId == commande.Id).ToListAsync();
            Products = await _context.EveryProduct.ToListAsync();
            EveryLivraison = await _context.EveryLivraisons.FirstOrDefaultAsync(e => e.Id == commande.LivraisonId);
        }

        public decimal GetLivraisonPrix(int? id)
        {
            return _context.Livraison.FirstOrDefault(l => l.Id == id).Prix;
        }

        private async Task mailcommandesuccess(Commande order, Client user)
        {
            await getdata(order);
            var model = new ModelTemplateFacture
            {
                Commande = order,
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

            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Support Mon Site","adresse@mail.com"));
            Message.To.Add(new MailboxAddress(user.Email, user.Email));
            Message.Subject = $"Reçu Mon Site commande {order.Id}";

            var builder = new BodyBuilder();

            builder.HtmlBody = @$"Merci pour votre commande !
            <div></div>
            Votre commande numéro {order.Id} à bien été pris en compte dans notre système. Nous vous invitons à prendre contact avec nous pour règler votre facture se trouvant en pièce jointe.
            Une fois la commande payée, nous préparerons votre commande pour pouvoir l'expédier dans les plus bref délais.

            <div></div>
            A très bientôt !
            <div></div>
            Ce mail a été envoyé automatiquement, merci de ne pas y répondre. Pour toutes question de votre part, contactez adresse@mail.com";
            var attachment = (MimePart) builder.Attachments.Add($"Facture-{order.Id}.pdf", pdfContent);
            
            Message.Body = builder.ToMessageBody();
            
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.monsmtp.fr", 000, false);
                smtpClient.Authenticate("adresse@mail.com", "monMDP");
                await smtpClient.SendAsync(Message);
                smtpClient.Disconnect(true);
            }
        }
    }
}
