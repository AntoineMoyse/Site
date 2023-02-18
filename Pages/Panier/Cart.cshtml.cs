using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Services;
using Site.Models.Panier;
using Microsoft.AspNetCore.Authorization;
using Site.Models.Produit;
using Site.Data;
using Microsoft.EntityFrameworkCore;

namespace Site.Pages.Panier
{
    [AllowAnonymous]
    public class CartModel : PageModel
    {
        public IEnumerable<CartItem> Cart { get; set; }
        public decimal CartTotal { get; set; }
        public IEnumerable<Product> Produits { get; set; }
        private readonly SiteContext _context;
        private readonly IProduct _productservice;
        private readonly Cart _cart;

        public CartModel(IProduct productService, Cart Cart, SiteContext context)
        {
            _productservice = productService;
            _cart = Cart;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Cart = this._cart.GetShoppingCartItems();
            CartTotal = _cart.GetShoppingCartTotal();
            Produits = await _context.Product.ToListAsync();
        }

        public IActionResult OnGetAdd(int id, int? amount)
        {
            var produit = _productservice.GetById(id);
            bool isValidAmount = false;
            if (produit != null)
            {
                isValidAmount = _cart.AddToCart(produit, amount);
            }

            return RedirectToPage("Cart");
        }

        public IActionResult OnGetRemove(int produitId)
        {
            var produit = _productservice.GetById(produitId);
            if (produit != null)
            {
                _cart.RemoveFromCart(produit);
            }
            return RedirectToPage("Cart");
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, int amount)
        {
            Cart = this._cart.GetShoppingCartItems();
            CartTotal = _cart.GetShoppingCartTotal();
            Produits = await _context.Product.ToListAsync();
            if (Cart != null)
            {
                _cart.UpdateQty(id, _cart.Id, amount);
            }
            return RedirectToPage("Cart");
        }
        
        public IActionResult Back(string returnUrl="/")
        {
            return Redirect(returnUrl);
        }
    }
}
