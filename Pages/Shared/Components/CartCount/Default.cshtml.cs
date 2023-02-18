using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Site.Models.Panier;
using Site.Services;

namespace Site.Pages.Shared
{
    public class CartCount : ViewComponent
    {
        public IEnumerable<CartItem> Cart { get; set; }
        private readonly Cart _cart;

        public CartCount(Cart cart)
        {
            _cart = cart;
        }
        public void OnGet()
        {
            Cart = this._cart.GetShoppingCartItems();
        }

        public ViewViewComponentResult Invoke()
        {
            var items = _cart.GetShoppingCartItems();
            _cart.PanierItem = items;
            
            var model = new CartCount(_cart)
            {
                Cart = items
            };
            return View(model);
        }
    }
}