using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

namespace Site.Areas.Identity.Pages.Account.Manage
{
    public class CommandeDetailsModel : PageModel
    {
        private readonly SiteContext _context;
        private readonly UserManager<Client> _client;
        public Client Client { get; set; }
        public Commande Commande { get; set; }
        public IList<CommandeDetail> Details { get; set; }
        public IList<EveryProduct> Products { get; set; }
        public EveryLivraison EveryLivraison { get; set; }
        private readonly ITemplate _templateService;

        public CommandeDetailsModel(SiteContext context, UserManager<Client> client, ITemplate templateService)
        {
            _context = context;
            _client = client;
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

        public async Task getdata(int? id)
        {
            Products = await _context.EveryProduct.ToListAsync();
            Commande = await _context.Commande.FirstOrDefaultAsync(m => m.Id == id);
            Client = await _client.GetUserAsync(User);
            Details = await _context.CommandeDetails.Where(d => d.CommandeId == Commande.Id).ToListAsync();
            EveryLivraison = await _context.EveryLivraisons.FirstOrDefaultAsync(e => e.Id == Commande.LivraisonId);
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
