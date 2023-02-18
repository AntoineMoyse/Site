using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Site.Data;
using Site.Models.Produit;

namespace Site.Pages_Products
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }
        private readonly SiteContext _context;
        public List<SelectListItem> Options { get; set; }

        public CreateModel(SiteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Options = _context.Categories.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Options = _context.Categories.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
                return Page();
            }
            else
            {
                if (Image != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await Image.CopyToAsync(stream);
                        Product.Image = stream.ToArray();
                    }
                }
                _context.Product.Add(Product);
                await _context.SaveChangesAsync();
                int id = Product.Id;
                _context.EveryProduct.Add(
                    new EveryProduct
                    {
                        Id = id,
                        Name = Product.Name,
                        Description = Product.Description,
                        Price = Product.Price,
                        Image = Product.Image
                    }
                );
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
        }
    }
}