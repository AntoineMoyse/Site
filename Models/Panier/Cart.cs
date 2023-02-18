using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Site.Data;
using Site.Models.Produit;

namespace Site.Models.Panier
{
    public class Cart
    {
        public string Id { get; set; }

        public IEnumerable<CartItem> PanierItem { get; set; }

        private readonly SiteContext _context;

        public Cart(SiteContext context)
        {
            _context = context;
        }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
			var context = services.GetService<SiteContext>();
			string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

			session.SetString("CartId", cartId);
			return new Cart(context) { Id = cartId };
        }

        public bool AddToCart(Product produit, int? qty)
        {
            if(qty==0)
            {
                return false;
            }
            var panieritem = _context.ItemPanier.SingleOrDefault(s => s.Produit.Id == produit.Id && s.PanierId == Id);
            if (panieritem == null)
            {
                panieritem = new CartItem
                {
                    PanierId = Id,
                    Produit = produit,
                    Quantite = qty ?? default(int)
                };
                _context.ItemPanier.Add(panieritem);
            }
            else
            {
                panieritem.Quantite += qty ?? default(int);
            }
            _context.SaveChanges();
            return true;
        }

        public void RemoveFromCart(Product produit)
        {
            var panieritem = _context.ItemPanier.SingleOrDefault(s => s.Produit.Id == produit.Id && s.PanierId == Id);
            if(panieritem != null)
            {
                if(panieritem.Quantite > 1)
                {
                    panieritem.Quantite--;
                }
                else
                {
                    _context.ItemPanier.Remove(panieritem);
                }
            }
            _context.SaveChanges();
        }

        public IEnumerable<CartItem> GetShoppingCartItems()
        {
            return PanierItem ?? (PanierItem = _context.ItemPanier.Where(c => c.PanierId == Id).Include(s => s.Produit));
        }

        public void ClearCart()
        {
            var cartItems = _context.ItemPanier.Where(cart => cart.PanierId == Id);
            _context.ItemPanier.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            return _context.ItemPanier.Where(c => c.PanierId == Id).Select(c => c.Produit.Price * c.Quantite).Sum();
        }

        public void UpdateQty(int produitid, string cartid, int NewQty)
        {
            _context.ItemPanier.FirstOrDefault(i => i.PanierId == cartid && i.Produit.Id == produitid).Quantite = NewQty;
            _context.SaveChanges();
        }
    }
}